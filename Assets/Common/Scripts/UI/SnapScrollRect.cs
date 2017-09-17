using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class SnapScrollRect : MonoBehaviour, IDragHandler, IEndDragHandler
{
    float[] points;
    [Tooltip("how many screens or pages are there within the content (steps)")]
    public int screens = 1;
    public int speed = 10;
    float stepSize;

    ScrollRect scroll;
    bool LerpH;
    float targetH;
    [HideInInspector]
    public int indexH = 0;
    [Tooltip("Snap horizontally")]
    public bool snapInHorizontal = true;

    bool LerpV;
    float targetV;
    [HideInInspector]
    public int indexV = 0;
    [Tooltip("Snap vertically")]
    public bool snapInVertical = true;

    public GameObject[] indicators;
    public Text tabName;
    public string tabNamePrefix;

    void Awake()
    {
        scroll = gameObject.GetComponent<ScrollRect>();
        InitPoints();
    }

    public void InitPoints()
    {
        points = new float[screens];
        if (screens > 1)
        {
            stepSize = 1 / (float)(screens - 1);

            for (int i = 0; i < screens; i++)
            {
                points[i] = i * stepSize;
            }
        }
        else
        {
            points[0] = 0;
        }
    }

    void Update()
    {
        if (LerpH)
        {
            scroll.horizontalNormalizedPosition = Mathf.Lerp(scroll.horizontalNormalizedPosition, targetH, speed * scroll.elasticity * Time.deltaTime);
            if (Mathf.Approximately(scroll.horizontalNormalizedPosition, targetH)) LerpH = false;
        }
        if (LerpV)
        {
            scroll.verticalNormalizedPosition = Mathf.Lerp(scroll.verticalNormalizedPosition, targetV, speed * scroll.elasticity * Time.deltaTime);
            if (Mathf.Approximately(scroll.verticalNormalizedPosition, targetV)) LerpV = false;
        }
    }

    public void OnEndDrag(PointerEventData data)
    {
        if (scroll.horizontal && snapInHorizontal)
        {
            int delta = 0;
            delta = (int)Mathf.Sqrt(Mathf.Abs(scroll.velocity.x)) / 10;
            if (scroll.velocity.x > 0)
            {
                delta = -delta;
            }
            indexH = Math.Min(screens - 1, Math.Max(0, FindNearest(scroll.horizontalNormalizedPosition, points) + delta));
            targetH = points[indexH];
            UpdateIndicator(true);
            LerpH = true;
        }
        if (scroll.vertical && snapInVertical)
        {
            int delta = 0;
            delta = (int)Mathf.Sqrt(Mathf.Abs(scroll.velocity.y)) / 10;
            if (scroll.velocity.y > 0)
            {
                delta = -delta;
            }
            indexV = Math.Min(screens - 1, Math.Max(0, FindNearest(scroll.verticalNormalizedPosition, points) + delta));
            targetV = points[indexV];
            UpdateIndicator(false);
            LerpV = true;
        }
    }

    public void NextPage(bool isHorizontal)
    {
        Sound.instance.PlayButton();
        if (isHorizontal)
        {
            indexH = Math.Min(screens - 1, indexH + 1);
            targetH = points[indexH];
            LerpH = true;
        }
        else
        {
            indexV = Math.Min(screens - 1, indexV + 1);
            targetV = points[indexV];
            LerpV = true;
        }
        UpdateIndicator(isHorizontal);
    }

    public void PreviousPage(bool isHorizontal)
    {
        Sound.instance.PlayButton();
        if (isHorizontal)
        {
            indexH = Math.Max(0, indexH - 1);
            targetH = points[indexH];
            LerpH = true;
        }
        else
        {
            indexV = Math.Max(0, indexV - 1);
            targetV = points[indexV];
            LerpV = true;
        }
        UpdateIndicator(isHorizontal);
    }

    public void SetPage(int pageIndex, bool isHorizontal)
    {
        if (isHorizontal)
        {
            indexH = Math.Min(screens - 1, pageIndex);
            targetH = points[indexH];
            scroll.horizontalNormalizedPosition = targetH;
        }
        else
        {
            indexV = Math.Min(screens - 1, pageIndex);
            targetV = points[indexV];
            scroll.verticalNormalizedPosition = targetV;
        }
        UpdateIndicator(isHorizontal);
    }

    public void UpdateIndicator(bool isHorizontal)
    {
        int index = isHorizontal ? indexH : indexV;
        for (int i = 0; i < indicators.Length; i++)
        {
            indicators[i].SetActive(i == index);
        }
        if (tabName != null)
        {
            tabName.text = tabNamePrefix + (index + 1);
        }
    }

    public void OnDrag(PointerEventData data)
    {
        LerpH = false;
        LerpV = false;
    }

    int FindNearest(float f, float[] array)
    {
        float distance = Mathf.Infinity;
        int output = 0;
        for (int index = 0; index < array.Length; index++)
        {
            if (Mathf.Abs(array[index] - f) < distance)
            {
                distance = Mathf.Abs(array[index] - f);
                output = index;
            }
        }
        return output;
    }
}