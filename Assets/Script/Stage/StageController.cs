using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    private bool mInit;
    private Stage mStage;
    private StageBuilder mBuilder;

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
        mBuilder = new StageBuilder(0);
        mStage = mBuilder.BuildStage(0, 9, 9);
        mStage.ComposeStage(this.transform);
    }
}
