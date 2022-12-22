using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Missions.Sabotage
{
    public class VirusGame : MonoBehaviour
    {
        public Sprite serverWindowActive;
        public Sprite serverWindowInactive;
        public GameObject serverWindowButton;
        public GameObject serverWindow;
        public GameObject inputFieldCommandLine;
        public GameObject outputFieldCommandLine;
        public GameObject spidersContainer;

        public GameObject knifeCursor;

        private bool _restarting;
        private bool _usbAttached;

        private int _numSpiders = 0;
        private const int maxSpiders = 16;
        
        void Start()
        {
            _restarting = false;
            _usbAttached = true;
            
            ToggleServerWindow();
        }

        // Update is called once per frame
        void Update()
        {
            knifeCursor.transform.position = Input.mousePosition;
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
            outField.color = Color.red;
            outField.text = "Server crashed.";
            _restarting = false;
        }

        IEnumerator DiagnoseServer(TextMeshProUGUI outField)
        {
            outField.color = Color.white;
            outField.text = "Starting diagnosis...";
            yield return new WaitForSeconds(1);
            bool flipFlop = true;
            _numSpiders = maxSpiders;
            for (int i = 0; i < maxSpiders; i++)
            {
                yield return new WaitForSeconds(0.2f);
                flipFlop = !flipFlop;

                GameObject spider = Resources.Load<GameObject>(flipFlop ? 
                    "Missions/Sabotage/SpiderPurple" : "Missions/Sabotage/SpiderOrange");
                
                Instantiate(spider, spidersContainer.transform);
            }
        }
        

        public void ToggleServerWindow()
        {
            serverWindow.SetActive(!serverWindow.activeSelf);
            serverWindowButton.GetComponent<Image>().sprite = serverWindow.activeSelf ? serverWindowActive : serverWindowInactive;
            ToggleKnifeCursor();
        }

        public void ToggleKnifeCursor()
        {
            knifeCursor.SetActive(!knifeCursor.activeSelf);
        }
    }   
}
