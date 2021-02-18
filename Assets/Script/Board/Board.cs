using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board
{
    private int mRow; public int MRow { get { return mRow; } }
    private int mCol; public int MCol { get { return mCol; } }
    private BoardShuffler mShuffler; public BoardShuffler MShuffler { get { return mShuffler; } }
    private Cell[,] mCells; public Cell[,] MCells { get { return mCells; } }
    private Block[,] mBlocks; public Block[,] MBlocks { get { return mBlocks; } }
    private StageBuilder mBuilder;

    private List<Block> matchedBlocks = new List<Block>();
    private List<Block> clearBlocks = new List<Block>();
    private List<KeyValuePair<int, int>> border = new List<KeyValuePair<int, int>>();

    private List<KeyValuePair<int, int>> emptyRemainBlocks = new List<KeyValuePair<int, int>>();
    private SortedList<int, int> emptyBlocks = new SortedList<int, int>();

    public Board(int row, int col,StageBuilder builder)
    {
        mRow = row;
        mCol = col;
        mCells = new Cell[row, col];
        mBlocks = new Block[row, col];
        mBuilder = builder;
        mShuffler = new BoardShuffler(this, true);
    }
    public void ComposeStage(Transform parent)
    {
        float x = SetPosX(0.5f);
        float y = SetPosY(0.5f);
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

    public IEnumerator Evaluate(Returnable<bool> matched)
    {
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
                Block block = MBlocks[i, j];
                block?.DoEvaluation(i, j);
            }
        }
        clearBlocks.Clear();
        for (int i = 0; i < mRow; i++)
        {
            for (int j = 0; j < mCol; j++)
            {
                Block block = MBlocks[i, j];
                if (block != null)
                {
                    if (block.MStatus == BlockStatus.CLEAR)
                    {
                        clearBlocks.Add(block);
                        mBlocks[i, j] = null;
                    }
                }
            }
        }
        for (int i = 0; i < clearBlocks.Count; i++)
        {
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
                    ceiling = i+1;
                    border.Add(new KeyValuePair<int, int>(floor, ceiling));
                    do
                    {
                        i++;
                    } while (i<mRow&&mCells[i, j].MType == CellType.EMPTY);
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
        float y = SetPosY(0.5f)+ ceiling;
        GameObject blockObj = BlockCellPoolManager.Instance.GetBlock();
        Block block = BlockFactory.RespawnBlock(blockObj.GetComponent<BlockObj>().MBlock,BlockType.BASIC);
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

}