using UnityEngine;
using System.Collections;

public class QuitGame : MonoBehaviour {

    public static QuitGame instance;
    private void Awake()
    {
        instance = this;
    }

    public void ShowConfirmDialog()
    {
        var promote = PromoteController.instance.GetPromote(PromoteType.QuitGame);
        if (promote != null)
            DialogController.instance.ShowDialog(DialogType.PromoteQuit);
        else
            DialogController.instance.ShowDialog(DialogType.QuitGame);
    }
}
