#if UNITY_EDITOR || UNITY_ANDROID || UNITY_IPHONE
using UnityEngine;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine.SocialPlatforms;
using System;

public class GooglePlayService
{
    public Action<byte[]> onSaveGameLoaded;

    private static GooglePlayService instance;
    private bool isSaving;
    private byte[] mData;

    private GooglePlayService()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            // enables saving game progress.
            //.EnableSavedGames()
            // registers a callback to handle game invitations received while the game is not running.
            //.WithInvitationDelegate(<callback method>)
            // registers a callback for turn based match notifications received while the
            // game is not running.
            //.WithMatchDelegate(<callback method>)
        .Build();

        PlayGamesPlatform.InitializeInstance(config);
        // recommended for debugging:
        PlayGamesPlatform.DebugLogEnabled = true;
        // Activate the Google Play Games platform
        PlayGamesPlatform.Activate();
    }

    public static GooglePlayService Instance
    {
        get { return instance ?? (instance = new GooglePlayService()); }
    }

    public bool Authenticated
    {
        get { return PlayGamesPlatform.Instance.localUser.authenticated; }
    }

    public void SignIn(Action onSigninSuccess, Action onSigninFail)
    {
        if (!Authenticated)
        {
            PlayGamesPlatform.Instance.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    Debug.Log("Signin google play successed");
                    if (onSigninSuccess != null)
                        onSigninSuccess();
                }
                else
                {
                    Debug.Log("Signin google play failed");
                    if (onSigninFail != null)
                        onSigninFail();
                }
            });
        }
        else
        {
            if (onSigninSuccess != null)
                onSigninSuccess();
        }
    }

    public void SignIn()
    {
        SignIn(null, null);
    }

    public void SetData(byte[] data)
    {
        mData = data;
    }

    public void SaveGame()
    {
        if (Authenticated)
        {
            isSaving = true;
            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
            savedGameClient.OpenWithAutomaticConflictResolution(GameProgress.SAVE_KEY, DataSource.ReadCacheOrNetwork,
                ConflictResolutionStrategy.UseLongestPlaytime, OnSavedGameOpened);
        }
    }

    public void OnSavedGameOpened(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            if (isSaving)
            {
                SavedGameMetadataUpdate.Builder builder = new
                        SavedGameMetadataUpdate.Builder()
                            .WithUpdatedDescription("Saved Game at " + DateTime.Now);
                SavedGameMetadataUpdate updatedMetadata = builder.Build();
                ((PlayGamesPlatform)Social.Active).SavedGame.CommitUpdate(game, updatedMetadata, mData, SavedGameWritten);
            }
            else
            {
                ((PlayGamesPlatform)Social.Active).SavedGame.ReadBinaryData(game, SavedGameLoaded);
            }
        }
        else
        {
            Debug.LogWarning("Error opening game: " + status);
        }
    }

    public void SavedGameWritten(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            GameProgress.Instance.OnSaveGameWrittenSuccess();
            Debug.Log("Game " + game.Description + " written");
        }
        else
        {
            Debug.LogWarning("Error saving game: " + status);
        }
    }

    public void SavedGameLoaded(SavedGameRequestStatus status, byte[] data)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            Debug.Log("SaveGameLoaded success");
            GameProgress.Instance.OnSaveGameLoadedSuccess(data);
        }
        else
        {
            Debug.LogWarning("Error reading game: " + status);
        }
        DialogController.instance.CloseCurrentDialog();
    }

    public void LoadGame()
    {
        if (Authenticated)
        {

            isSaving = false;
            ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution(GameProgress.SAVE_KEY,
                                                                                             DataSource.ReadCacheOrNetwork,
                                                                                             ConflictResolutionStrategy.UseLongestPlaytime,
                                                                                             OnSavedGameOpened);
        }
    }

    public void PostScoreLeaderboard(int score, int leaderboardIndex = 0)
    {
        if (!Authenticated) return;
        Social.ReportScore(score, CommonConst.LEADERBOARD[leaderboardIndex], (bool success) =>
        {

        });
    }

    public void ShowLeaderboard(int leaderboardIndex = 0)
    {
        if (Authenticated)
        {
            PlayGamesPlatform.Instance.ShowLeaderboardUI(CommonConst.LEADERBOARD[leaderboardIndex]);
        }
        else
        {
            SignIn();
        }
    }

    public void CheckSavedGameExist()
    {
        if (Authenticated)
        {
            ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution(GameProgress.SAVE_KEY,
                                                                                             DataSource.ReadNetworkOnly,
                                                                                             ConflictResolutionStrategy.UseLongestPlaytime,
                                                                                             OnCheckSavedGameExist);
        }
    }

    public void OnCheckSavedGameExist(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        DialogController.instance.CloseCurrentDialog();
        if (status == SavedGameRequestStatus.Success)
        {
            Action onOkAction = (Action)(() =>
            {
                GooglePlayService.instance.LoadGame();
            });

            DialogController.instance.ShowYesNoDialog("Load Game", "Do you want to load saved game from cloud. Warning: progress in the current game will be lost", onOkAction, null);
        }
    }
}
#endif