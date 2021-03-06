﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GooglePlayGames;
public class InitGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayGamesPlatform.Activate();
        Social.localUser.Authenticate((bool _bSuccess) =>
        {
            if (true == _bSuccess)
            {
                Social.ReportProgress(GPGSIds.Login, 100f, (bool b) => { });
            }
            else
            {
            }
        });
        PlayerData.Load();
        StageInfoList.Initialization();

        SceneManager.LoadScene("scLobby");
    }
}
