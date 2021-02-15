using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board
{
    private int mRow; public int MRow { get { return mRow; } }
    private int mCol; public int MCol { get { return MCol; } }
    private Cell[,] mCells; public Cell[,] MCells { get { return mCells; } }
    private Block[,] mBlocks; public Block[,] MBlocks { get { return mBlocks; } }
    public Board(int row,int col)
    {
        mRow = row;
        mCol = col;
        mCells=new Cell[row,col];
        mBlocks = new Block[row, col];
    }
    public void ComposeStage(Transform parent)
    {
        float x = SetPosX(0.5f);
        float y = SetPosY(0.5f);

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

    public float SetPosX(float offset)
    {
        return -mCol / 2.0f + offset;

    }
    public float SetPosY(float offset)
    {
        return -mRow / 2.0f + offset;
        
    }

}
