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
        mRow = row;
        mCol = col;
        mBoard = new Board(mRow, mCol);
    }
    public void ComposeStage(Transform parent)
    {
        mBoard.ComposeStage(parent);
    }

    public bool IsValidSwipe(int row, int col, Swipe dir)
    {
        switch (dir)
        {
            case Swipe.RIGHT:
                return col < mCol - 1;
            case Swipe.UP:
                return row < mRow - 1;
            case Swipe.LEFT:
                return col > 0;
            case Swipe.DOWN:
                return row > 0;
            default:
                return false;
        }
    }

    internal bool IsOnValidBlock(Vector2 point, out Vector2 blockPos)
    {
        Vector2 pos = new Vector2(point.x + (mCol / 2.0f), point.y + (mRow / 2.0f));
        int row = (int)pos.y;
        int col = (int)pos.x;
        blockPos = new Vector2(pos.x, pos.y);
        return mBoard.IsSwipeable(row, col);
    }

    public IEnumerator CoDoSwipeAction(int row, int col, Swipe dir, Returnable<bool> swiped)
    {
        swiped.value = false;
        int swipedRow = row, swipedCol = col;
        switch (dir)
        {
            case Swipe.RIGHT:
                swipedCol++;
                break;
            case Swipe.UP:
                swipedRow++;
                break;
            case Swipe.LEFT:
                swipedCol--;
                break;
            case Swipe.DOWN:
                swipedRow--;
                break;
        }
        if (mBoard.IsSwipeable(row, col))
        {
            Block targetBlock = MBlocks[swipedRow, swipedCol];
            Block baseBlock = MBlocks[row, col];
            Vector3 basePos = baseBlock.MObj.transform.position;
            Vector3 targetPos = targetBlock.MObj.transform.position;
            if (targetBlock.IsSwipeable(baseBlock))
            {
                baseBlock.MoveTo(targetPos, 0.3f);
                targetBlock.MoveTo(basePos, 0.3f);
                yield return YieldInstructionCache.WaitForSeconds(0.5f);
                MBlocks[row, col] = targetBlock;
                MBlocks[swipedRow, swipedCol] = baseBlock;
                swiped.value = true; ;

            }
        }
    }

    public bool IsInsideBoard(Vector2 point)
    {
        Vector2 pos = new Vector2(point.x + (mCol / 2.0f), point.y + (mRow / 2.0f));
        if (pos.y < 0 || pos.y > mRow || pos.x < 0 || pos.x > mCol) return false;
        return true;
    }
}
