using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BlockType
{
    EMPTY = 0,
    BASIC,
}
public enum BlockColor
{
    NA=-1,RED,GREEN,BLUE,CYAN,MAGENTA,YELLOW
}
public class Block
{
    protected BlockType mType; public BlockType MType { get { return mType; } set { mType = value; } }
    protected BlockColor mColor; public BlockColor MColor { get { return mColor; } set { mColor = value; } }
    protected BlockObj mObj;
    public BlockObj MObj
    {
        get { return mObj; }
        set { mObj = value; mObj.MBlock = this; }
    }
    protected Vector2Int mVtDuplicate;
    public int MVtDuplicateX { get { return mVtDuplicate.x; } set { mVtDuplicate.x = value; } }
    public int MVtDuplicateY { get { return mVtDuplicate.y; } set { mVtDuplicate.y = value; } }


    public Block(BlockType type)
    {
        mType = type;
    }
    public Block CallBlockObj(Transform parent)
    {
        if (mType == BlockType.EMPTY) return null;
        GameObject newObj = BlockCellPoolManager.Instance.GetBlock();
        newObj.transform.parent = parent;
        MObj = newObj.GetComponent<BlockObj>();
        newObj.SetActive(true);
        return this;
    }

    public void Move(float x, float y)
    {
        mObj.transform.position = new Vector3(x, y);
    }

    public void ResetDuplicationInfo()
    {
        mVtDuplicate = Vector2Int.zero;
    }
    public bool IsEqual(Block target)
    {
        if (mColor == target.MColor) return true;
        return false;
    }
}

