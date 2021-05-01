using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class ObjectObserver : MonoBehaviour
{
    [SerializeField]
    private SightDetection sightDetection;

    private void Start()
    {
        sightDetection.OnDetected
            .DistinctUntilChanged()
            .Subscribe(_ =>
            {
                if (_ == true)
                {
                    Debug.Log("OnDetected");
                }
                else
                {
                    Debug.Log("Not Detected");
                }
            });
    }
}
