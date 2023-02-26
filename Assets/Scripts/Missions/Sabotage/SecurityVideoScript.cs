using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace Missions.Sabotage
{
    public class SecurityVideoScript : MonoBehaviour
    {
        public GameObject footageView;
        public VideoPlayer videoPlayer;

        public void PlayVideo(VideoClip clip)
        {
            
        }
        
        public void SetVideoSpeed(float speed)
        {
            videoPlayer.playbackSpeed = speed;
        }

        public void PauseVideo()
        {
            videoPlayer.Pause();
        }

        public void ResumeVideo()
        {
            videoPlayer.Play();
        }

        public void Back()
        {
            footageView.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}

