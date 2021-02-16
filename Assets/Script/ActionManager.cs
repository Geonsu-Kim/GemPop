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
            Returnable<bool> swiped = new Returnable<bool>(false);
            yield return mStage.CoDoSwipeAction(row, col, dir, swiped);
            if (swiped.value)
            {
                Returnable<bool> matched = new Returnable<bool>(false);
                yield return EvaluateBoard(matched);
                if (!matched.value)
                {
                    yield return mStage.CoDoSwipeAction(row, col, dir, swiped);
                }
            }
            mRunning = false;
        }
        yield break;
    }
    private IEnumerator EvaluateBoard(Returnable<bool> matched)
    {
        yield return mStage.Evaluate(matched);
        if (matched.value)
        {
            yield return mStage.PostprocessAfterEvaluate();
        }
    }
}