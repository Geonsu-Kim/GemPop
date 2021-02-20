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
        int rand = Random.Range(0, 100);
        if (rand > 70)
        {
            block.MType = (BlockType)Random.Range(2, 4);
        }
        else
            block.MType = BlockType.BASIC;
        if (type != BlockType.EMPTY)
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