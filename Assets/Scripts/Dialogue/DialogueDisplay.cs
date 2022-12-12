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
            _textBox = textBoxObj.GetComponent<TextMeshProUGUI>();
            _textBox.enabled = false;
            _textBoxChoice1 = choice1Obj.GetComponent<TextMeshProUGUI>();
            _textBoxChoice1.enabled = false;
            _textBoxChoice2 = choice2Obj.GetComponent<TextMeshProUGUI>();
            _textBoxChoice2.enabled = false;
            _textBoxChoice3 = choice3Obj.GetComponent<TextMeshProUGUI>();
            _textBoxChoice3.enabled = false;
            _textBoxChoice4 = choice4Obj.GetComponent<TextMeshProUGUI>();
            _textBoxChoice4.enabled = false;

            _choiceTextBoxes[0] = _textBoxChoice1;
            _choiceTextBoxes[1] = _textBoxChoice2;
            _choiceTextBoxes[2] = _textBoxChoice3;
            _choiceTextBoxes[3] = _textBoxChoice4;

            _dialogueReader = null;
            _dialogueState = State.NpcSpeak;
            _choice = 0;
        }
        
        public void SetDialogueReader(DialogueReader reader)
        {
            _dialogueReader = reader;
            _dialogueState = State.NpcSpeak;
        }

        public void Update()
        {
            if (_dialogueState == State.PlayerSpeak)
            {
                for (int key = (int) KeyCode.Alpha1; key <= (int) KeyCode.Alpha4; key++)
                {
                    if (Input.GetKeyDown((KeyCode) key))
                    {
                        _choice = key - (int) KeyCode.Alpha1;
                        DialogueUpdate();
                        break;
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.Space))
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
                    _textBox.enabled = true;
                    
                    _textBox.text = "NPC: " + _dialogueReader.GetCurrent().GetMessage();
                    _dialogueState = options.Length == 0 ? State.Cleanup : State.PlayerOptions;
                    break;
                case State.PlayerOptions:
                    _textBox.enabled = false;
                    
                    for (int i = 0; i < options.Length; i++)
                    {
                        _choiceTextBoxes[i].enabled = true;
                        _choiceTextBoxes[i].text = i+1 + " - " + options[i];
                    }

                    _dialogueState = State.PlayerSpeak;
                    break;
                case State.PlayerSpeak:
                    foreach (TextMeshProUGUI choiceBox in _choiceTextBoxes)
                    {
                        choiceBox.enabled = false;
                    }

                    _textBox.enabled = true;
                    _textBox.text = "You: " + options[_choice];
                    _dialogueReader.Choice(options[_choice]);
                    _dialogueState = State.NpcSpeak;
                    break;
                case State.Cleanup:
                    _textBox.enabled = false;
                    foreach (TextMeshProUGUI choiceBox in _choiceTextBoxes)
                    {
                        choiceBox.enabled = false;
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
