using UnityEngine;
public static class BlockFactory
{
    public static Block SpawnBlock(BlockType type)
    {
        Block block = new Block(type);
        if (type == BlockType.BASIC)
        {
            block.MColor = (BlockColor)Random.Range(0, 6);
        }
        else
        {
            block.MColor = BlockColor.NA;
        }
        return block;
    }
    public static Block RespawnBlock(Block block, BlockType type)
    {
        block.Respawn(type);
        if (type == BlockType.BASIC)
        {
            block.MColor = (BlockColor)Random.Range(0, 6);
        }
        else
        {
            block.MColor = BlockColor.NA;
        }
        block.MObj.UpdateView(false);
        return block;
    }

}