using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DialogOverlay : MonoBehaviour {
	public RectTransform overlayTransform;
    private Image overlay;

	private void Start () {
		overlay = GetComponent<Image>();
		DialogController.instance.onDialogsOpened += OnDialogOpened;
		DialogController.instance.onDialogsClosed += OnDialogClosed;
		
		UpdateOverlay();
		MainCamera.instance.onScreenSizeChanged += UpdateOverlay;
	}

	private void OnLevelWasLoaded (int level)
	{
		MainCamera.instance.onScreenSizeChanged += UpdateOverlay;
	}
	
	private void UpdateOverlay()
	{
		overlayTransform.sizeDelta = new Vector2(MainCamera.instance.virtualWidth, MainCamera.instance.virtualHeight);
	}

    private void OnDialogOpened()
    {
        overlay.enabled = true;
    }

    private void OnDialogClosed()
    {
        overlay.enabled = false;
    }

    private void OnDestroy()
    {
        DialogController.instance.onDialogsOpened -= OnDialogOpened;
        DialogController.instance.onDialogsClosed -= OnDialogClosed;
    }
}
