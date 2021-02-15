using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage
{
    private int mRow;  public int MRow { get { return mRow; } }
    private int mCol;  public int MCol { get { return MCol; } }
    private Board mBoard; public Board MBoard { get { return mBoard; } }
    public Block[,] MBlocks { get { return mBoard.MBlocks; } }
    public Cell[,] MCells { get { return mBoard.MCells; } }

    public Stage(int row,int col)
    {
        mBoard = new Board(row, col);
    }
    public void ComposeStage(Transform parent)
    {
        mBoard.ComposeStage(parent);
    }
}
