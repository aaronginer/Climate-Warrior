using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Dialogue
{
    public class DialogueDisplay : MonoBehaviour
    {
        public GameObject textBoxObj;
        private TextMeshProUGUI _textBox;
        public GameObject choice1Obj;
        private TextMeshProUGUI _textBoxChoice1;
        public GameObject choice2Obj;
        private TextMeshProUGUI _textBoxChoice2;
        public GameObject choice3Obj;
        private TextMeshProUGUI _textBoxChoice3;
        public GameObject choice4Obj;
        private TextMeshProUGUI _textBoxChoice4;

        private readonly GameObject[] _choiceObjs = new GameObject[4];
        private readonly TextMeshProUGUI[] _choiceTextBoxes = new TextMeshProUGUI[4];
        
        private DialogueReader _dialogueReader;
        private State _dialogueState;
        private int _choice;

        private enum State
        {
            NpcSpeak,
            PlayerOptions,
            PlayerSpeak,
            Cleanup,
            Finished
        }
        
        // Start is called before the first frame update
        void Start()
        {
            _textBox = textBoxObj.GetComponentInChildren<TextMeshProUGUI>();
            textBoxObj.SetActive(false);
            _textBoxChoice1 = choice1Obj.GetComponentInChildren<TextMeshProUGUI>();
            choice1Obj.SetActive(false);
            _textBoxChoice2 = choice2Obj.GetComponentInChildren<TextMeshProUGUI>();
            choice2Obj.SetActive(false);
            _textBoxChoice3 = choice3Obj.GetComponentInChildren<TextMeshProUGUI>();
            choice3Obj.SetActive(false);
            _textBoxChoice4 = choice4Obj.GetComponentInChildren<TextMeshProUGUI>();
            choice4Obj.SetActive(false);

            _choiceTextBoxes[0] = _textBoxChoice1;
            _choiceTextBoxes[1] = _textBoxChoice2;
            _choiceTextBoxes[2] = _textBoxChoice3;
            _choiceTextBoxes[3] = _textBoxChoice4;

            _choiceObjs[0] = choice1Obj;
            _choiceObjs[1] = choice2Obj;
            _choiceObjs[2] = choice3Obj;
            _choiceObjs[3] = choice4Obj;

            _dialogueReader = null;
            _dialogueState = State.NpcSpeak;
            _choice = 0;
        }
        
        public void SetDialogueReader(DialogueReader reader)
        {
            _dialogueReader = reader;
            _dialogueState = State.NpcSpeak;
        }

        public void OnClick(int index)
        {
            _choice = index;
            DialogueUpdate();
        }
        
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && _dialogueState != State.PlayerSpeak)
            {
                DialogueUpdate();
            }
        }

        public void DialogueUpdate()
        {
            if (_dialogueReader == null)
            {
                Debug.Log("No DialogueReader initialized.");
                return;
            }

            DialogueNode currentNode = _dialogueReader.GetCurrent();
            string[] options = currentNode.GetOptions();
            switch (_dialogueState)
            {
                case State.NpcSpeak:
                    textBoxObj.SetActive(true);
                    
                    _textBox.text = _dialogueReader.npcNamePrefix + _dialogueReader.GetCurrent().GetMessage();
                    _dialogueState = options.Length == 0 ? State.Cleanup : State.PlayerOptions;
                    break;
                case State.PlayerOptions:
                    textBoxObj.SetActive(false);
                    
                    for (int i = 0; i < options.Length; i++)
                    {
                        _choiceObjs[i].SetActive(true);
                        _choiceTextBoxes[i].text = options[i];
                    }

                    _dialogueState = State.PlayerSpeak;
                    break;
                case State.PlayerSpeak:
                    foreach (GameObject choiceObj in _choiceObjs)
                    {
                        choiceObj.SetActive(false);
                    }

                    textBoxObj.SetActive(true);
                    _textBox.text = "You: " + options[_choice];
                    _dialogueReader.Choice(options[_choice]);
                    _dialogueState = State.NpcSpeak;
                    break;
                case State.Cleanup:
                    textBoxObj.SetActive(false);
                    foreach (GameObject choiceObj in _choiceObjs)
                    {
                        choiceObj.SetActive(false);
                    }
                    break;
                case State.Finished:
                    break;
                default:
                    Debug.Log("An error occured updating the dialogue. Invalid State.");
                    break;
            }
        }
    }   
}
