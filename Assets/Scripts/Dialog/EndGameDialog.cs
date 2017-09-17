using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndGameDialog : Dialog {
    public Text score, best;

    public void SetInfo(int scoreValue)
    {
        score.text = scoreValue.ToString();
        int bestScoreValue = Utils.GetBestScore();
        if (bestScoreValue == 0 || bestScoreValue < scoreValue)
        {
            bestScoreValue = scoreValue;
            Utils.SetBestScore(scoreValue);
        }
        best.text = bestScoreValue.ToString();
#if (UNITY_ANDROID || UNITY_IPHONE) && !UNITY_EDITOR
        GooglePlayService.Instance.PostScoreLeaderboard(bestScoreValue);
#endif
    }

    const int REPLAY = 0, MORE_GAME = 1, RATE = 2, LEADERBOARD = 3, HOME = 4;
    public void OnClick(int index)
    {
        switch (index)
        {
            case REPLAY:
                MainController.instance.ResetGame();
                Close();
                break;
            case MORE_GAME:
#if UNITY_ANDROID && !UNITY_EDITOR
                Application.OpenURL(CommonConst.GOOGLE_STORE);
#endif
                break;
            case RATE:
                CUtils.RateGame();
                break;
            case LEADERBOARD:
#if UNITY_EDITOR || UNITY_ANDROID || UNITY_IPHONE
                GooglePlayService.Instance.ShowLeaderboard();
#endif
                break;
            case HOME:
                Close();
                CUtils.LoadLevel(0);
                break;
        }
        Sound.instance.PlayButton();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Close();
            CUtils.LoadLevel(0);
        }
    }
}
