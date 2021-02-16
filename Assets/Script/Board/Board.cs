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
    public Board(int row,int col)
    {
        mRow = row;
        mCol = col;
        mCells=new Cell[row,col];
        mBlocks = new Block[row, col];
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
                Cell cell = mCells[i, j]?.CallCellObj(mRow,i,j,parent);
                cell?.Move(x+j,y+i);
                Block block = mBlocks[i, j]?.CallBlockObj(parent);
                block?.Move(x + j, y + i);
            }
        }
    }

    internal bool IsSwipeable(int row, int col)
    {
        return mCells[row, col].MType != CellType.EMPTY;
    }

    public bool CanShuffle(int row,int col)
    {
        if (mCells[row, col].MType == CellType.EMPTY) return false;
        return true;
    }
    public void ChangeBlock(Block block,BlockColor prevColor)
    {
        BlockColor newColor=BlockColor.NA;
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

}