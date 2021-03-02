using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System;
public static class PlayerData
{
    private static int playerScore=0;

    private static int playerClearCount=0;
    public static void RenewData(int score)
    {
        playerScore += score;
        playerClearCount++;
        RenewLeaderBoard();
        RenewAchievement();
        Save();
    }
    public static int GetScore()
    {
        return playerScore;
    }
    public static int GetCount()
    {
        return playerClearCount;
    }
    public static void Save()
    {
        XmlDocument XmlDoc = new XmlDocument();
        XmlElement XmlEl = XmlDoc.CreateElement("PlayerDB");
        XmlDoc.AppendChild(XmlEl);
            XmlElement ElementSetting = XmlDoc.CreateElement("Player");
            if (ElementSetting == null)
            {
                StageInfoList.recordList.Add(new StageRecord());
            }
            ElementSetting.SetAttribute("PlayerScore", playerScore.ToString());
            ElementSetting.SetAttribute("PlayerClearCount", playerClearCount.ToString());
            XmlEl.AppendChild(ElementSetting);
        
        XmlDoc.Save(Application.persistentDataPath + "/PlayerData.xml");

    }
    public static void Load()
    {

        string path = Application.persistentDataPath + "/PlayerData.xml";
        if (!System.IO.File.Exists(path))
        {
            Save();
            return;
        }
        XmlDocument XmlDoc = new XmlDocument();
        XmlDoc.Load(path);
        XmlElement xmlElement = XmlDoc["PlayerDB"];
        foreach (XmlElement node in xmlElement.ChildNodes)
        {
            playerScore = Convert.ToInt32(node.GetAttribute("PlayerScore"));
            playerClearCount = Convert.ToInt32(node.GetAttribute("PlayerClearCount"));
            
        }
    }
    public static void RenewLeaderBoard()
    {
        Social.ReportScore(playerScore, GPGSIds.leaderboard_scoreranking, (bool b) => { });
    }
    public static void RenewAchievement()
    {
        Social.ReportProgress(GPGSIds.StageFirstClear, 100f, (bool b) => { });
        Social.ReportProgress(GPGSIds.Stage5Times, (float)playerClearCount/5f*100f, (bool b) => { });
        Social.ReportProgress(GPGSIds.Stage10Times, (float)playerClearCount / 10f * 100f, (bool b) => { });
        Social.ReportProgress(GPGSIds.Get100Ts, (float)playerScore / 100000f * 100f, (bool b) => { });
    }
}
