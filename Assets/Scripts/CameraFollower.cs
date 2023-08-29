using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public static CameraFollower Instance { get; private set; }
    public Vector3 offset;
    private void Awake()
    {
        offset = transform.position;
        if (Instance != null && Instance != this) 
            Destroy(this); 
        else 
            Instance = this; 
    }

    public void UpdateTransform()
    {
        transform.position += new Vector3(0f, 0.1f, 0f);
    }    
}
