using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : SingletonBase<UIManager>
{
    private const string textClear = "Stage Clear!";
    private const string textFail = "Stage Fail...";




    public TextMeshProUGUI Text_MoveCnt;
    public TextMeshProUGUI Text_Score;
    public TextMeshProUGUI Text_ScoreRet;
    public TextMeshProUGUI Text_StageClear;
    public Image Bar_Score;
    public GameObject Panel_Pause;
    public GameObject Panel_Result;
    public GameObject Btn_NextStage;
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
}
