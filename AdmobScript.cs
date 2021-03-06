using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdmobScript : MonoBehaviour
{
    private string bannerId = "ca-app-pub-3940256099942544/6300978111";
    private string interstitialId = "ca-app-pub-3940256099942544/1033173712";
    public BannerView bannerView;
    public InterstitialAd interstitial;
    AdsManager.BannerSizeAndPosition bannerType;
    private RewardedAd rewardedAd;
    private float deltaTime = 0.0f;
    private static string outputMessage = string.Empty;

    private static AdmobScript _instance;	
    public static AdmobScript Instance()
    {
        if(_instance == null)
        {
            _instance = new AdmobScript();
            _instance.preInitAdmob ();
        }
        return _instance;
    }
    
    internal delegate void AdmobAdCallBack(string adtype, string eventName, string msg);
    private void preInitAdmob()
    {

    }

    public static string OutputMessage
    {
        set { outputMessage = value; }
    }

    public void Start()
    {
        // #if UNITY_ANDROID
        //         string appId = "ca-app-pub-3940256099942544~3347511713";
        // #elif UNITY_IPHONE
        //         string appId = "ca-app-pub-3940256099942544~1458002511";
        // #else
        //         string appId = "unexpected_platform";
        // #endif

        // MobileAds.SetiOSAppPauseOnBackground(true);

        // // Initialize the Google Mobile Ads SDK.
        // MobileAds.Initialize(appId);

        // this.CreateAndLoadRewardedAd();
    }

    public void InitializeAdmob(string bannerUnitId, string interstitialUnitId, AdsManager.BannerSizeAndPosition bannerTy) {
        bannerId = bannerUnitId;
        interstitialId = interstitialUnitId;
        bannerType = bannerTy;
        RequestBanner();
        RequestInterstitial();
    }

    public void Update()
    {
        // Calculate simple moving average for time to render screen. 0.1 factor used as smoothing
        // value.
        this.deltaTime += (Time.deltaTime - this.deltaTime) * 0.1f;
    }

    // Returns an ad request with custom ad targeting.
    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder()
            .AddTestDevice(AdRequest.TestDeviceSimulator)
            .AddTestDevice("0123456789ABCDEF0123456789ABCDEF")
            .AddKeyword("game")
            .SetGender(Gender.Male)
            .SetBirthday(new DateTime(1985, 1, 1))
            .TagForChildDirectedTreatment(false)
            .AddExtra("color_bg", "9B30FF")
            .Build();
    }

    public void RequestBanner()
    {
        // These ad units are configured to always serve test ads.
        #if UNITY_EDITOR
                string adUnitId = "unused";
        #elif UNITY_ANDROID
                string adUnitId = bannerId;
        #elif UNITY_IPHONE
                string adUnitId = bannerId;
        #else
                string adUnitId = "unexpected_platform";
        #endif

        // Clean up banner ad before creating a new one.
        if (this.bannerView != null) {
            this.bannerView.Destroy();
        }

        // Create a 320x50 banner at the top of the screen.
        if(bannerType == AdsManager.BannerSizeAndPosition.SmartBannerTop)
            this.bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Top);
        else if(bannerType == AdsManager.BannerSizeAndPosition.SmartBannerBottom)
            this.bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Bottom);
        else if(bannerType == AdsManager.BannerSizeAndPosition.BannerTopCenter)
            this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Top);
        else if(bannerType == AdsManager.BannerSizeAndPosition.BannerBottomCenter)
            this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
        else if(bannerType == AdsManager.BannerSizeAndPosition.BannerBottomLeft)
            this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.BottomLeft);
        else if(bannerType == AdsManager.BannerSizeAndPosition.BannerBottomRight)
            this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.BottomRight);
        else if(bannerType == AdsManager.BannerSizeAndPosition.BannerTopLeft)
            this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.TopLeft);
        else if(bannerType == AdsManager.BannerSizeAndPosition.BannerTopRight)
            this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.TopRight);

        // Register for ad events.
        this.bannerView.OnAdLoaded += this.HandleAdLoaded;
        this.bannerView.OnAdFailedToLoad += this.HandleAdFailedToLoad;
        this.bannerView.OnAdOpening += this.HandleAdOpened;
        this.bannerView.OnAdClosed += this.HandleAdClosed;
        this.bannerView.OnAdLeavingApplication += this.HandleAdLeftApplication;

        // Load a banner ad.
        this.bannerView.LoadAd(this.CreateAdRequest());
    }

    public void RequestInterstitial()
    {
        // These ad units are configured to always serve test ads.
        #if UNITY_EDITOR
                string adUnitId = "unused";
        #elif UNITY_ANDROID
                string adUnitId = interstitialId;
        #elif UNITY_IPHONE
                string adUnitId = interstitialId;
        #else
                string adUnitId = "unexpected_platform";
        #endif

        // Clean up interstitial ad before creating a new one.
        if (this.interstitial != null)
        {
            this.interstitial.Destroy();
        }

        // Create an interstitial.
        this.interstitial = new InterstitialAd(adUnitId);

        // Register for ad events.
        this.interstitial.OnAdLoaded += this.HandleInterstitialLoaded;
        this.interstitial.OnAdFailedToLoad += this.HandleInterstitialFailedToLoad;
        this.interstitial.OnAdOpening += this.HandleInterstitialOpened;
        this.interstitial.OnAdClosed += this.HandleInterstitialClosed;
        this.interstitial.OnAdLeavingApplication += this.HandleInterstitialLeftApplication;

        // Load an interstitial ad.
        this.interstitial.LoadAd(this.CreateAdRequest());
    }

    public void CreateAndLoadRewardedAd()
    {
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
        string adUnitId = "unexpected_platform";
#endif
        // Create new rewarded ad instance.
        this.rewardedAd = new RewardedAd(adUnitId);

        // Called when an ad request has successfully loaded.
        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        // Create an empty ad request.
        AdRequest request = this.CreateAdRequest();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);
    }

    public void ShowBanner() {
        bannerView.Show();
    }

    public void HideBanner() {
        bannerView.Hide();
    }

    public void ShowInterstitial()
    {
        if (this.interstitial.IsLoaded())
        {
            this.interstitial.Show();
            this.RequestInterstitial();
        }
        else
        {
            MonoBehaviour.print("Interstitial is not ready yet");
        }
    }

    private void ShowRewardedAd()
    {
        if (this.rewardedAd.IsLoaded())
        {
            this.rewardedAd.Show();
        }
        else
        {
            MonoBehaviour.print("Rewarded ad is not ready yet");
        }
    }

    #region Banner callback handlers

    public void HandleAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
    }

    public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: " + args.Message);
    }

    public void HandleAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
    }

    public void HandleAdLeftApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeftApplication event received");
    }

    #endregion

    #region Interstitial callback handlers

    public void HandleInterstitialLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleInterstitialLoaded event received");
    }

    public void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print(
            "HandleInterstitialFailedToLoad event received with message: " + args.Message);
    }

    public void HandleInterstitialOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleInterstitialOpened event received");
    }

    public void HandleInterstitialClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleInterstitialClosed event received");
    }

    public void HandleInterstitialLeftApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleInterstitialLeftApplication event received");
    }

    #endregion

    #region RewardedAd callback handlers

    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdLoaded event received");
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToLoad event received with message: " + args.Message);
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdOpening event received");
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToShow event received with message: " + args.Message);
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdClosed event received");
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        MonoBehaviour.print(
            "HandleRewardedAdRewarded event received for "
                        + amount.ToString() + " " + type);
    }

    #endregion
}
