﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
public class StageUIManager : SingletonBase<StageUIManager>
{
    private const string textClear = "Stage Clear!";
    private const string textFail = "Stage Fail...";
    private bool onResume = false;
    private int continueCount= 2;
    public bool OnResume { get { return onResume; } }

    public UnityEvent RetryEvent;

    public TextMeshProUGUI Text_MoveCnt;
    public TextMeshProUGUI Text_Score;
    public TextMeshProUGUI Text_ScoreRet;
    public TextMeshProUGUI Text_StageClear;
    public TextMeshProUGUI Text_StageTitle;

    public Image Bar_Score;
    public Image BackGround;
    public GameObject Panel_Pause;
    public GameObject Panel_Result;
    public GameObject Btn_NextStage;
    public GameObject Btn_ContinueStage;

    public BackGroundConfig config;
    public void Start()
    {
        SetBackGround();

    }
    public void SetBackGround()
    {
        BackGround.sprite = config.sprites[(StageInfoList.GetNumber()-1)%config.sprites.Length];
    }
    public void RenewMoveCnt(int moveCnt)
    {
        Text_MoveCnt.text = string.Format("MOVE\n{0:D2}", moveCnt.ToString());
    }
    public void RenewScore(int score,float ratio)
    {
        Text_Score.text = string.Format("SCORE : {0:D6}",score.ToString());
        Bar_Score.fillAmount = ratio;
    }
    public void Pause(bool p)
    {
        if (p)
        {
            Panel_Pause.SetActive(true);
            onResume = true;
        }
        else
        {

            Panel_Pause.SetActive(false); onResume = false;
        }
    }
    public void ContinueGame()
    {
        continueCount--;
        onResume = false;
        Panel_Result.SetActive(false);
        Btn_ContinueStage.SetActive(false);
        RetryEvent.Invoke();
        RenewMoveCnt(5);

    }
    public void ResultWindowOn(bool isClear)
    {
        onResume = true;
        Panel_Result.SetActive(true);
        if (isClear)
        {
            Btn_NextStage.SetActive(true);
            Text_StageClear.text = textClear;
        }
        else 
        {
            if (continueCount >= 1) Btn_ContinueStage.SetActive(true);
            else Btn_NextStage.SetActive(false);
            Btn_NextStage.SetActive(false);
            Text_StageClear.text = textFail;
        }
        Text_ScoreRet.text=string.Copy(Text_Score.text);
    }
    public void SetStageTitle(int num)
    {
        Text_StageTitle.text= string.Format("STAGE {0:D2}", num);
    }
    public void OnClickToLobby()
    {
        StageReader.Save();
        LoadingSceneManager.LoadScene("scLobby");
    }
    public void OnClickRetry()
    {
        StageReader.Save();
        LoadingSceneManager.LoadScene("scStage");
    }
    public void OnClickToNextStage()
    {

        StageReader.Save();
        if (StageInfoList.GetNumber() >= StageInfoList.InfoList.Count)
        {
            LoadingSceneManager.LoadScene("scLobby");
        }
        else
        {
            StageInfoList.SetNumber(StageInfoList.GetNumber() + 1);
            LoadingSceneManager.LoadScene("scStage");
        }
    }
}
