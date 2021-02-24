using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StageInfoList
{
    public static List<StageInfo> InfoList= new List<StageInfo>();
    public static void Initialization()
    {
        int stageCnt = 0;
        
        while (true)
        {
            StageInfo info=StageReader.LoadStage(stageCnt);
            if (info == null) break;
            InfoList.Add(info);
        }
    }
}
