using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class HeadToPlayer : MonoBehaviour
{
    private void Start()
    {
        this.UpdateAsObservable()
            .Subscribe(_ => LookAtPlayer());
    }

    private void LookAtPlayer()
    {
        var p = Camera.main.transform.position;
        p.y = transform.position.y;
        transform.LookAt(p);
    }
}
