using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : IUnityAdsListener
{

// Changes the gameId and type string if IOS or Android
    #if UNITY_IOS
        string gameId = "4554459";
        string type = "iOS";
    #else   
        string gameId = "4554458";
        string type = "Android";
    #endif

    public AdsManager() {
        Advertisement.Initialize(gameId);
    }

// Plays Interstitial ad; Quick Ad
    public void PlayAd() {
       if(Advertisement.IsReady("Interstitial_" + type)) {
           Advertisement.Show("Interstitial_" + type);
       } else 
            Debug.Log("Ads not ready");
   }

// Plays Reward ad; Longer Ad
   public void PlayRewardAd() {
       if(Advertisement.IsReady("Rewarded_" + type)) {
           Advertisement.Show("Rewarded_" + type);
       } else 
            Debug.Log("Ads not ready");
   }

// Necessary Methods for Ads 
    public void OnUnityAdsReady(string placementId) {
        throw new System.NotImplementedException();
    }

    public void OnUnityAdsDidError(string message) {
        Debug.Log("ERROR: " + message);
    }

    public void OnUnityAdsDidStart(string placementId) {
        Debug.Log("VIDEO STARTED");
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult) {
        if(placementId == "Reward" && showResult == ShowResult.Finished) {
            Debug.Log("PLAYER REWARDED");
        }
    }
}
