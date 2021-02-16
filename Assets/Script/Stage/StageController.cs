using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    private int mStageNumber;
    private bool mInit;
    private bool mTouchDown;

    private Vector2 startPos;
    private Stage mStage;
    private StageBuilder mBuilder;
    private InputManager mInput;
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
    }
    void OnInputHandler()
    {
        if (!mTouchDown && mInput.isDown)
        {
            Vector2 point = mInput.PosToBoard;
            if (!mStage.IsInsideBoard(point)) return;
            Vector2 blockPos;
            if(mStage.IsOnValidBlock(point,out blockPos))
            {
                mTouchDown = true;
                startPos = point;
            }
        }
        else if (mTouchDown && mInput.isUp)
        {
            Vector2 point = mInput.PosToBoard;
            Swipe dir = mInput.EvalSwipeDir(startPos, point);
            mTouchDown = false;
            }
    }
}
