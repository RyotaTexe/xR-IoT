using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UniRx;
using UniRx.Triggers;
using xRIoT.Util;
using Cysharp.Threading.Tasks;
using System.Linq;

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
        private Renderer m_PlaneMaterial = null;

        // 動画を変更
        private readonly BehaviorSubject<VideoClip> m_ChangeVideoClip = new BehaviorSubject<VideoClip>(null);
        public IObserver<VideoClip> ChangeVideoClip => this.m_ChangeVideoClip;

        // 動画の状態
        private readonly Subject<VideoState> m_NotifyVideoState = new Subject<VideoState>();
        public IObservable<VideoState> NotifyVideoState => this.m_NotifyVideoState;

        // 動画の番号
        private int videoCount = 0;


        private void Awake()
        {
            var preparedReactive
                = this.m_VideoPlayer
                .ObserveEveryValueChanged(_ => _.isPrepared)
                .ToSequentialReadOnlyReactiveProperty();

            this.m_PlaneMaterial
                .ObserveEveryValueChanged(_ => _.material.mainTexture)
                .Select(_ => _ != null ? Color.white : Color.black)
                .Subscribe(_ => this.m_PlaneMaterial.material.color = _)
                .AddTo(this.m_PlaneMaterial);

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
            => this.VideoPlayerUpdate(this.m_VideoPlayer, this.m_PlaneMaterial);

        private void VideoPlayerUpdate(VideoPlayer videoPlayer, Renderer rawImage)
        {
            rawImage.material.mainTexture = videoPlayer.texture;
        }

        private void ChangeVideo(string path)
        {
            path.ResourcesLoadAsync<VideoClip>()
                .ToObservable()
                .Where(_ => _ != null)
                .Subscribe(ChangeVideoClip.OnNext);
        }

        public void OnNextVideo()
        {
            if(VideoPathArray.VideoPaths.Count() > videoCount)
            {
                videoCount++;
                ChangeVideo(VideoPathArray.VideoPaths.ElementAt(videoCount));
            }
        }

        public void OnPrevVideo()
        {
            if (0 <= videoCount)
            {
                videoCount--;
                ChangeVideo(VideoPathArray.VideoPaths.ElementAt(videoCount));
            }
        }
    }
}

