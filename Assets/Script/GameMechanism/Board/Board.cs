using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board
{
    private int mRow; public int MRow { get { return mRow; } }
    private int mCol; public int MCol { get { return mCol; } }
    private bool existItem;

    private Returnable<bool> checkDeadlockAgain;
    private BoardShuffler mShuffler; public BoardShuffler MShuffler { get { return mShuffler; } }
    private Cell[,] mCells; public Cell[,] MCells { get { return mCells; } }
    private Block[,] mBlocks; public Block[,] MBlocks { get { return mBlocks; } }
    private StageBuilder mBuilder;

    private List<Block> matchedBlocks = new List<Block>();
    private List<Block> clearBlocks = new List<Block>();
    private List<KeyValuePair<int, int>> border = new List<KeyValuePair<int, int>>();

    private List<KeyValuePair<int, int>> emptyRemainBlocks = new List<KeyValuePair<int, int>>();
    private SortedList<int, int> emptyBlocks = new SortedList<int, int>();

    public Board(int row, int col, StageBuilder builder)
    {
        mRow = row;
        mCol = col;
        mCells = new Cell[row, col];
        mBlocks = new Block[row, col];
        mBuilder = builder;
        mShuffler = new BoardShuffler(this, true);
        checkDeadlockAgain = new Returnable<bool>(false);
    }
    public void ComposeStage(Transform parent)
    {
        float x = SetPosX(0.5f);
        float y = SetPosY(0.5f);
        checkDeadlockAgain.value = false;
        do
        {
            mShuffler.Shuffle();
            CheckDeadlock(checkDeadlockAgain);
        } while (!checkDeadlockAgain.value);

        mShuffler.Shuffle();
        for (int i = 0; i < mRow; i++)
        {
            for (int j = 0; j < mCol; j++)
            {
                Cell cell = mCells[i, j]?.CallCellObj(mRow, i, j, parent);
                cell?.Move(x + j, y + i);
                Block block = mBlocks[i, j]?.CallBlockObj(parent);
                block?.Move(x + j, y + i);
            }
        }
    }
    public void ResetDeadlock(Transform parent)
    {
        float x = SetPosX(0.5f);
        float y = SetPosY(0.5f);
        checkDeadlockAgain.value = false;
        do
        {
            mShuffler.Shuffle();
            CheckDeadlock(checkDeadlockAgain);
        } while (!checkDeadlockAgain.value);
        for (int i = 0; i < mRow; i++)
        {
            for (int j = 0; j < mCol; j++)
            {
                Block block = mBlocks[i, j];
                block.Move(x + j, y + i);
            }
        }

    }

    public bool IsSwipeable(int row, int col)
    {
        return mCells[row, col].MType != CellType.EMPTY;
    }

    public bool CanShuffle(int row, int col)
    {
        if (mCells[row, col].MType == CellType.EMPTY) return false;
        return true;
    }
    public void ChangeBlock(Block block, BlockColor prevColor)
    {
        BlockColor newColor = BlockColor.NA;
        do
        {
            newColor = (BlockColor)Random.Range(0, 6);
        } while (prevColor == BlockColor.NA || prevColor == newColor);
    }
    public float SetPosX(float offset)
    {
        return -mCol / 2.0f + offset;

    }
    public float SetPosY(float offset)
    {
        return -mRow / 2.0f + offset;

    }

    public IEnumerator Evaluate(Returnable<bool> matched,Returnable<int> returnScore)
    {
        existItem = false;
        bool matchedBlockFound = UpdateAllBlocksMatchedStatus();
        if (!matchedBlockFound)
        {
            matched.value = false;
            yield break;
        }
        for (int i = 0; i < mRow; i++)
        {
            for (int j = 0; j < mCol; j++)
            {
                mBlocks[i, j]?.DoEvaluation(i, j);
            }
        }
        clearBlocks.Clear();
        for (int i = 0; i < mRow; i++)
        {
            for (int j = 0; j < mCol; j++)
            {
                if (mBlocks[i, j] != null)
                {

                    CheckBlockType(mBlocks[i, j].MType, mBlocks[i, j].MStatus, i, j, returnScore);

                }
            }
        }
        if (existItem) SoundManager.Instance.PlaySFX("PopStartItem");
        else SoundManager.Instance.PlaySFX("PopStartNormal");
        for (int i = 0; i < clearBlocks.Count; i++)
        {
            clearBlocks[i].PopAction();
        }
        yield return YieldInstructionCache.WaitForSeconds(0.5f);
        if (existItem) SoundManager.Instance.PlaySFX("PopItem");
        else SoundManager.Instance.PlaySFX("PopNormal");
        for (int i = 0; i < clearBlocks.Count; i++)
        {
            if (clearBlocks[i].MStatus == BlockStatus.CLEAR)
            {
                ParticlePool.Instance.GetParticle(0, clearBlocks[i].MObj.transform.position);
            }
            else
            {
                ParticlePool.Instance.GetParticle(1,clearBlocks[i].MObj.transform.position);
            }
            clearBlocks[i].MObj.gameObject.SetActive(false);
        }
        matched.value = true;
        yield break;
    }

    public IEnumerator SpawnBlocksAfterClear(List<Block> movingBlocks)
    {

        for (int j = 0; j < mCol; j++)
        {
            border.Clear();
            int floor = 0;
            int ceiling = mRow;
            for (int i = 0; i < mRow; i++)
            {
                if (mCells[i, j].MType == CellType.EMPTY)
                {
                    ceiling = i + 1;
                    border.Add(new KeyValuePair<int, int>(floor, ceiling));
                    do
                    {
                        i++;
                    } while (i < mRow && mCells[i, j].MType == CellType.EMPTY);
                    floor = i;
                }
            }

            border.Add(new KeyValuePair<int, int>(floor, mRow));

            for (int idx = 0; idx < border.Count; idx++)
            {
                for (int i = border[idx].Key; i < border[idx].Value; i++)
                {
                    if (mBlocks[i, j] == null)
                    {
                        int topRow = i;
                        int spawnBaseY = border[idx].Key;
                        for (int y = topRow; y < border[idx].Value; y++)
                        {
                            if (mBlocks[y, j] != null || !CanBlockBeAllocatable(y, j)) continue;
                            Block block = SpawnBlockWithDrop(y, j, spawnBaseY, j, ceiling);
                            if (block != null)
                                movingBlocks.Add(block);
                            spawnBaseY++;
                        }
                        break;
                    }
                }
            }

        }
        yield return null;
    }

    private Block SpawnBlockWithDrop(int row, int col, int spawnRow, int spawnCol, int ceiling)
    {
        float x = SetPosX(0.5f);
        float y = SetPosY(0.5f) + ceiling;
        GameObject blockObj = BlockCellPoolManager.Instance.GetBlock();
        Block block = BlockFactory.RespawnBlock(blockObj.GetComponent<BlockObj>().MBlock, BlockType.BASIC);
        if (block != null)
        {
            mBlocks[row, col] = block;
            block.Move(x + (float)spawnCol, y + (float)spawnRow);
            blockObj.SetActive(true);
            block.dropDistance = new Vector2(spawnCol - col, ceiling + (spawnRow - row));
        }
        return block;
    }

    public IEnumerator ArrangeBlocksAfterClear(List<KeyValuePair<int, int>> unfilledBlocks, List<Block> movingBlocks)
    {
        emptyRemainBlocks.Clear();
        for (int j = 0; j < mCol; j++)
        {
            emptyBlocks.Clear();
            for (int i = 0; i < mRow; i++)
            {
                if (CanBlockBeAllocatable(i, j))
                    emptyBlocks.Add(i, i);
            }
            if (emptyBlocks.Count == 0)
                continue;
            int firstValue = emptyBlocks.Values[0];
            for (int i = firstValue + 1; i < mRow; i++)
            {
                if (mCells[i, j].MType == CellType.EMPTY) break;
                Block block = mBlocks[i, j];
                if (block == null || mCells[i, j].MType == CellType.EMPTY) continue;
                block.dropDistance = new Vector2(0, i - firstValue);
                movingBlocks.Add(block);
                mBlocks[firstValue, j] = block;
                mBlocks[i, j] = null;
                emptyBlocks.RemoveAt(0);
                emptyBlocks.Add(i, i);
                firstValue = emptyBlocks.Values[0];
                i = firstValue;
            }
        }
        yield return null;
        if (emptyRemainBlocks.Count > 0)
        {
            unfilledBlocks.AddRange(emptyRemainBlocks);
        }

    }

    private bool CanBlockBeAllocatable(int row, int col)
    {
        if (mCells[row, col].MType == CellType.EMPTY)
            return false;
        return mBlocks[row, col] == null;
    }

    private bool UpdateAllBlocksMatchedStatus()
    {
        matchedBlocks.Clear();
        int cnt = 0;
        for (int i = 0; i < mRow; i++)
        {
            for (int j = 0; j < mCol; j++)
            {
                if (CheckMatched(i, j))
                {
                    cnt++;
                }
            }
        }
        return cnt > 0;
    }

    private bool CheckMatched(int row, int col)
    {
        bool found = false;
        Block block = mBlocks[row, col];
        if (block == null) return false;
        if (block.MMatch != MatchType.NONE || block.MType == BlockType.EMPTY || mCells[row, col].MType == CellType.EMPTY)
            return false;
        matchedBlocks.Add(block);
        for (int i = col + 1; i < mCol; i++)
        {
            if (mBlocks[row, i] == null || !mBlocks[row, i].IsEqual(block)) break;
            matchedBlocks.Add(mBlocks[row, i]);
        }
        for (int i = col - 1; i >= 0; i--)
        {
            if (mBlocks[row, i] == null || !mBlocks[row, i].IsEqual(block)) break;
            matchedBlocks.Add(mBlocks[row, i]);
        }
        if (matchedBlocks.Count >= 3)
        {
            found = true;
            for (int i = 0; i < matchedBlocks.Count; i++)
            {
                matchedBlocks[i].UpdateMatchType((MatchType)matchedBlocks.Count);
            }
        }
        matchedBlocks.Clear();
        matchedBlocks.Add(block);
        for (int i = row + 1; i < mRow; i++)
        {
            if (mBlocks[i, col] == null || !mBlocks[i, col].IsEqual(block)) break;
            matchedBlocks.Add(mBlocks[i, col]);
        }
        for (int i = row - 1; i >= 0; i--)
        {
            if (mBlocks[i, col] == null || !mBlocks[i, col].IsEqual(block)) break;
            matchedBlocks.Add(mBlocks[i, col]);
        }
        if (matchedBlocks.Count >= 3)
        {
            found = true;
            for (int i = 0; i < matchedBlocks.Count; i++)
            {
                matchedBlocks[i].UpdateMatchType((MatchType)matchedBlocks.Count);
            }
        }

        matchedBlocks.Clear();
        return found;
    }
    public void CheckBlockType(BlockType type, BlockStatus status, int row, int col,Returnable<int>returnScore)
    {
        if (status != BlockStatus.CLEAR) return;
        returnScore.value += 500;
        Block block;
        switch (type)
        {
            case BlockType.BASIC:
                block = mBlocks[row, col];
                if (block == null || clearBlocks.Contains(block)) break;
                clearBlocks.Add(block);
                mBlocks[row, col] = null;
                break;
            case BlockType.VERTICAL:
                existItem = true;
                returnScore.value += 1000;
                for (int i = 0; i < mRow; i++)
                {
                    block = mBlocks[i, col];
                    if (block == null || clearBlocks.Contains(block) || mCells[i, col].MType == CellType.EMPTY) continue;

                    clearBlocks.Add(block);
                    mBlocks[i, col] = null;
                    CheckBlockType(block.MType, block.MStatus, i, col, returnScore);
                }
                break;
            case BlockType.HORIZON:
                existItem = true;
                returnScore.value += 200;
                for (int i = 0; i < mCol; i++)
                {
                    block = mBlocks[row, i];
                    if (block == null || clearBlocks.Contains(block) || mCells[row, i].MType == CellType.EMPTY) continue;

                    clearBlocks.Add(block);
                    mBlocks[row, i] = null;
                    CheckBlockType(block.MType, block.MStatus, row, i, returnScore);

                }
                break;/*
            case BlockType.SQUARE:
                for (int i = row - 1; i <= row + 1; i++)
                {
                    for (int j = col - 1; j <= col + 1; j++)
                    {
                        if (i < 0 || i >= mRow || j < 0 || j > mCol || clearBlocks.Contains(mBlocks[row, i])||mBlocks[i,j]==null||mCells[i,j].MType==CellType.EMPTY) continue;

                        clearBlocks.Add(mBlocks[i, j]);
                        mBlocks[i, j] = null;
                    }
                }
                break;
            case BlockType.SAMECOLOR:
                for (int i = 0; i < mRow; i++)
                {
                    for (int j = 0; j < mCol; j++)
                    {
                        if (clearBlocks.Contains(mBlocks[i, j]) || mBlocks[i, j] == null || !mBlocks[i, j].IsEqual(mBlocks[row, col])|| mCells[i, j].MType == CellType.EMPTY) continue;
                        
                            clearBlocks.Add(mBlocks[i, j]);
                        mBlocks[i, j] = null;

                    }
                }
                break;*/
        }
    }
    public void CheckDeadlock(Returnable<bool> matchable)
    {
        for (int i = 0; i < mRow; i++)
        {
            for (int j = 0; j < mCol; j++)
            {
                if (j + 1 < mCol)
                {
                    if (mCells[i, j].MType == CellType.EMPTY || mCells[i, j + 1].MType == CellType.EMPTY) continue;
                    matchable.value = CheckMatchableHorz(mBlocks[i, j], mBlocks[i, j + 1], i, j, i, j + 1);
                }
                if (matchable.value) return;
                if (i + 1 < mRow)
                {
                    if (mCells[i, j].MType == CellType.EMPTY || mCells[i+1, j].MType == CellType.EMPTY) continue;
                    matchable.value = matchable.value || CheckMatchableVert(mBlocks[i, j], mBlocks[i+1, j], i, j, i+1, j);
                }
                if (matchable.value) return;
            }
        }
        return;
    }
    public bool CheckMatchableBlockNavigation(out int row1, out int col1, out int row2, out int col2)
    {
        row1 = 0; col1 = 0;row2 = 0;col2 = 0;
        bool detected = false;
        for (int i = 0; i < mRow; i++)
        {
            for (int j = 0; j < mCol; j++)
            {
                if (j + 1 < mCol)
                {
                    if (mCells[i, j].MType == CellType.EMPTY || mCells[i, j + 1].MType == CellType.EMPTY) continue;
                    detected = CheckMatchableHorz(mBlocks[i, j], mBlocks[i, j + 1], i, j, i, j + 1);
                }
                if (detected) {
                    row1 = i;col1 = j;row2 = i;col2 = j + 1;
                    return true;
                }
                if (i + 1 < mRow)
                {
                    if (mCells[i, j].MType == CellType.EMPTY || mCells[i + 1, j].MType == CellType.EMPTY) continue;
                    detected = detected || CheckMatchableVert(mBlocks[i, j], mBlocks[i + 1, j], i, j, i + 1, j);
                }
                if (detected)
                {
                    row1 = i;col1 = j;row2 = i+1;col2 = j;
                return true;
                }
            }
        }
        return false;
    }
    public bool CheckMatchableHorz(Block block1,Block block2, int row1, int col1,int row2,int col2)
    {
        return CheckMatchableRight(block1,row1,col1)|| CheckMatchableLeft(block2,row2,col2);
    }
    public bool CheckMatchableVert(Block block1, Block block2, int row1, int col1, int row2, int col2)
    {
        return CheckMatchableUp(block1, row1, col1) || CheckMatchableDown(block2, row2, col2);
    }
    public bool CheckMatchableRight(Block block, int row, int col)
    {
        if (col + 3 < mCol)
        {
            if (block.IsMatchable(mBlocks[row, col + 2], mBlocks[row, col + 3])) return true;
        }
        if (col + 1 < mCol)
        {
            if (row + 2 < mRow)
            {
                if (block.IsMatchable(mBlocks[row+1, col + 1], mBlocks[row+2, col + 1])) return true;
            }
            if (row >= 2)
            {
                if (block.IsMatchable(mBlocks[row - 1, col + 1], mBlocks[row - 2, col + 1])) return true;
            }
            if(row+1<mRow&&row>=1)
            {
                if (block.IsMatchable(mBlocks[row - 1, col + 1], mBlocks[row + 1, col + 1])) return true;
            }
        }
        return false;
    }
    public bool CheckMatchableLeft(Block block, int row, int col)
    {
        if (col >= 3)
        {
            if (block.IsMatchable(mBlocks[row, col - 2], mBlocks[row, col - 3])) return true;
        }
        if (col >= 1)
        {
            if (row + 2 < mRow)
            {
                if (block.IsMatchable(mBlocks[row + 1, col - 1], mBlocks[row + 2, col - 1])) return true;
            }
            if (row >= 2)
            {
                if (block.IsMatchable(mBlocks[row - 1, col - 1], mBlocks[row - 2, col - 1])) return true;
            }
            if (row + 1 < mRow && row >= 1)
            {
                if (block.IsMatchable(mBlocks[row - 1, col - 1], mBlocks[row + 1, col - 1])) return true;
            }
        }
        return false;
    }
    public bool CheckMatchableUp(Block block, int row, int col)
    {
        if (row + 3 < mRow)
        {
            if (block.IsMatchable(mBlocks[row+2, col], mBlocks[row+3, col])) return true;
        }
        if (row + 1 < mRow)
        {
            if (col + 2 < mCol)
            {
                if (block.IsMatchable(mBlocks[row + 1, col + 1], mBlocks[row + 1, col + 2])) return true;
            }
            if (col >= 2)
            {
                if (block.IsMatchable(mBlocks[row + 1, col - 1], mBlocks[row +1, col - 2])) return true;
            }
            if (col + 1 < mCol && col >= 1)
            {
                if (block.IsMatchable(mBlocks[row + 1, col - 1], mBlocks[row + 1, col + 1])) return true;
            }
        }
        return false;
    }
    public bool CheckMatchableDown(Block block, int row, int col)
    {
        if (row  >=3)
        {
            if (block.IsMatchable(mBlocks[row - 2, col], mBlocks[row - 3, col])) return true;
        }
        if (row >=1)
        {
            if (col + 2 < mCol)
            {
                if (block.IsMatchable(mBlocks[row - 1, col + 1], mBlocks[row - 1, col + 2])) return true;
            }
            if (col >= 2)
            {
                if (block.IsMatchable(mBlocks[row - 1, col - 1], mBlocks[row - 1, col - 2])) return true;
            }
            if (col + 1 < mCol && col >= 1)
            {
                if (block.IsMatchable(mBlocks[row - 1, col - 1], mBlocks[row - 1, col + 1])) return true;
            }
        }
        return false;
    }
    public bool ChMatchableUp (Block block, int row, int col)
    {
        return false;
    }
}