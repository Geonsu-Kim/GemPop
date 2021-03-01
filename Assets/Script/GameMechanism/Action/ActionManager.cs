using UnityEngine;
using System.Collections;
using System;

public class ActionManager
{
    private bool mRunning;

    private Transform mParent;
    private Stage mStage;
    private MonoBehaviour mMono;

    Returnable<bool> swiped = new Returnable<bool>(false);

    Returnable<bool> nextMatchable = new Returnable<bool>(false);

    Returnable<bool> matched = new Returnable<bool>(false);
    Returnable<bool> checkAgain = new Returnable<bool>(true);
    public bool MRunning { get { return mRunning; } }
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
             swiped.value =false;
            SoundManager.Instance.PlaySFX("BlockSwipe");
            yield return mStage.CoDoSwipeAction(row, col, dir, swiped);
            if (swiped.value)
            {
                matched.value = false;
                yield return EvaluateBoard(matched);
                if (!matched.value)
                {
                    yield return mStage.CoDoSwipeAction(row, col, dir, swiped);
                }
                else
                {
                    nextMatchable.value = false;
                    mStage.MInfo.SubMoveCnt();
                    yield return mStage.CheckDeadlock(nextMatchable);
                    if (!nextMatchable.value)
                    {
                        StageUIManager.Instance.AlarmDeadlock(true);
                        yield return YieldInstructionCache.WaitForSeconds(1.5f);
                        mStage.ResetDeadlock(mParent);
                        StageUIManager.Instance.AlarmDeadlock(false);

                    }
                    mStage.CheckGameEnd();
                }
            }
            mRunning = false;
        }
        yield break;
    }
    private IEnumerator EvaluateBoard(Returnable<bool> matched)
    {
        do
        {
            checkAgain.value = false;
            yield return mStage.Evaluate(checkAgain);
            if (checkAgain.value)
            {
                matched.value = true;
                yield return mStage.PostprocessAfterEvaluate();
            }
        } while (checkAgain.value);


        yield break;
    }
}