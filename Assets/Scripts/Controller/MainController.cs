using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainController : BaseController {

    public static MainController instance;
    public Text scoreText;
    public bool pausing = false;
    private int score;
    public int Score
    {
        get { return score; }
        set
        {
            score = value;
            scoreText.text = score.ToString();
        }
    }

    protected override void Start()
    {
 	    base.Start();
        instance = this;
        Music.instance.Play(Music.Type.MainMusic);
    }

    public void ResetGame()
    {
        CUtils.LoadLevel(1);
    }

    public void ShowEndDialog()
    {
        EndGameDialog endDialog = (EndGameDialog)DialogController.instance.GetDialog(DialogType.EndGame);
        endDialog.SetInfo(Score);
        DialogController.instance.ShowDialog(endDialog);
        Timer.Schedule(this, 1f, () =>
        {
            BarrierRegion.instance.DestroyAll();
        });
    }

    public void OnBackClick()
    {
        Sound.instance.PlayButton();
        DialogController.instance.ShowDialog(DialogType.Pause);
        pausing = true;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !DialogController.instance.IsDialogShowing())
        {
            OnBackClick();
        }
    }
}
