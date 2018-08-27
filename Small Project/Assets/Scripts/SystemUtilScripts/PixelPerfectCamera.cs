using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelPerfectCamera : MonoBehaviour
{

    public float pixelsToUnits = 100;
    public float zoom = 2;
    private Camera cam;

    void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
        cam.orthographicSize = Screen.height / pixelsToUnits / (zoom);
    }
}
