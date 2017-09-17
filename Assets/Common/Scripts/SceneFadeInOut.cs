using UnityEngine;
using System.Collections;

public class SceneFadeInOut : MonoBehaviour
{
    public float fadeSpeed = 1f;          // Speed that the screen fades to and from black.


    private bool sceneStarting = true;      // Whether or not the scene is still fading in.
    private bool sceneEnding = false;
    private int sceneID;


    void Awake()
    {
        // Set the texture so that it is the the size of the screen and covers it.
        GetComponent<GUITexture>().enabled = true;
        GetComponent<GUITexture>().pixelInset = new Rect(0f, 0f, Screen.width, Screen.height);
    }


    void Update()
    {
        // If the scene is starting...
        if (sceneStarting)
            // ... call the StartScene function.
            StartScene();

        if (sceneEnding)
            EndScene();
    }


    void FadeToClear()
    {
        Color color = GetComponent<GUITexture>().color;
        color.a -= Time.deltaTime * fadeSpeed;
        GetComponent<GUITexture>().color = color;
    }


    void FadeToBlack()
    {
        Color color = GetComponent<GUITexture>().color;
        color.a += Time.deltaTime * fadeSpeed;
        GetComponent<GUITexture>().color = color;
    }


    void StartScene()
    {
        // Fade the texture to clear.
        FadeToClear();

        // If the texture is almost clear...
        if (GetComponent<GUITexture>().color.a <= 0)
        {
            // ... set the colour to clear and disable the GUITexture.
            GetComponent<GUITexture>().color = Color.clear;
            GetComponent<GUITexture>().enabled = false;

            // The scene is no longer starting.
            sceneStarting = false;
        }
    }

    public void StartEndScene(int id)
    {
        sceneEnding = true;
        sceneID = id;
        // Make sure the texture is enabled.
        GetComponent<GUITexture>().enabled = true;
    }

    private void EndScene()
    {
        // Start fading towards black.
        FadeToBlack();

        // If the screen is almost black...
        if (GetComponent<GUITexture>().color.a >= 0.7f)
            // ... reload the level.
            Application.LoadLevel(sceneID);
    }
}