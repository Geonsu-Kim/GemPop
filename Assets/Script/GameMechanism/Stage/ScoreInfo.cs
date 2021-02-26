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
        StageUIManager.Instance.RenewMoveCnt(moveCnt);
        StageUIManager.Instance.RenewScore(0,0f);

    }
    public void SubMoveCnt()
    {
        mCurMoveCnt--;
        StageUIManager.Instance.RenewMoveCnt(mCurMoveCnt);

    }
    public void GetScore(int score)
    {
        mCurScore += score;

        float ratio = (float)mCurScore / (float)mScore;
        StageUIManager.Instance.RenewScore(mCurScore, ratio);

    }
    public void CheckGameEnd()
    {
        if (!(CheckScore()| CheckMoveCnt()))
        {
            SoundManager.Instance.PlaySFX("StageFail");
            StageUIManager.Instance.ResultWindowOn(false);

        }
        else if (CheckScore())
        {
            SoundManager.Instance.PlaySFX("StageClear");
            StageInfoList.RenewRecord(mCurScore);
            StageUIManager.Instance.ResultWindowOn(true);
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
