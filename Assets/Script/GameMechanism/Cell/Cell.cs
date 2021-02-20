using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CellType
{
    EMPTY=0,
    BASIC,
    FIXTURE,
    GEM
}
public class Cell
{
    protected CellType mType; public CellType MType { get { return mType; } set { mType = value; } }
    protected CellObj mObj;
    public CellObj MObj
    {
        get { return mObj; }
        set { mObj = value; mObj.MCell=this; }
    }
    public Cell(CellType type)
    {
        mType = type;
    }

    public Cell CallCellObj(int maxRow,int row,int col,Transform parent)
    {
        GameObject newObj = BlockCellPoolManager.Instance.Pool_Cell[maxRow * row + col];
        newObj.transform.parent = parent;
        MObj = newObj.GetComponent<CellObj>();
        newObj.SetActive(true);
        return this;
    }

    public void Move(float x,float y)
    {
        mObj.transform.position = new Vector3(x,y);
    }
}
