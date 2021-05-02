using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UniRx;
using UniRx.Triggers;

namespace xRIoT.VideoWindow
{
    public enum VideoState
    {
        None,
        Playback,
        Pause,
        Stop,
    }

    public class VideoPlayerWithRawImage : MonoBehaviour
    {
        [SerializeField]
        private VideoPlayer m_VideoPlayer = null;

        [SerializeField]
        private RawImage m_RawImage = null;

        // 動画を変更
        private readonly BehaviorSubject<VideoClip> m_ChangeVideoClip = new BehaviorSubject<VideoClip>(null);
        public IObserver<VideoClip> ChangeVideoClip => this.m_ChangeVideoClip;

        // 動画の状態
        private readonly Subject<VideoState> m_NotifyVideoState = new Subject<VideoState>();
        public IObservable<VideoState> NotifyVideoState => this.m_NotifyVideoState;


        private void Awake()
        {
            var preparedReactive
                = this.m_VideoPlayer
                .ObserveEveryValueChanged(_ => _.isPrepared)
                .ToSequentialReadOnlyReactiveProperty();

            this.m_RawImage
                .ObserveEveryValueChanged(_ => _.texture)
                .Select(_ => _ != null ? Color.white : Color.black)
                .Subscribe(_ => this.m_RawImage.color = _)
                .AddTo(this.m_RawImage);

            // 動画変更
            this.m_ChangeVideoClip
                .Where(_ => _ != null)
                .Subscribe(_ =>
                {
                    this.m_VideoPlayer.clip = _;
                    this.StartVideo();
                })
                .AddTo(this.m_VideoPlayer);

            // 動画更新
            this.UpdateAsObservable()
                .Where(_ => preparedReactive.Value)
                .Subscribe(_ => this.VideoPlayerUpdate())
                .AddTo(this);
        }

        private void OnEnable()
        {
            StartVideo();
        }

        private void StartVideo()
        {
            this.m_VideoPlayer.Play();
            this.m_NotifyVideoState.OnNext(VideoState.Playback);
        }

        private void PauseVideo()
        {
            this.m_VideoPlayer.Pause();
            this.m_NotifyVideoState.OnNext(VideoState.Pause);
        }

        private void StopVideo()
        {
            this.m_VideoPlayer.Stop();
            this.m_NotifyVideoState.OnNext(VideoState.Stop);
        }

        private void VideoPlayerUpdate()
            => this.VideoPlayerUpdate(this.m_VideoPlayer, this.m_RawImage);

        private void VideoPlayerUpdate(VideoPlayer videoPlayer,RawImage rawImage)
        {
            rawImage.texture = videoPlayer.texture;
        }
    }
}

