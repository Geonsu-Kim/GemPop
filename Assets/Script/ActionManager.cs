using UnityEngine;
using System.Collections;
using System;

public class ActionManager
{
    private bool mRunning;

    private Transform mParent;
    private Stage mStage;
    private MonoBehaviour mMono;


    public ActionManager(Transform parent, Stage stage)
    {
        mParent = parent;
        mStage = stage;
        mMono = mParent.GetComponent<MonoBehaviour>();
    }
    public Coroutine StartCoroutine(IEnumerator routine)
    {
        return mMono.StartCoroutine(routine);
    }
    public void DoSwipeAction(int row, int col, Swipe dir)
    {
        if (mStage.IsValidSwipe(row, col, dir))
        {
            StartCoroutine(CoDoSwipeAction(row, col, dir));
        }
    }

    private IEnumerator CoDoSwipeAction(int row, int col, Swipe dir)
    {
        if (!mRunning)
        {
            mRunning = true;
            Returnable<bool> swiped =new Returnable<bool>(false);
            yield return mStage.CoDoSwipeAction(row, col, dir, swiped);
            mRunning = false;
        }
        yield break;
    }
}