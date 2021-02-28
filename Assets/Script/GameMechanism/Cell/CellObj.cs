using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellObj : MonoBehaviour
{ 
    private Cell mCell; public Cell MCell { set { mCell = value; } get { return mCell; } }
    private SpriteRenderer mSprite;
    void Awake()
    {
        mSprite = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        UpdateView(false);
    }
    public void UpdateView(bool pValueChanged)
    {
        if (mCell.IsEmpty())
        {
            mSprite.sprite = null;
        }
    }
}
