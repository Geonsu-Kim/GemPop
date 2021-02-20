using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageBuilder
{
    private int mStageNumber;
    private StageInfo mInfo;
    public StageBuilder(int stageNumber)
    {
        mStageNumber = stageNumber;
    }
    public Stage ComposeStage()
    {
        mInfo = LoadStage(mStageNumber);
        Stage stage = new Stage(mInfo.mRow, mInfo.mCol,this);
        for (int i = 0; i < mInfo.mRow; i++)
        {
            for (int j = 0; j < mInfo.mCol; j++)
            {
                stage.MBlocks[i, j] = SpawnBlockForStage(i, j);
                stage.MCells[i, j] = SpawnCellForStage(i, j);
            }
        }
        return stage;
    }
    public StageInfo LoadStage(int stageNum)
    {
        StageInfo info = StageReader.LoadStage(stageNum);

        return info;
    }
    public Stage BuildStage(int stageNum)
    {
        return ComposeStage();
    }
    Block SpawnBlockForStage(int row,int col)
    {
        if(mInfo.GetCellType(row,col)==CellType.EMPTY)
            return BlockFactory.SpawnBlock(BlockType.EMPTY);
        return BlockFactory.SpawnBlock(BlockType.BASIC);
    }
    Cell SpawnCellForStage(int row,int col)
    {
        return CellFactory.SpawnCell(mInfo,row,col);
    }
}
