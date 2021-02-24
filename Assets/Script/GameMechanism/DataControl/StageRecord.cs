using System;
using UnityEngine;

[Serializable]
public class StageRecord
{
    private int mBestScore;
    public int MBestScore { get { return mBestScore; } set { mBestScore = value; } }
    public StageRecord(int score)
    {
        mBestScore = score;
    }
}
