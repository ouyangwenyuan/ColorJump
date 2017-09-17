#if UNITY_EDITOR || UNITY_ANDROID || UNITY_IPHONE
using UnityEngine;
using Newtonsoft.Json;
using System.Text;

public class GameProgress
{
    public static readonly string SAVE_KEY = "GameProgress";
    private static GameProgress instance;

    public int versionCode;
    public int coin;
    public int currentLevel;

    public static GameProgress Instance
    {
        set { instance = value; }
        get { return instance ?? (instance = new GameProgress()); }
    }

    [JsonConstructor]
    private GameProgress()
    {
    }

    public void Save()
    {
        LoadDataFromPrefs();
        versionCode++;
        string dataStr = JsonConvert.SerializeObject(this);
        Debug.Log("saving data: " + dataStr);
        byte[] data = Encoding.ASCII.GetBytes(dataStr);

        GooglePlayService.Instance.SetData(data);
        GooglePlayService.Instance.SaveGame();
    }

    public void OnSaveGameWrittenSuccess()
    {
        CPlayerPrefs.SetInt(PrefKeys.SAVE_VERSION_CODE, versionCode);
    }

    public void OnSaveGameLoadedSuccess(byte[] data)
    {
        //khi load xong data tu google roi thi set lai auto_sign_in_gps de no k auto signin nua
        CPlayerPrefs.SetInt("auto_sign_in_gps", CommonConst.MAX_AUTO_SIGNIN + 1);

        versionCode = CPlayerPrefs.GetInt(PrefKeys.SAVE_VERSION_CODE);
        string dataStr = Encoding.ASCII.GetString(data);
        Debug.Log("GPService: loaded data: " + dataStr);
        GameProgress gameProgress = JsonConvert.DeserializeObject<GameProgress>(dataStr);
        if (gameProgress != null && gameProgress.versionCode > versionCode)
        {
            Instance = gameProgress;
            SaveData2Prefs();
            Debug.Log("GPService: Applied data");
        }
    }

    private void LoadDataFromPrefs()
    {
        versionCode = CPlayerPrefs.GetInt(PrefKeys.SAVE_VERSION_CODE);
        coin = CurrencyController.GetBalance();
        currentLevel = LevelController.GetCurrentLevel();
    }

    private void SaveData2Prefs()
    {
        CPlayerPrefs.SetInt(PrefKeys.SAVE_VERSION_CODE, versionCode);
        CPlayerPrefs.SetInt(PrefKeys.CURRENCY, coin);
        CPlayerPrefs.SetInt(PrefKeys.CURRENT_LEVEL, currentLevel);
    }
}
#endif