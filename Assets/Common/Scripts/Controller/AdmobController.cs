using UnityEngine;
using System;
using GoogleMobileAds;
using GoogleMobileAds.Api;

public class AdmobController : MonoBehaviour
{
    private BannerView bannerView;
    public InterstitialAd normalInterstitial;
    public InterstitialAd videoInterstitial;
    public static AdmobController instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
        RequestBanner();
        RequestInterstitial();
        DontDestroyOnLoad(gameObject);
    }

    public void RequestBanner()
    {
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = CUtils.GetAdmobBanner();
#elif UNITY_IPHONE
        string adUnitId = CUtils.GetAdmobBanner();
#else
        string adUnitId = "unexpected_platform";
#endif

        // Create a 320x50 banner at the top of the screen.
        bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Bottom);
        // Register for ad events.
        bannerView.AdLoaded += HandleAdLoaded;
        bannerView.AdFailedToLoad += HandleAdFailedToLoad;
        bannerView.AdOpened += HandleAdOpened;
        bannerView.AdClosing += HandleAdClosing;
        bannerView.AdClosed += HandleAdClosed;
        bannerView.AdLeftApplication += HandleAdLeftApplication;
        // Load a banner ad.
        bannerView.LoadAd(createAdRequest());
    }

    public void RequestInterstitial(bool video = false, bool showAfterLoaded = false)
    {
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = video ? CUtils.GetVideoAdmobAndroid() : CUtils.GetAdmobAndroid();
#elif UNITY_IPHONE
        string adUnitId = video ? CUtils.GetVideoAdmobIOS() : CUtils.GetAdmobIOS();
#else
        string adUnitId = "unexpected_platform";
#endif

        // Create an interstitial.
        InterstitialAd interstitial = new InterstitialAd(adUnitId);
        // Register for ad events.
        if (showAfterLoaded)
            interstitial.AdLoaded += HandleInterstitialLoadedAndShow;
        else
            interstitial.AdLoaded += HandleInterstitialLoaded;
        interstitial.AdFailedToLoad += HandleInterstitialFailedToLoad;
        interstitial.AdOpened += HandleInterstitialOpened;
        interstitial.AdClosing += HandleInterstitialClosing;
        interstitial.AdClosed += HandleInterstitialClosed;
        interstitial.AdLeftApplication += HandleInterstitialLeftApplication;
        interstitial.LoadAd(createAdRequest());

        if (video)
            videoInterstitial = interstitial;
        else
            normalInterstitial = interstitial;
    }

    // Returns an ad request with custom ad targeting.
    private AdRequest createAdRequest()
    {
        return new AdRequest.Builder()
                .AddTestDevice(AdRequest.TestDeviceSimulator)
                .AddTestDevice("0123456789ABCDEF0123456789ABCDEF")
                .AddKeyword("game")
                .TagForChildDirectedTreatment(false)
                .AddExtra("color_bg", "9B30FF")
                .Build();
    }

    public void ShowInterstitial(InterstitialAd ad)
    {
        if (ad != null && ad.IsLoaded())
        {
            ad.Show();
        }
    }

    public void ShowBanner()
    {
        if (CUtils.IsBuyItem()) return;
        if (bannerView != null)
        {
            bannerView.Show();
        }
        else
        {
            RequestBanner();
        }
    }

    public void HideBanner()
    {
        if (bannerView != null)
        {
            bannerView.Hide();
        }
    }

    public bool ShowInterstitial(bool video = false)
    {
        InterstitialAd ad = video ? videoInterstitial : normalInterstitial;
        if (ad != null && ad.IsLoaded())
        {
            ad.Show();
            return true;
        }
        return false;
    }

    #region Banner callback handlers

    public void HandleAdLoaded(object sender, EventArgs args)
    {
        HideBanner();
        print("HandleAdLoaded event received.");
    }

    public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        print("HandleFailedToReceiveAd event received with message: " + args.Message);
    }

    public void HandleAdOpened(object sender, EventArgs args)
    {
        print("HandleAdOpened event received");
    }

    void HandleAdClosing(object sender, EventArgs args)
    {
        print("HandleAdClosing event received");
    }

    public void HandleAdClosed(object sender, EventArgs args)
    {
        print("HandleAdClosed event received");
    }

    public void HandleAdLeftApplication(object sender, EventArgs args)
    {
        print("HandleAdLeftApplication event received");
    }

    #endregion

    #region Interstitial callback handlers

    public void HandleInterstitialLoaded(object sender, EventArgs args)
    {
        print("HandleInterstitialLoaded event received.");
    }

    public void HandleInterstitialLoadedAndShow(object sender, EventArgs args)
    {
        ShowInterstitial((InterstitialAd)sender);
        print("HandleInterstitialLoaded event received.");
    }

    public void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        print("HandleInterstitialFailedToLoad event received with message: " + args.Message);
    }

    public void HandleInterstitialOpened(object sender, EventArgs args)
    {
        print("HandleInterstitialOpened event received");
    }

    void HandleInterstitialClosing(object sender, EventArgs args)
    {
        print("HandleInterstitialClosing event received");
    }

    public void HandleInterstitialClosed(object sender, EventArgs args)
    {
        print("HandleInterstitialClosed event received");
        RequestInterstitial(false);
    }

    public void HandleInterstitialLeftApplication(object sender, EventArgs args)
    {
        print("HandleInterstitialLeftApplication event received");
    }

    #endregion
}
