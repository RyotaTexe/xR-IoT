using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;
using UniRx.Triggers;
using System;

public class SightDetection : MonoBehaviour
{
    [SerializeField]
    private Transform targetTransform;

    private Transform cameraDirection;

    private Subject<bool> detectSight = new Subject<bool>();

    public IObservable<bool> OnDetected
    {
        get { return detectSight; }
    }

    private void Start()
    {
        cameraDirection = Camera.main.transform;

        this.UpdateAsObservable()
            .Subscribe(_ => CameraDegree());
    }

    private bool CameraDegree()
    {
        Vector3 targetToCameraDirection_N = (cameraDirection.position - targetTransform.position).normalized;

        if (Vector3.Dot(targetToCameraDirection_N, cameraDirection.forward.normalized) < -0.9f)
        {
            detectSight.OnNext(true);
            return true;
        }
        else
        {
            detectSight.OnNext(false);
            return false;
        }
    }
}
