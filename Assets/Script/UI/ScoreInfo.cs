using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreInfo
{
    private int mScore;
    private int mCurMoveCnt;
    private int mCurScore;

    public ScoreInfo(int moveCnt,int score)
    {
        mScore = score;
        mCurMoveCnt = moveCnt;
        mCurScore = 0;
        UIManager.Instance.RenewMoveCnt(moveCnt);
        UIManager.Instance.RenewScore(0,0f);

    }
    public void SubMoveCnt()
    {
        mCurMoveCnt--;
        UIManager.Instance.RenewMoveCnt(mCurMoveCnt);
        if (mCurMoveCnt <= 0)
        {

        }
    }
    public void GetScore(int score)
    {
        mCurScore += score;

        float ratio = (float)mCurScore / (float)mScore;
        UIManager.Instance.RenewScore(mCurScore, ratio);
        if (mCurScore >= mScore)
        {

        }
    }



}
