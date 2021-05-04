using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UniRx;

namespace xRIoT.VideoWindow
{
    public class VideoWindow : MonoBehaviour
    {
        [SerializeField]
        private VideoPlayerWithRawImage m_VideoPlayerWithRawImage = null;

        private readonly BehaviorSubject<VideoClip> m_ChangeVideoClip = new BehaviorSubject<VideoClip>(null);
        public IObserver<VideoClip> ChangeVideoClip => this.m_ChangeVideoClip;
        public IObservable<VideoState> NotifyVideoEnd
            => this.m_VideoPlayerWithRawImage.NotifyVideoState.Where(_ => _ == VideoState.Stop).First();

        private void Awake()
        {
            this.gameObject.SetActive(false);

            this.m_ChangeVideoClip
                .StartWith()
                .Where(_ => this.m_VideoPlayerWithRawImage != null)
                .Subscribe(this.m_VideoPlayerWithRawImage.ChangeVideoClip)
                .AddTo(this);
        }

        public void OnButtonPressed()
        {
            Debug.Log("Button was Pressed");
        }
    }
}

