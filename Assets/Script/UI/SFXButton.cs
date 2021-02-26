using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SFXButton : Button
{
    private const string buttonClick = "ButtonClick";
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        if (SoundManager.Instance!=null)
        {
            onClick.AddListener(ClickSound);
        }
    }
    void ClickSound()
    {
        SoundManager.Instance.PlaySFX(buttonClick);
    }

}
