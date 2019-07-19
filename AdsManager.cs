using System;
using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;
using System.Collections.Generic;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class AdsManager : MonoBehaviour
{
    private static AdsManager _instance;
    private AdmobScript admob;
    public BannerSizeAndPosition bannerSizePos = BannerSizeAndPosition.SmartBannerTop;
    public string bannerId = "ca-app-pub-3940256099942544/6300978111";
    public string interstitialId = "ca-app-pub-3940256099942544/1033173712";
    public string androidGameId = "3219434";
    public bool unityTestAdsOn;
    string placementId = "rewardedVideo";
	
    public enum BannerSizeAndPosition {
        BannerTopCenter,
        BannerTopRight,
        BannerTopLeft,
        SmartBannerTop,
        BannerBottomCenter,
        BannerBottomRight,
        BannerBottomLeft,
        SmartBannerBottom,
    };

	public static AdsManager Instance
	{
		get
		{
			if(_instance==null)
			{
				#if !UNITY_EDITOR
				_instance= GameObject.FindObjectOfType<AdsManager>();
				DontDestroyOnLoad(_instance.gameObject);
				#endif
			}
			return _instance;
		}
	}	
	
	void Awake()
	{
		#if !UNITY_EDITOR
            if(_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                if(this != _instance)
                    Destroy(this.gameObject);
            }
		#endif
	}

    // Start is called before the first frame update
    void Start()
    {
        //------------------- Admob Initialization ----------------------------------//
        InitAdmob(bannerId, interstitialId);
        //---------------------------------------------------------------------------//

        //---------- ONLY NECESSARY FOR UnityAds ASSET PACKAGE INTEGRATION: ----------//
        if (Advertisement.isSupported) {
            Advertisement.Initialize (androidGameId, unityTestAdsOn); // Unity test ads
            // Advertisement.Initialize (androidGameId, false); // For Unity ads UnComment this line and Comment Above Line
        }
        //-------------------------------------------------------------------//

    }

    #region Admob
        public void InitAdmob(string banID, string interID) {
            admob = AdmobScript.Instance();     // this instance of AdmobScript is used in all placeses
            admob.InitializeAdmob(bannerId, interstitialId, bannerSizePos);
        }

        public void ShowBanner() {
            admob.ShowBanner();
        }

        public void HideBanner() {
            admob.HideBanner();
        }

        public void ShowInterstitial() {
            admob.ShowInterstitial();
        }
    #endregion

    #region UnityAds
        public void ShowUnityAds() {
            ShowAd();
        }

        private void ShowAd ()
        {
            ShowOptions options = new ShowOptions();
            options.resultCallback = HandleShowResult;

            Advertisement.Show(placementId, options);
        }

        void HandleShowResult (ShowResult result)
        {
            if(result == ShowResult.Finished) {
            Debug.Log("Video completed - Offer a reward to the player");

            }else if(result == ShowResult.Skipped) {
                Debug.LogWarning("Video was skipped - Do NOT reward the player");

            }else if(result == ShowResult.Failed) {
                Debug.LogError("Video failed to show");
            }
        }
    #endregion
    
    public void ShowUnityOrAdmobAd() {
        #if !UNITY_EDITOR
            if (Advertisement.IsReady()) {
                ShowUnityAds();
            } else {
                ShowInterstitial();
            }
		#endif
    }


}
