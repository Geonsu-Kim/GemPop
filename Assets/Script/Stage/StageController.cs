using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    private int mStageNumber;
    private bool mInit;
    private Stage mStage;
    private StageBuilder mBuilder;

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
        BuildStage();
    }
    void BuildStage()
    {
        mBuilder = new StageBuilder(1);
        mStage = mBuilder.BuildStage(1);
        mStage.ComposeStage(this.transform);
    }
}
