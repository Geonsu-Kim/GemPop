using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    private int mStageNumber;
    private bool mInit;
    private bool mTouchDown;

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
        mBuilder = new StageBuilder(1);
        mStage = mBuilder.BuildStage(1);
        mStage.ComposeStage(this.transform);
        mAction = new ActionManager(transform, mStage);
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

            mTouchDown = false;
        }
    }
}
