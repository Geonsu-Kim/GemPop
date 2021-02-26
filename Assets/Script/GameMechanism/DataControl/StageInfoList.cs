using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StageInfoList
{
    public static List<StageInfo> InfoList= new List<StageInfo>();
    public static List<StageRecord> recordList = new List<StageRecord>();
    public static bool binit = false;
    static int selectedNumber=1;
    public static void Initialization()
    {
        if (binit) return;
        binit = true;
        int stageCnt = 0;
        
        while (true)
        {
            StageInfo info=StageReader.LoadStage(++stageCnt);
            if (info == null) break;

            InfoList.Add(info);
            recordList.Add(new StageRecord());
        }
        StageReader.Load();
    }

    public static int GetNumber()
    {
        return selectedNumber;
    }
    public static void SetNumber(int num)
    {
        selectedNumber = num;
    }
    public static void RenewRecord(int score)
    {
        if (recordList[selectedNumber-1].MBestScore < score)
        {
            recordList[selectedNumber-1].MBestScore = score;
        }
        recordList[selectedNumber-1].MClear = true;
    }
}
