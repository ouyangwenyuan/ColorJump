using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeColor : MonoBehaviour{
    public Image image;
    public Text text;
    public float speed = 1.7f;
    public Toast toast;

    private const int INCREASE = 1;
    private const int REDUCE = -1;
    private int status = 0;

    private void Awake()
    {
        toast.onBeginShowing += OnColorBeginIncreasing;
        toast.onBeginHiding += OnColorBeginReducing;
    }

    private void Update()
    {
        if (status == INCREASE)
        {
            Color color = GetColor();
            color.a += Time.deltaTime * speed;
            SetColor(color);
            if (color.a > 1)
            {
                status = 0;
            }
        }
        else if (status == REDUCE)
        {
            Color color = GetColor();
            color.a -= Time.deltaTime * speed;
            SetColor(color);
            if (color.a < 0)
            {
                status = 0;
            }
        }
    }

    private Color GetColor()
    {
        return image != null ? image.color : text.color;
    }

    private void SetColor(Color color)
    {
        if (image != null)
            image.color = color;
        else if (text != null)
            text.color = color;
    }

    private void OnColorBeginIncreasing()
    {
        Color color = GetColor();
        color.a = 0;
        SetColor(color);
        status = INCREASE;
    }

    private void OnColorBeginReducing()
    {
        Color color = GetColor();
        color.a = 1;
        SetColor(color);
        status = REDUCE;
    }
}
