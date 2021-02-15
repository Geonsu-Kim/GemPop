using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BlockType
{
    EMPTY = 0,
    BASIC,
}
public class Block
{
    protected BlockType mType; public BlockType MType { get { return mType; } set { mType = value; } }
    public Block(BlockType type)
    {
        mType = type;
    }
}

