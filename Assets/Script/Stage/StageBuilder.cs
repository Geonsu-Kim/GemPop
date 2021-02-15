using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageBuilder
{
    private int mStageNumber;
    public StageBuilder(int stageNumber)
    {
        mStageNumber = stageNumber;
    }
    public Stage ComposeStage(int row,int col)
    {
        Stage stage = new Stage(row, col);
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                stage.MBlocks[i, j] = SpawnBlockForStage(i, j);
                stage.MCells[i, j] = SpawnCellForStage(i, j);
            }
        }
        return stage;
    }
    public Stage BuildStage(int stageNum,int row,int col)
    {
        return ComposeStage(row, col);
    }
    Block SpawnBlockForStage(int row,int col)
    {
        return BlockFactory.SpawnBlock(BlockType.BASIC);
    }
    Cell SpawnCellForStage(int row,int col)
    {
        return new Cell(CellType.BASIC);
    }
}
