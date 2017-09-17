using UnityEngine;
using System.Collections;
using System;

public class ColorChanger : MonoBehaviour {
    public enum ChangerType {All, BlueRed, GreenYellow, ExceptYellow};
    public ChangerType type;

    public void Rotate(bool clockwise)
    {
        GetComponent<Animator>().SetTrigger(clockwise ? "clockwise" : "anticlockwise");
    }

    public void DestroyChanger()
    {
        Destroy(gameObject);
    }
}
