using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : SingletonBase<UIManager>
{
    public RectTransform canvas;
    public RectTransform RectBG;
    public RectTransform RectGrayBG;
    public void Awake()
    {
        RectGrayBG.sizeDelta = new Vector2(canvas.rect.width, canvas.rect.height);
    }
}
