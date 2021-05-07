using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class ObjectObserver : MonoBehaviour
{
    [SerializeField]
    private SightDetection sightDetection;

    [SerializeField]
    private GameObject buttons;

    private void Start()
    {
        sightDetection.OnDetected
            .DistinctUntilChanged()
            .Subscribe(_ =>
            {
                buttons.SetActive(_);

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
