using UnityEngine;
using System.Collections;
using System;

public class Barrier : MonoBehaviour {
    public Action<Barrier> onMoveOut;
    public Animator anim;
    public bool startRotate;
    private void Start()
    {
        if (startRotate) Rotate(true);
    }

    public void Rotate(bool clockwise)
    {
        anim.SetTrigger(clockwise ? "clockwise" : "anticlockwise");
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "DestroyBar")
        {
            if (onMoveOut != null) onMoveOut(this);
        }
    }
}
