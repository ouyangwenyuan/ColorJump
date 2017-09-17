using UnityEngine;
using System.Collections;

public class FirstSceneController : MonoBehaviour
{
	public static FirstSceneController instance;

	private void Awake()
	{
		instance = this;
		Application.targetFrameRate = 60;
        CPlayerPrefs.useRijndael(CommonConst.ENCRYPTION_PREFS);

		if (!CUtils.IsGameInitialzied())
		{
			CUtils.SetInitGame();
			CUtils.SetAdmobAndroid(CommonConst.ADMOB_ANDROID);
            CUtils.SetVideoAdmobAndroid(CommonConst.VIDEO_ADMOB_ANDROID);
			CUtils.SetAdmobIOS(CommonConst.ADMOB_IOS);
            CUtils.SetVideoAdmobIOS(CommonConst.VIDEO_ADMOB_IOS);
            CUtils.SetAdmobBanner(CommonConst.ADMOB_BANNER);
		}

		CUtils.SetAndroidVersion(GameVersion.ANDROID);
		CUtils.SetIOSVersion(GameVersion.IOS);
		CUtils.SetWindowsPhoneVersion(GameVersion.WP);
	}

	private void Update()
    {
#if !UNITY_WSA
        if (Input.GetKeyDown(KeyCode.Escape) && !DialogController.instance.IsDialogShowing())
		{
			QuitGame.instance.ShowConfirmDialog();
        }
#endif
    }
}
