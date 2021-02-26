using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
public class StageButton : MonoBehaviour
{
    private int mStageNum = 0; public int MStageNum { get { return mStageNum; } set { mStageNum = value; } }
    private SFXButton btn;

    public TextMeshProUGUI StageText;
    void Start()
    {
        btn = GetComponent<SFXButton>();
        btn.onClick.AddListener(OnClick);
    }

    // Update is called once per frame
    void OnClick()
    {
        StageInfoList.SetNumber(mStageNum);
        LoadingSceneManager.LoadScene("scStage");
    }

    public void SetText()
    {
        StageText.text = string.Format("STAGE {0:D2}", mStageNum);
    }
}
