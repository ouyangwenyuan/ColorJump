using UnityEngine;
using UnityEngine.UI;

public class Cover : MonoBehaviour
{
    public RectTransform left, right, above, below;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        UpdateCover();
        MainCamera.instance.onScreenSizeChanged += UpdateCover;
    }

    private void OnLevelWasLoaded(int level)
    {
        MainCamera.instance.onScreenSizeChanged += UpdateCover;
    }

    private void UpdateCover()
    {
        float width = MainCamera.instance.GetWidth();
        float height = MainCamera.instance.GetHeight();
        float screenWidth = MainCamera.instance.virtualWidth;
        float screenHeight = MainCamera.instance.virtualHeight;

        float paddingLeftRight = (screenWidth - width * 200) / 2;
        float paddingAboveBelow = (screenHeight - height * 200) / 2;

        left.gameObject.SetActive(paddingLeftRight > 0.0001f);
        right.gameObject.SetActive(paddingLeftRight > 0.0001f);

        above.gameObject.SetActive(paddingAboveBelow > 0.0001f);
        below.gameObject.SetActive(paddingAboveBelow > 0.0001f);

        float leftWidth = MainCamera.instance.landscape ? 400 : 800;
        float aboveHeight = MainCamera.instance.landscape ? 800 : 400;

        if (left.sizeDelta.x < paddingLeftRight)
        {
            left.sizeDelta = new Vector2(paddingLeftRight, left.sizeDelta.y);
            right.sizeDelta = new Vector2(paddingLeftRight, right.sizeDelta.y);
        }
        else
        {
            left.sizeDelta = new Vector2(leftWidth, left.sizeDelta.y);
            right.sizeDelta = new Vector2(leftWidth, right.sizeDelta.y);
        }

        if (above.sizeDelta.y < paddingAboveBelow)
        {
            above.sizeDelta = new Vector2(above.sizeDelta.x, paddingAboveBelow);
            below.sizeDelta = new Vector2(below.sizeDelta.x, paddingAboveBelow);
        }
        else
        {
            above.sizeDelta = new Vector2(above.sizeDelta.x, aboveHeight);
            below.sizeDelta = new Vector2(below.sizeDelta.x, aboveHeight);
        }
    }
}