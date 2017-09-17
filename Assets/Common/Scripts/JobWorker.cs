using UnityEngine;
using System.Collections.Generic;
using System;

public class JobWorker : MonoBehaviour
{
    public Action<string> onEnterScene;
    public Action onLink2Store;
    public Action onDailyGiftReceived;
    public Action onShowBanner;
    public Action onCloseBanner;
    public Action onRequestInterstitial;

    public static JobWorker instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        CUtils.SetPreviousScene(-1);
    }

    private void Start()
    {
        CurrencyController.onBallanceIncreased += OnBallanceIncreased;

#if (UNITY_ANDROID) && !UNITY_EDITOR
		int autoSignin = CUtils.GetAutoSigninGPS();
		if (autoSignin < CommonConst.MAX_AUTO_SIGNIN && !GooglePlayService.Instance.Authenticated)
		{
			GooglePlayService.Instance.SignIn();
			CUtils.SetAutoSigninGPS(autoSignin + 1);
		}
#endif
    }

    private void OnBallanceIncreased(int value)
    {
        int currentScore = CUtils.GetLeaderboardScore();
        CUtils.SetLeaderboardScore(currentScore + value);
    }
}