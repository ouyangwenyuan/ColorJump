//#define DEVELOPMENT
public class CommonConst
{
    public const string ADMOB_ANDROID = "ca-app-pub-5207375798933617/3021816883";
    public const string ADMOB_IOS = "ca-app-pub-5207375798933617/3021816883";
    public const string ADMOB_BANNER = "ca-app-pub-5207375798933617/1545083688";
    public const string VIDEO_ADMOB_ANDROID = "";
    public const string VIDEO_ADMOB_IOS = "";


	public const string IOS_APP_ID = "";
	public const string MAC_APP_ID = "";
    public const string BB_APP_ID = "";
    public const string ANDROID_LINK = "https://play.google.com/store/apps/details?id=com.superpow.colorswitch";
    public const string GP_PAGE_LINK = "https://plus.google.com/106625749530285902481/posts";
    public const string GOOGLE_STORE = "https://play.google.com/store/apps/developer?id=Superpow";
    public const iTween.DimensionMode ITWEEN_MODE = iTween.DimensionMode.mode2D;

#if DEVELOPMENT
    public const int MIN_INVITE_FRIEND = 1;
    public const int MAX_INVITE_FRIEND = 20;
    public const bool ENCRYPTION_PREFS = false;
    public const int MIN_LEVEL_TO_RATE = 1;
    public const int ADS_PERIOD = 5;
#else
    public const int MIN_INVITE_FRIEND = 40;
    public const int MAX_INVITE_FRIEND = 50;
#if (UNITY_ANDROID || UNITY_IPHONE) && !UNITY_EDITOR
    public const bool ENCRYPTION_PREFS = true;
#else
    public const bool ENCRYPTION_PREFS = false;
#endif
    public const int MIN_LEVEL_TO_RATE = 3;
    public const int ADS_PERIOD = 3 * 60;
#endif

    public const int MAX_FRIEND_IN_MAP = 15;
    public const int FACE_AVATAR_SIZE = 100;

    public const int TARGET_WIDTH = 640;
    public const int TARGET_HEIGHT = 1150;

    public const float MIN_ASPECT = 4/3f;
    public const float MAX_ASPECT = 1.78f;

    public const int TOTAL_LEVELS = 50;
    public const int NOTIFICATION_DAILY_GIFT = 0;
    public static readonly string[] LEADERBOARD = { "CgkIpPimyeALEAIQBQ"};
    public const int MAX_AUTO_SIGNIN = 2;
}
