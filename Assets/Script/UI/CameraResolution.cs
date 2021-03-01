using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    [SerializeField] private float mboardUnit;
    private Camera mCam;
    void Awake()
    {
        mCam = GetComponent<Camera>();
        if (mboardUnit == 0) mboardUnit = 10;
        mCam.orthographicSize = mboardUnit / mCam.aspect;
    }

}
