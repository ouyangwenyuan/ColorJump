using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

public enum PromoteType { QuitGame, Interstitial };
public enum RewardType { None, RemoveAds, Currency };

public class PromoteController : MonoBehaviour
{
    [HideInInspector]
    public List<Promote> promotes;

    public static PromoteController instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        promotes = GetPromotes();
        UpdatePromotion();
    }

    public Promote GetPromote(PromoteType promoteType)
    {
        if (promotes == null) return null;
        return promotes.Find(x => x.type == promoteType && !CUtils.IsAppInstalled(x.package) && CUtils.IsCacheExists(x.featureUrl));
    }

    private List<string> GetPackages()
    {
        return promotes.Select(x => x.package).ToList();
    }

    public void OnApplicationPause(bool pause)
    {
        if (pause == false)
        {
            UpdatePromotion();
        }
    }

    private void UpdatePromotion()
    {
        if (promotes == null) return;

        var apps = promotes.FindAll(x => CUtils.IsAppInstalled(x.package) && x.rewardType == RewardType.RemoveAds);
        if (apps.Count == 0) CUtils.SetRemoveAds(false);

        apps = promotes.FindAll(x => !CUtils.IsAppInstalled(x.package) && x.rewardType == RewardType.RemoveAds && IsRewarded(x.package));
        foreach (var promote in apps)
        {
            CPlayerPrefs.SetBool(promote.package + "_rewarded", false);
        }

        var packages = GetInstalledApp();
        Reward(packages);
    }

    private List<string> GetInstalledApp()
    {
        return GetPackages().FindAll(x => CUtils.IsAppInstalled(x) && !IsRewarded(x));
    }

    private void Reward(List<string> packages)
    {
        foreach (string package in packages)
        {
            Reward(package);
        }
    }

    private bool IsRewarded(string package)
    {
        return CPlayerPrefs.GetBool(package + "_rewarded");
    }

    private void Reward(string package)
    {
        GoogleAnalyticsV3.instance.LogEvent("Promotion", "Install app", package, 0);

        CPlayerPrefs.SetBool(package + "_rewarded", true);
        Promote promote = promotes.Find(x => x.package == package);
        if (promote == null) return;

        switch (promote.rewardType)
        {
            case RewardType.RemoveAds:
                CUtils.SetRemoveAds(true);
                Toast.instance.ShowMessage("You will no longer receive ads");
                break;
            case RewardType.Currency:
                CurrencyController.CreditBalance(promote.rewardValue);
                break;
        }
    }

    private void CacheFeature()
    {
        foreach (Promote promote in promotes)
        {
            StartCoroutine(CUtils.CachePicture(promote.featureUrl, null));
        }
    }

    public void ApplyPromotion(string data)
    {
        CPlayerPrefs.SetString("promotes", data);
        promotes = GetPromotes(data);
        CacheFeature();
    }

    private List<Promote> GetPromotes(string data)
    {
        return JsonConvert.DeserializeObject<Promotes>(data).promotes;
    }

    private List<Promote> GetPromotes()
    {
        if (promotes != null) return promotes;

        if (!CPlayerPrefs.HasKey("promotes"))
        {
            return null;
        }
        return GetPromotes(CPlayerPrefs.GetString("promotes"));
    }
}