using System.Collections.Generic;
using UnityEngine;
using BlockPosPair = System.Collections.Generic.KeyValuePair<Block, UnityEngine.Vector2Int>;
public class BoardShuffler
{
    private Board mBoard;
    private bool mLoadingMode;
    private bool mListComplete;

    private SortedList<int, BlockPosPair> mOrgBlocks = new SortedList<int, BlockPosPair>();
    private IEnumerator<KeyValuePair<int, BlockPosPair>> mIterator;
    private Queue<BlockPosPair> mUnusedBlock = new Queue<BlockPosPair>();
    public BoardShuffler(Board board, bool loadingMode)
    {
        mBoard = board;
        mLoadingMode = loadingMode;
    }
    public void Shuffle(bool animation = false)
    {
        PrepareDuplicationData();
        PrepareShuffleBlocks();
        RunShuffle(animation);
    }
    void PrepareDuplicationData()
    {
        for (int i = 0; i < mBoard.MRow; i++)
        {
            for (int j = 0; j < mBoard.MCol; j++)
            {
                Block block = mBoard.MBlocks[i, j];
                if (block == null) continue;
                if (mBoard.CanShuffle(i, j)) block.ResetDuplicationInfo();
                else
                {
                    block.MVtDuplicateX = 1;
                    block.MVtDuplicateY = 1;
                    if (j > 0 && !mBoard.CanShuffle(i, j - 1) && mBoard.MBlocks[i, j - 1] != null && mBoard.MBlocks[i, j - 1].IsEqual(block))
                    {
                        block.MVtDuplicateX = 2;
                        mBoard.MBlocks[i, j - 1].MVtDuplicateX = 2;
                    }
                    if (i > 0 && !mBoard.CanShuffle(i - 1, j) && mBoard.MBlocks[i - 1, j] != null && mBoard.MBlocks[i - 1, j].IsEqual(block))
                    {
                        block.MVtDuplicateY = 2;
                        mBoard.MBlocks[i - 1, j].MVtDuplicateY = 2;
                    }
                }
            }
        }
    }
    void PrepareShuffleBlocks()
    {
        int randNum = 0;
        for (int i = 0; i < mBoard.MRow; i++)
        {
            for (int j = 0; j < mBoard.MCol; j++)
            {
                if (!mBoard.CanShuffle(i, j)) continue;
                while (true)
                {
                    randNum = Random.Range(0, 10000);
                    if (mOrgBlocks.ContainsKey(randNum))
                        continue;
                    mOrgBlocks.Add(randNum, new BlockPosPair(mBoard.MBlocks[i, j], new Vector2Int(i, j)));
                    break;
                }

            }
        }
        mIterator = mOrgBlocks.GetEnumerator();
    }
    void RunShuffle(bool anim)
    {
        for (int i = 0; i < mBoard.MRow; i++)
        {
            for (int j = 0; j < mBoard.MCol; j++)
            {
                if (!mBoard.CanShuffle(i, j)) continue;
                mBoard.MBlocks[i, j] = GetShuffleBlock(i, j);
            }
        }
    }
    Block GetShuffleBlock(int row, int col)
    {
        BlockColor prevColor = BlockColor.NA;
        Block firstBlock = null;
        bool useQueue = true;
        while (true)
        {
            BlockPosPair blockInfo = NextBlock(useQueue);
            Block block = blockInfo.Key;
            if (block == null)
            {
                blockInfo = NextBlock(true);
                block = blockInfo.Key;
            }
            if (prevColor == BlockColor.NA)
            {
                prevColor = block.MColor;
            }
            if (mListComplete)
            {
                if (firstBlock == null)
                {
                    firstBlock = block;
                }
                else if (ReferenceEquals(firstBlock, block))
                {
                    mBoard.ChangeBlock(block, prevColor);
                }
            }
            Vector2Int vtDup = CalcDuplications(row, col, block);
            if (vtDup.x > 2 || vtDup.y > 2)
            {
                mUnusedBlock.Enqueue(blockInfo);
                useQueue = mListComplete || !useQueue;
                continue;
            }
            block.MVtDuplicateX = vtDup.x;
            block.MVtDuplicateY = vtDup.y;
            if (block.MObj != null)
            {
                float x = mBoard.SetPosX(0.5f);
                float y = mBoard.SetPosY(0.5f);
                block.Move(x + col, y + row);
            }
            return block;
        }
    }
    BlockPosPair NextBlock(bool _useQueue)
    {
        if (_useQueue && mUnusedBlock.Count > 0) return mUnusedBlock.Dequeue();
        if (!mListComplete && mIterator.MoveNext()) return mIterator.Current.Value;
        mListComplete = true;
        return new BlockPosPair(null, Vector2Int.zero);
    }
    Vector2Int CalcDuplications(int row, int col, Block block)
    {
        int colDup = 1, rowDup = 1;
        if (col > 0 && mBoard.MBlocks[row, col - 1] != null && mBoard.MBlocks[row, col - 1].IsEqual(block))
        {
            colDup += mBoard.MBlocks[row, col - 1].MVtDuplicateX;
        }
        if (row > 0 && mBoard.MBlocks[row - 1, col] != null && mBoard.MBlocks[row - 1, col].IsEqual(block))
        {
            rowDup += mBoard.MBlocks[row - 1, col].MVtDuplicateY;
        }
        if (col < mBoard.MCol - 1 && mBoard.MBlocks[row, col + 1] != null && mBoard.MBlocks[row, col + 1].IsEqual(block))
        {
            colDup += mBoard.MBlocks[row, col + 1].MVtDuplicateX;
            if (mBoard.MBlocks[row, col + 1].MVtDuplicateX == 2)
                mBoard.MBlocks[row, col + 1].MVtDuplicateX = 2;
        }
        if (row < mBoard.MRow - 1 && mBoard.MBlocks[row + 1, col] != null && mBoard.MBlocks[row + 1, col].IsEqual(block))
        {
            rowDup += mBoard.MBlocks[row + 1, col].MVtDuplicateY;
            if (mBoard.MBlocks[row + 1, col].MVtDuplicateY == 2)
                mBoard.MBlocks[row + 1, col].MVtDuplicateY = 2;
        }
        return new Vector2Int(colDup, rowDup);
    }
}