using UnityEngine;
using System.Collections;

// Only for testing.
public class Movement : MonoBehaviour {
    public Transform destination;
    public Vector3 velocity = Vector3.zero;
    public float smoothTime = 3F;

    private void Start()
    {
        //iTween.MoveTo(gameObject, iTween.Hash("position", destination, "orienttopath", true, "time", 3f));
        //transform.Rotate(0, 90, 90);
        //transform.position = new Vector3(Mathf.Clamp(Time.time, 1.0f, 3.0f), 0, 0);

        Vector3[] waypoints = iTweenPath.GetPath("complex_1");
        iTween.MoveTo(gameObject, iTween.Hash("path", waypoints, "orienttopath", true, "time", 10f));
    }

    private void Update()
    {
        //Vector3 targetPosition = (new Vector3(0, 3, 0));
        //transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
