using UnityEngine;
using System.Collections;
using System;

public class HomeController : BaseController {
    private const int PLAY = 0, RATE = 1, LEADERBOARD = 2;
    public ToggleButton tgSound;

    protected override void Start()
    {
        base.Start();
        tgSound.IsOn = Sound.instance.IsEnabled();
        Music.instance.Play(Music.Type.MainMusic);
//        CUtils.SetActionTime("show_ads");
    }

    public void OnToggleSound()
    {
        tgSound.Toggle();
        Sound.instance.SetEnabled(tgSound.IsOn);
        Music.instance.SetEnabled(tgSound.IsOn, true);
        Sound.instance.PlayButton();
    }

    public void OnClick(int index)
    {
        switch (index)
        {
            case PLAY:
                CUtils.LoadLevel(1);
                break;
            case RATE:
                CUtils.RateGame();
                break;
            case LEADERBOARD:
#if UNITY_EDITOR || UNITY_ANDROID || UNITY_IPHONE
                GooglePlayService.Instance.ShowLeaderboard();
#endif
                break;
        }
        Sound.instance.PlayButton();
    }
}
