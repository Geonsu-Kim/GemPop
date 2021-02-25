using System;
using UnityEngine;

[Serializable]
public class StageRecord
{
    private int mBestScore;
    private bool mClear;
    public int MBestScore { get { return mBestScore; } set { mBestScore = value; } }
    public bool MClear { get { return mClear; } set { mClear = value; } }
    public StageRecord(int score=0,bool clear=false)
    {
        mBestScore = score;
        mClear = clear;
    }
    public void Print()
    {
        Debug.Log(mBestScore + " " + mClear);
    }
}
