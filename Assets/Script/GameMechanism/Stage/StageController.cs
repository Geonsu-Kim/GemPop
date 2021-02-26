using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    private int mStageNumber;
    private bool mInit;
    private bool mTouchDown;
    private bool mCheckedNavi;

    private Vector2 startPos;
    private Vector2 blockPos;
    private Stage mStage;
    private StageBuilder mBuilder;
    private InputManager mInput;
    private ActionManager mAction;
    public int MStageNumber { get { return mStageNumber; } set { mStageNumber = value; } }
    void Start()
    {
        InitStage();
        StartCoroutine(CheckIdle());
    }
    void InitStage()
    {
        if (mInit)
            return;
        mInit = true;
        mInput = new InputManager(transform);
        BuildStage();
    }
    void BuildStage()
    {
        mBuilder = new StageBuilder(StageInfoList.GetNumber());
        mStage = mBuilder.BuildStage();
        mStage.ComposeStage(this.transform);
        mAction = new ActionManager(transform, mStage);
        StageUIManager.Instance.SetStageTitle(StageInfoList.GetNumber());
    }
    private void Update()
    {
        OnInputHandler();
    }
    void OnInputHandler()
    {
        if (!mTouchDown && mInput.isDown)
        {
            Vector2 point = mInput.PosToBoard;
            if (!mStage.IsInsideBoard(point)) return;
            if (mStage.IsOnValidBlock(point, out blockPos))
            {
                mTouchDown = true;
                startPos = point;
            }
        }
        else if (mTouchDown && mInput.isUp)
        {
            Vector2 point = mInput.PosToBoard;
            Swipe dir = mInput.EvalSwipeDir(startPos, point);
            if(dir!=Swipe.NA)
            {
                mAction.DoSwipeAction((int)blockPos.y, (int)blockPos.x, dir);
            }
            mCheckedNavi = false;
            mTouchDown = false;
        }
    }
    IEnumerator CheckIdle()
    {
        float t = 0;
        while (true)
        {
            yield return null;
            if (!mTouchDown&&!mAction.MRunning)
            {
                t += Time.deltaTime;
            }
            else
            {
                t = 0;
            }
            if (t > 3f)
            {
                t = 0;
                mStage.CheckMatchableNavigation(mCheckedNavi);
                mCheckedNavi = true;
            }
        }
    }
}
