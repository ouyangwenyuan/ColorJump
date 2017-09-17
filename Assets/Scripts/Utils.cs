using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class Utils
{
    public static string BEST_SCORE = "best_score_pref";

    public static int GetBestScore()
    {
        return CPlayerPrefs.GetInt(BEST_SCORE, 0);
    }

    public static void SetBestScore(int score)
    {
        CPlayerPrefs.SetInt(BEST_SCORE, score);
    }
}