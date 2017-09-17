﻿using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class MovingCamera : FollowAnObject
{
    public static MovingCamera instance;

    private void Awake()
    {
        instance = this;
        GetComponent<Camera>().orthographicSize = MainCamera.instance.GetComponent<Camera>().orthographicSize;
    }
}
