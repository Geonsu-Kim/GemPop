using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BlockType
{
    EMPTY = 0,
    BASIC,
    VERTICAL,
    HORIZON/*,
    SQUARE,
    SAMECOLOR*/
}
public enum BlockColor
{
    NA = -1, RED, GREEN, BLUE, CYAN, MAGENTA, YELLOW
}
public enum BlockStatus
{
    NORMAL, MATCH, CLEAR
}
public enum MatchType
{
    NONE = 0,
    THREE = 3,
    FOUR,
    FIVE,
    THREETHREE,
    THREEFOUR,
    THREEFIVE,
    FOURFIVE,
    FOURFOUR
}
public class Block
{
    protected BlockType mType; public BlockType MType { get { return mType; } set { mType = value; } }
    protected BlockStatus mStatus; public BlockStatus MStatus { get { return mStatus; } }
    protected BlockColor mColor; public BlockColor MColor { get { return mColor; } set { mColor = value; } }
    protected MatchType mMatch; public MatchType MMatch { get { return mMatch; } }
    protected BlockObj mObj;
    public BlockObj MObj
    {
        get { return mObj; }
        set { mObj = value; mObj.MBlock = this; }
    }
    protected BlockActionObj mActionObj;
    protected Vector2Int mVtDuplicate;
    public bool isMoving
    {
        get
        {
            return mObj != null && mActionObj.isMoving;
        }

    }
    public Vector2 dropDistance
    {
        set
        {
            mActionObj?.MoveDrop(value);
        }
    }

    public int MVtDuplicateX { get { return mVtDuplicate.x; } set { mVtDuplicate.x = value; } }
    public int MVtDuplicateY { get { return mVtDuplicate.y; } set { mVtDuplicate.y = value; } }


    public Block(BlockType type)
    {
        mType = type;
        mStatus = BlockStatus.NORMAL;
        mMatch = MatchType.NONE;
        mColor = BlockColor.NA;
    }
    public void Respawn(BlockType type)
    {
        mType = type;
        mStatus = BlockStatus.NORMAL;
        mMatch = MatchType.NONE;
        mColor = BlockColor.NA;
    }
    public Block CallBlockObj(Transform parent)
    {
        if (mType == BlockType.EMPTY) return null;
        GameObject newObj = BlockCellPoolManager.Instance.GetBlock();
        newObj.transform.parent = parent;
        MObj = newObj.GetComponent<BlockObj>();
        mActionObj = newObj.GetComponent<BlockActionObj>();
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
    public bool IsMatchable(Block t1,Block t2)
    {
        return mColor == t1.MColor && mColor == t2.MColor;
    }
    public bool IsSwipeable(Block baseBlock)
    {
        return true;
    }
    public void MoveTo(Vector3 targetPos, float duration)
    {
        mObj.StartCoroutine(Action2D.MoveTo(mObj.transform, targetPos, duration));
    }

    public void DoEvaluation(int i, int j)
    {
        if (mStatus == BlockStatus.CLEAR || mType == BlockType.EMPTY) return;
        else if (mStatus == BlockStatus.MATCH)
        {
            mStatus = BlockStatus.CLEAR;
            return;
        }
        else
        {
            mStatus = BlockStatus.NORMAL;
            mMatch = MatchType.NONE;
            return;
        }
    }

    public void UpdateMatchType(MatchType type)
    {
        mStatus = BlockStatus.MATCH;
        if (mMatch == MatchType.FOUR && type == MatchType.FOUR)
            mMatch = MatchType.FOURFOUR;
        else
            mMatch = (MatchType)((int)mMatch + (int)type);
    }
    public void PopAction()
    {
        if (mStatus == BlockStatus.CLEAR)
        {
            mActionObj.PopAction();
        }
    }
}

