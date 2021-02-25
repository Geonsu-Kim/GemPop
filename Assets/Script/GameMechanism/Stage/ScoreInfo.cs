using System.Collections;
using System.Collections.Generic;

public class ScoreInfo
{
    private int mScore;
    private int mCurMoveCnt;
    private int mCurScore;
    public int MScore { get { return mScore; } }
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

    }
    public void GetScore(int score)
    {
        mCurScore += score;

        float ratio = (float)mCurScore / (float)mScore;
        UIManager.Instance.RenewScore(mCurScore, ratio);

    }
    public void CheckGameEnd()
    {
        if (!(CheckScore()| CheckMoveCnt()))
        {
            SoundManager.Instance.PlaySFX("StageFail");
            UIManager.Instance.ResultWindowOn(false);
        }
        else if (CheckScore())
        {
            SoundManager.Instance.PlaySFX("StageClear");
            UIManager.Instance.ResultWindowOn(true);
        }
    }
    public bool CheckMoveCnt()
    {
        return mCurMoveCnt >= 1;
    }
    public bool CheckScore()
    {
        return mCurScore > mScore;
    }



}
