using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage
{
    private int mRow;  public int MRow { get { return mRow; } }
    private int mCol;  public int MCol { get { return MCol; } }
    private Board mBoard; public Board MBoard { get { return mBoard; } }
    private ScoreInfo mInfo; public ScoreInfo MInfo { get { return mInfo; } }
    private List<Block> movingBlocks = new List<Block>();
    private List<KeyValuePair<int, int>> unfilledBlocks = new List<KeyValuePair<int, int>>();
    private Returnable<int> returnScore=new Returnable<int>(0);
    public Block[,] MBlocks { get { return mBoard.MBlocks; } }
    public Cell[,] MCells { get { return mBoard.MCells; } }

    int row1, col1, row2, col2;
    public Stage(int row,int col, StageBuilder builder,int score,int moveCnt)
    {
        mRow = row;
        mCol = col;
        mBoard = new Board(mRow, mCol,builder);
        mInfo = new ScoreInfo(moveCnt, score);
    }
    public void ComposeStage(Transform parent)
    {
        mBoard.ComposeStage(parent);
    }
    public void ResetDeadlock(Transform parent)
    {
        mBoard.ResetDeadlock(parent);
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
        if (mBoard.IsSwipeable(row, col)&& mBoard.IsSwipeable(swipedRow, swipedCol))
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

    public IEnumerator PostprocessAfterEvaluate()
    {
        unfilledBlocks.Clear();
        movingBlocks.Clear();
        yield return mBoard.ArrangeBlocksAfterClear(unfilledBlocks, movingBlocks);
        yield return mBoard.SpawnBlocksAfterClear(movingBlocks);
        yield return WaitForDropping(movingBlocks);
    }

    private IEnumerator WaitForDropping(List<Block> movingBlocks)
    {
        bool bContinue = false;
        do
        {
            bContinue = false;
            for (int i = 0; i < movingBlocks.Count; i++)
            {
                if (movingBlocks[i].isMoving)
                {
                    bContinue = true;
                    break;
                }
            }
            yield return YieldInstructionCache.WaitForSeconds(0.05f);
        } while (bContinue);
    }

    public IEnumerator Evaluate(Returnable<bool> matched)
    {
        returnScore.value = 0;
        yield return mBoard.Evaluate(matched,returnScore);
        mInfo.GetScore(returnScore.value);
    }
    public IEnumerator CheckDeadlock(Returnable<bool> deadlock)
    {
         mBoard.CheckDeadlock(deadlock);
        yield break;
    }
    public bool IsInsideBoard(Vector2 point)
    {
        Vector2 pos = new Vector2(point.x + (mCol / 2.0f), point.y + (mRow / 2.0f));
        if (pos.y < 0 || pos.y > mRow || pos.x < 0 || pos.x > mCol) return false;
        return true;
    }
    public void CheckMatchableNavigation(bool checkedNavi)
    {
        if (checkedNavi)
        {
            MBlocks[row1, col1].NaviAction();
            MBlocks[row2, col2].NaviAction();
        }
        bool p = mBoard.CheckMatchableBlockNavigation(out row1, out col1, out row2, out col2);
        if (p)
        {
            MBlocks[row1, col1].NaviAction();
            MBlocks[row2, col2].NaviAction();

        }

    }
    public void CheckGameEnd()
    {
        mInfo.CheckGameEnd();
    }
    public void ContinueGame()
    {
        mInfo.ContinueGame();
    }
}
