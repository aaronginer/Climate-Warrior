using System.Collections.Generic;
using Items;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using Random = UnityEngine.Random;

namespace Missions.Sabotage
{
    public class SecurityFootageViewScript : MonoBehaviour
    {
        public GameObject videoPlayerObj;
        public VideoPlayer videoPlayer;
        public List<GameObject> slots;

        public VideoClip defaultClip;
        public List<VideoClip> customClips; // nr 4 in list is always the correct one

        private Dictionary<int, VideoClip> _clipMap = new ();
        private int _correctIndex;
        private GameObject _selectedSlot;
        private int _selectedSlotIdx;

        private void Start()
        {
            List<int> customClipIndices = new List<int>{0, 1, 2, 3};
            for (int i = 0; i < 28; i++) // 8, 9, 10, 11 are custom clips
            {
                if (i != 8 && i != 9 && i != 10 && i != 11)
                {
                    _clipMap.Add(i, defaultClip);
                    continue;
                }

                int rndIndex = Random.Range(0, customClipIndices.Count);
                int clipIndex = customClipIndices[rndIndex];

                if (clipIndex == 3)
                {
                    _correctIndex = i;
                }
                customClipIndices.RemoveAt(rndIndex);
                _clipMap.Add(i, customClips[clipIndex]);
            }
        }

        public void OnImageClick(int number)
        {
            if (_selectedSlot != null)
            {
                _selectedSlot.GetComponent<Image>().color = Color.white;
                if (_selectedSlot == slots[number])
                {
                    _selectedSlot = null;
                    _selectedSlotIdx = 0;
                    return;
                }
            }

            _selectedSlot = slots[number];
            _selectedSlotIdx = number;
            _selectedSlot.GetComponent<Image>().color = new Color(0.745283f, 0.4949804f, 0.4949804f);
        }
        
        public void View()
        {
            if (_selectedSlot == null)
            {
                return; // no clip selected
            }
            videoPlayerObj.SetActive(true);
            videoPlayer.clip = _clipMap[_selectedSlotIdx];
            videoPlayer.frame = 0;
            videoPlayer.playbackSpeed = 1.0f;
            gameObject.SetActive(false);
        }

        public void SaveToFloppyDisk()
        {
            if (_selectedSlot == null)
            {
                return; // no clip selected
            }
            
            GameStateManager.Instance.gameState.playerData.inventory.AddItem(ItemType.FloppyDisk);
            if (_correctIndex == _selectedSlotIdx)
            {
                // state correct
                GameStateManager.Instance.CurrentMission.State.stateID = (int)MissionSabotage.States.GotCorrectTape;
            }
            else
            {
                // state incorrect
                GameStateManager.Instance.CurrentMission.State.stateID = (int)MissionSabotage.States.GotIncorrectTape;
            }
            SceneManager.LoadScene(GameStateManager.Instance.gameState.playerData.sceneName);
        }
    }
}

