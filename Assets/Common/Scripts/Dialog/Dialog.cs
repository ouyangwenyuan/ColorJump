using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class Dialog : MonoBehaviour
{
    public Animator anim;
    public AnimationClip hidingAnimation;
    public GameObject title, message;
    public Action<Dialog> onDialogOpened;
    public Action<Dialog> onDialogClosed;
    public Action onDialogCompleteClosed;
    public Action<Dialog> onButtonCloseClicked;
    public DialogType dialogType;

    private AnimatorStateInfo info;
    private bool isShowing;

    protected virtual void Awake()
    {
        if (anim == null) anim = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        onDialogCompleteClosed += OnDialogCompleteClosed;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Close();
        }
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
        if (anim != null && IsIdle())
        {
            isShowing = true;
            anim.SetTrigger("show");
            onDialogOpened(this);
        }
        // Show ads
        CUtils.CheckAndShowAds();
        AdmobController.instance.ShowBanner();
    }

    public virtual void Close()
    {
        if (isShowing == false) return;
        if (anim != null)
        {
            if (!IsIdle())
            {
                anim.Play("Idle");
            }
            isShowing = false;
            if (hidingAnimation != null)
            {
                anim.SetTrigger("hide");
                Timer.Schedule(this, hidingAnimation.length, DoClose);
            }
            else
            {
                DoClose();
            }
        }
        else
        {
            DoClose();
        }
        // Hide ads
        AdmobController.instance.HideBanner();
    }

    private void DoClose()
    {
        onDialogClosed(this);
        if (onDialogCompleteClosed != null) onDialogCompleteClosed();
        Destroy(gameObject);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        isShowing = false;
    }

    public bool IsIdle()
    {
        info = anim.GetCurrentAnimatorStateInfo(0);
        return info.IsName("Idle");
    }

    public bool IsShowing()
    {
        return isShowing;
    }

    public virtual void OnDialogCompleteClosed()
    {
        onDialogCompleteClosed -= OnDialogCompleteClosed;
    }
}
