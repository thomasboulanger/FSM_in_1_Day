using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointList : MonoBehaviour
{
    public static CheckPointList singleton;

    public List<Transform> checkpointTransforms = new List<Transform>();
    private void Awake()
    {
        singleton = this;
        foreach (GameObject checkpoint in GameObject.FindGameObjectsWithTag("CheckPoint"))
        {
            checkpointTransforms.Add(checkpoint.transform);
        }
    }
}
