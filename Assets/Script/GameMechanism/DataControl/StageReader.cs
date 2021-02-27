using System.IO;
using System;
using System.Xml;
using UnityEngine;

public static class StageReader
{
    public static StageInfo LoadStage(int stageNumber)
    {
        StageInfo info = null;
        if (Application.platform == RuntimePlatform.Android)
        {
            string path = Application.streamingAssetsPath+"/Stage/"+GetFileName(stageNumber) + ".json";
            if (path == null) return null;
            WWW reader = new WWW(path);
            while (!reader.isDone) { }
            string str = reader.text;
            info = JsonUtility.FromJson<StageInfo>(str);
        }
        else
        {

            TextAsset textAsset = Resources.Load<TextAsset>($"Stage/{GetFileName(stageNumber)}");
            if (textAsset == null) return null;
            info = JsonUtility.FromJson<StageInfo>(textAsset.text);
        }
        return info;
    }
    static string GetFileName(int stageNumber)
    {
        return string.Format("{0:D2}", stageNumber);
    }
    public static void Save()
    {
        XmlDocument XmlDoc = new XmlDocument();
        XmlElement XmlEl= XmlDoc.CreateElement("StageRecord");
        XmlDoc.AppendChild(XmlEl);
        for (int i = 0; i < StageInfoList.InfoList.Count; i++)
        { 

            XmlElement ElementSetting = XmlDoc.CreateElement($"Stage{GetFileName(i+1)}");
            if (ElementSetting == null)
            {
                StageInfoList.recordList.Add(new StageRecord());
            }
            ElementSetting.SetAttribute("BestScore", StageInfoList.recordList[i].MBestScore.ToString());
            ElementSetting.SetAttribute("Clear", StageInfoList.recordList[i].MClear.ToString());
            XmlEl.AppendChild(ElementSetting);
        }
        XmlDoc.Save(Application.persistentDataPath + "/StageRecord.xml");

    }
    public static void Load()
    {

        string path = Application.persistentDataPath + "/StageRecord.xml";
        if (!System.IO.File.Exists(path))
        {
            Save();
            return;
        }
        XmlDocument XmlDoc = new XmlDocument();
        XmlDoc.Load(path);
        XmlElement xmlElement = XmlDoc["StageRecord"];
        int idx = 0;
        foreach (XmlElement node in xmlElement.ChildNodes)
        {
            StageInfoList.recordList[idx].MBestScore = Convert.ToInt32(node.GetAttribute("BestScore"));
            StageInfoList.recordList[idx].MClear = Convert.ToBoolean(node.GetAttribute("Clear"));

            idx++;
        }
    }

}
