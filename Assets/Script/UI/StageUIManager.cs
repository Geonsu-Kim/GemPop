using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class StageUIManager : SingletonBase<StageUIManager>
{
    private const string textClear = "Stage Clear!";
    private const string textFail = "Stage Fail...";



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

    public BackGroundConfig config;
    public void Start()
    {
        SetBackGround();

    }
    public void SetBackGround()
    {
        BackGround.sprite = config.sprites[StageInfoList.GetNumber()-1];
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
            Time.timeScale = 0;
        }
        else
        {

            Panel_Pause.SetActive(false);
            Time.timeScale = 1;
        }
    }
    public void ResultWindowOn(bool isClear)
    {
        Time.timeScale = 0;
        Panel_Result.SetActive(true);
        if (isClear)
        {
            Btn_NextStage.SetActive(true);
            Text_StageClear.text = textClear;
        }
        else 
        { 
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
