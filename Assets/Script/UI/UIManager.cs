using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : SingletonBase<UIManager>
{
    public RectTransform canvas;
    public RectTransform RectBG;
    public RectTransform RectGrayBG;

    public TextMeshProUGUI Text_MoveCnt;
    public TextMeshProUGUI Text_Score;
    public Image Bar_Score;
    public GameObject PauseWindow;
    public void RenewMoveCnt(int moveCnt)
    {
        Text_MoveCnt.text = string.Format("MOVE\n{0:D2}", moveCnt.ToString());
    }
    public void RenewScore(int score,float ratio)
    {
        Text_Score.text = string.Format("SCORE : {0:D6}",score.ToString());
        Bar_Score.fillAmount = ratio;
    }
}
