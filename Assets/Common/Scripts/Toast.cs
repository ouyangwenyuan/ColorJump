using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class Toast : MonoBehaviour{
    public Canvas canvas;
    public RectTransform backgroundTransform;
    public RectTransform messageTransform;
    public Text message;
    public string msg;
    public static Toast instance;
    public bool isShowing = false;

    public Action onBeginShowing;
    public Action onBeginHiding;

    private Queue<AToast> queue = new Queue<AToast>();

    private class AToast
    {
        public string msg;
        public float time;
        public AToast(string msg, float time)
        {
            this.msg = msg;
            this.time = time;
        }
    }

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void SetMessage(string msg)
    {
        this.msg = msg;
        message.text = msg;
        backgroundTransform.sizeDelta = new Vector2(message.preferredWidth + 30, backgroundTransform.sizeDelta.y);
    }

    private void Show(AToast aToast)
    {
        SetMessage(aToast.msg);
        canvas.enabled = true;
        Invoke("Hide", aToast.time);
        isShowing = true;
        onBeginShowing();
    }

    public void ShowMessage(string msg, float time = 1.5f)
    {
        AToast aToast = new AToast(msg, time);
        queue.Enqueue(aToast);

        ShowOldestToast();
    }

    private void Hide()
    {
        onBeginHiding();
        Invoke("CompleteHiding", 1);
    }

    private void CompleteHiding()
    {
        canvas.enabled = false;
        isShowing = false;
        ShowOldestToast();
    }

    private void ShowOldestToast()
    {
        if (queue.Count == 0) return;
        if (isShowing) return;

        AToast current = queue.Dequeue();
        Show(current);
    }
}
