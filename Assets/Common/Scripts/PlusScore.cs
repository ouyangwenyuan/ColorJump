using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlusScore : MonoBehaviour {

    public Text Text;

    public void SetText(string text)
    {
        Text.text = text;
    }
}
