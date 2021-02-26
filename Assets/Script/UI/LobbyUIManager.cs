using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LobbyUIManager : SingletonBase<LobbyUIManager>
{
    public GameObject stageButton;
    public Transform StageSelectMenu;
    public void Start()
    {
        for (int i = 0; i < StageInfoList.recordList.Count; i++)
        {
            if (i != 0 && !StageInfoList.recordList[i - 1].MClear) break;
            GameObject newBtn = Instantiate(stageButton, StageSelectMenu);
            newBtn.GetComponent<StageButton>().MStageNum = i + 1;
            newBtn.GetComponent<StageButton>().SetText();
        }
    }
   public void OnExitButton()
    {
        Application.Quit();
    }
}
