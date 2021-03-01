using System;
using UnityEngine;
using GoogleMobileAds.Api;
public class ADManager : SingletonBase<ADManager>
{
    private const string appId = "ca-app-pub-9482772848272703~5590578697";
    private const string rewardID = "ca-app-pub-9482772848272703/4126766869";
    private const string rewardTestID = "ca-app-pub-3940256099942544/5224354917";

    private RewardedAd rewardedAd;

    private bool rewarded = false;

    void Start()
    {
        rewardedAd = new RewardedAd(rewardTestID);
        AdRequest request = new AdRequest.Builder().Build();
        rewardedAd.LoadAd(request); // 광고 로드

        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        rewardedAd.OnAdClosed += HandleRewardedAdClosed;
    }

    void Update()
    {
        if (rewarded)
        {
            StageUIManager.Instance.ContinueGame();
            rewarded = false;
        }
    }

    public void UserChoseToWatchAd()
    {
        if (rewardedAd.IsLoaded()) 
        {
            rewardedAd.Show(); 
        }
    }

    public void CreateAndLoadRewardedAd() 
    {
        rewardedAd = new RewardedAd(rewardTestID);

        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        AdRequest request = new AdRequest.Builder().Build();
        rewardedAd.LoadAd(request);
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args) { 
        CreateAndLoadRewardedAd();  
    }

    private void HandleUserEarnedReward(object sender, Reward e) { 
        rewarded = true; 

    }
}
