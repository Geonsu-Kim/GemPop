using System;
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

    internal bool IsOnValidBlock(Vector2 point, out Vector2 blockPos)
    {
        Vector2 pos = new Vector2(point.x + (mCol / 2.0f), point.y + (mRow / 2.0f));
        int row = (int)pos.y;
        int col = (int)pos.x;
        blockPos = new Vector2(pos.x, pos.y);
        return mBoard.IsSwipeable(row, col);
    }

    internal bool IsInsideBoard(Vector2 point)
    {
        Vector2 pos = new Vector2(point.x + (mCol / 2.0f), point.y + (mRow / 2.0f));
        if (pos.y < 0 || pos.y > mRow || pos.x < 0 || pos.x > mCol) return false;
        return true;
    }
}
