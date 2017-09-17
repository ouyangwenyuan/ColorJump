using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PromoteQuitDialog : YesNoDialog{
    public Image feature;
    public Text promoteMessage;
    public Transform installBtn;

    [HideInInspector]
    public Promote promote;

    protected override void Start()
    {
        base.Start();
        onYesClick = Quit;

        promote = PromoteController.instance.GetPromote(PromoteType.QuitGame);
        StartCoroutine(CUtils.LoadPicture(promote.featureUrl, LoadPictureComplete, promote.featureWidth, promote.featureHeight));

        if (!string.IsNullOrEmpty(promote.message))
        {
            promoteMessage.text = promote.message;
            RectTransform parent = promoteMessage.transform.parent.GetComponent<RectTransform>();
            parent.sizeDelta = new Vector2(promoteMessage.preferredWidth + 30, parent.sizeDelta.y);
            if (promote.rewardType == RewardType.RemoveAds && (CUtils.IsBuyItem() || CUtils.IsAdsRemoved()))
            {
                HideText();
            }
        }
        else
        {
            HideText();
        }
    }

    private void HideText()
    {
        promoteMessage.transform.parent.gameObject.SetActive(false);
        installBtn.position = promoteMessage.transform.position + Vector3.up * 0.1f;
    }

    private void Quit()
    {
        Application.Quit();
    }

    public void OnFeatureClick()
    {
        CUtils.OpenStore(promote.package);
        GoogleAnalyticsV3.instance.LogEvent("Promotion", "Click install app", promote.package, 0);
    }

    private void LoadPictureComplete(Texture2D texture)
    {
        feature.sprite = CUtils.CreateSprite(texture, promote.featureWidth, promote.featureHeight);
    }
}
