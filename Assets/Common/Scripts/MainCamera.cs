﻿using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class MainCamera : MonoBehaviour
{
    private float width;
    private float height;
    public float virtualWidth;
    public float virtualHeight;
    public bool landscape = false;

    private int screenWidth, screenHeight;

    public Action onScreenSizeChanged;
    public static MainCamera instance;

    private void Awake()
    {
        instance = this;
        UpdateCamera();
    }

    private void Update()
    {
        if (screenWidth != Screen.width || screenHeight != Screen.height)
        {
            UpdateCamera();
            if (onScreenSizeChanged != null) onScreenSizeChanged();
        }
    }

    private void UpdateCamera()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        if (landscape)
        {
            SetUpForLandscape();
        }
        else
        {
            SetUpForPortrait();
        }
    }

    private void SetUpForPortrait()
    {
        float aspect = Screen.height / (float)Screen.width;
        // 1 unit = 100 pixel. Height and width are considered as their half size -> 200.

        float targetHeight = CommonConst.TARGET_HEIGHT / 200f;
        float targetWidth = CommonConst.TARGET_WIDTH / 200f;

        if (aspect < CommonConst.MIN_ASPECT)
        {
            height = targetWidth * CommonConst.MIN_ASPECT;
            GetComponent<Camera>().orthographicSize = height;
            virtualHeight = height * 200;
            virtualWidth = virtualHeight / aspect;
        }
        else
        {
            GetComponent<Camera>().orthographicSize = targetWidth * aspect;
            virtualWidth = targetWidth * 200;
            virtualHeight = virtualWidth * aspect;
            if (aspect > CommonConst.MAX_ASPECT)
            {
                height = targetHeight;
            }
            else
            {
                height = targetWidth * aspect;
            }
        }

        width = targetWidth;
    }

    private void SetUpForLandscape()
    {
        float aspect = Screen.width / (float)Screen.height;
        // 1 unit = 100 pixel. Height and width are considered as their half size -> 200.

        float targetHeight = CommonConst.TARGET_HEIGHT / 200f;
        float targetWidth = CommonConst.TARGET_WIDTH / 200f;

        if (aspect < CommonConst.MIN_ASPECT)
        {
            GetComponent<Camera>().orthographicSize = targetWidth / aspect;
            virtualWidth = targetWidth * 200;
            virtualHeight = virtualWidth / aspect;
            width = targetWidth;
        }
        else
        {
            GetComponent<Camera>().orthographicSize = targetHeight;
            virtualHeight = targetHeight * 200;
            virtualWidth = virtualHeight * aspect;
            if (aspect > CommonConst.MAX_ASPECT)
            {
                width = targetWidth;
            }
            else
            {
                width = targetHeight * aspect;
            }
        }

        height = targetHeight;
    }

    // Half of the real height.
    public float GetHeight()
    {
        return height;
    }

    // Half of the real width.
    public float GetWidth()
    {
        return width;
    }
}
