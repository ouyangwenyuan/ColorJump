using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PauseDialog : Dialog
{
    protected override void Start()
    {
        base.Start();
        Time.timeScale = 0;
    }

    public void HomeClick()
    {
        CUtils.LoadLevel(0);
        Close();
    }

    public void ContinueClick()
    {
        Close();
    }

    public override void Close()
    {
        Time.timeScale = 1;
        MainController.instance.pausing = false;
        base.Close();
    }

}
