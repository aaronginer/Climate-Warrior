using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Missions.Sabotage
{
    public class VirusGame : MonoBehaviour
    {
        public Sprite serverWindowActive;
        public Sprite serverWindowInactive;
        public Sprite cursorSettingsActive;
        public Sprite cursorSettingsInactive;
        public GameObject serverWindowButton;
        public GameObject serverWindow;
        public GameObject cursorSettingsButton;
        public GameObject cursorSettingsWindow;
        public GameObject inputFieldCommandLine;
        public GameObject outputFieldCommandLine;
        public GameObject spidersContainer;

        public GameObject cursor;

        public bool knifeSelected;
        public bool usbAttached;
        
        private bool _restarting;
        private bool _hasDiagnosed;

        private int _numSpiders;
        private const int MaxSpiders = 1;
        
        void Start()
        {
            Cursor.visible = false;

            _numSpiders = 0;
            _restarting = false;
            _hasDiagnosed = false;
            usbAttached = true;
            knifeSelected = false;
            
            ToggleServerWindow();
            ToggleCursorSettingsWindow();
        }

        // Update is called once per frame
        void Update()
        {
            cursor.transform.position = Input.mousePosition + new Vector3(7, -7, 0);

            if (Input.GetKeyDown(KeyCode.Return) && !_restarting && _numSpiders == 0)
            {
                if (serverWindow.activeSelf)
                {
                    TMP_InputField inField = inputFieldCommandLine.GetComponent<TMP_InputField>();
                    HandleCommandLineInput(inField.text);
                    inField.text = "";
                }
            }
        }

        private void HandleCommandLineInput(string input)
        {
            TextMeshProUGUI outField = outputFieldCommandLine.GetComponent<TextMeshProUGUI>();
            switch (input)
            {
                case "help":
                    outField.color = Color.white;
                    outField.text = "Available server commands:\n- restart\n- diagnose";
                    break;
                case "restart":
                    StartCoroutine(RestartServer(outField));
                    break;
                case "diagnose":
                    _hasDiagnosed = true;
                    StartCoroutine(DiagnoseServer(outField));
                    break;
                default:
                    outField.color = Color.white;
                    outField.text = "Unknown command. Enter \"help\" to get a list of available commands.";
                    break;
            }
        }

        IEnumerator RestartServer(TextMeshProUGUI outField)
        {
            _restarting = true;
            outField.color = Color.white;
            outField.text = "Starting server";
            yield return new WaitForSeconds(1);
            outField.text = "Starting server.";
            yield return new WaitForSeconds(1);
            outField.text = "Starting server..";
            yield return new WaitForSeconds(1);
            outField.text = "Starting server...";
            yield return new WaitForSeconds(1);
            if (!usbAttached && _hasDiagnosed)
            {
                outField.text = "Server status: RUNNING.";
                yield return new WaitForSeconds(1);
                GameStateManager.Instance.CurrentMission?.UpdateCurrentMissionState((int) MissionSabotage.States.ServerFixed);
                SceneManager.LoadScene("HydroPlantUpper");
            }
            else
            {
                outField.color = Color.red;
                outField.text = "Server crashed. Please run a diagnostic.";
                _hasDiagnosed = false;
            }
            _restarting = false;
        }

        IEnumerator DiagnoseServer(TextMeshProUGUI outField)
        {
            outField.color = Color.white;
            outField.text = "Starting diagnosis...";
            yield return new WaitForSeconds(1);
            bool flipFlop = true;
            _numSpiders = MaxSpiders;;
            for (int i = 0; i < MaxSpiders; i++)
            {
                yield return new WaitForSeconds(0.2f);
                flipFlop = !flipFlop;

                GameObject spiderPrefab = Resources.Load<GameObject>(flipFlop ? 
                    "Missions/Sabotage/SpiderPurple" : "Missions/Sabotage/SpiderOrange");
                
                GameObject spider = Instantiate(spiderPrefab, spidersContainer.transform);
                spider.GetComponent<Spider>().gameScript = this;
            }
        }
        
        public void ToggleCursorSettingsWindow()
        {
            cursorSettingsWindow.SetActive(!cursorSettingsWindow.activeSelf);
            
            cursorSettingsButton.GetComponent<Image>().sprite = cursorSettingsWindow.activeSelf ? cursorSettingsActive : cursorSettingsInactive;
        }
        
        public void ToggleServerWindow()
        {
            serverWindow.SetActive(!serverWindow.activeSelf);
            serverWindowButton.GetComponent<Image>().sprite = serverWindow.activeSelf ? serverWindowActive : serverWindowInactive;
        }

        public void Squish()
        {
            _numSpiders--;
        }

        public void SelectKnife()
        {
            knifeSelected = true;
        }

        public void DeselectKnife()
        {
            knifeSelected = false;
        }

        public void ExitGame()
        {
            SceneManager.LoadScene("HydroPlantUpper");
        }
    }   
}
