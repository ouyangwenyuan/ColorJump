﻿using UnityEngine;

public class FollowAnObject : MonoBehaviour
{
    public Transform movingObject;

    public bool limitTop, limitBottom, limitLeft, limitRight;
    public float limitTopValue, limitBottomValue, limitLeftValue, limitRightValue;

    public virtual void Update()
    {
        float? posX = null, posY = null;
        if (limitTop && movingObject.position.y > limitTopValue) posY = limitTopValue;
        if (limitBottom && movingObject.position.y < limitBottomValue) posY = limitBottomValue ;
        if (limitLeft && movingObject.position.x < limitLeftValue) posX = limitLeftValue;
        if (limitRight && movingObject.position.x > limitRightValue) posX = limitRightValue;

        transform.position = new Vector3(posX ?? movingObject.position.x, posY ?? movingObject.position.y, transform.position.z);
    }
}
