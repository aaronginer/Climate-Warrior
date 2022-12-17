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
        private State _current;
        private State _next;
        private int _choice;

        private enum State
        {
            NpcSpeak,
            PlayerOptions,
            PlayerSpeak,
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
            _current = State.Finished;
            _next = State.Finished;
            _choice = 0;
        }
        
        public void StartNewDialogue(string dialogueFile)
        {
            if (_current != State.Finished || !UIStateManager.UISM.CanStartDialogue()) return;
            
            UIStateManager.UISM.uIState = UIState.Dialogue;
            
            _dialogueReader = new DialogueReader(dialogueFile);
            _next = State.NpcSpeak;

            DialogueUpdate();
        }

        public void OnClick(int index)
        {
            _choice = index;
            DialogueUpdate();
        }
        
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && _current != State.PlayerOptions)
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

            _current = _next;

            DialogueNode currentNode = _dialogueReader.GetCurrent();
            string[] options = currentNode.GetOptions();
            switch (_current)
            {
                case State.NpcSpeak: // show npc text
                    textBoxObj.SetActive(true);
                    
                    _textBox.text = _dialogueReader.npcNamePrefix + _dialogueReader.GetCurrent().GetMessage();
                    _next = options.Length == 0 ? State.Finished : State.PlayerOptions;
                    break;
                case State.PlayerOptions: // show the player option buttons
                    textBoxObj.SetActive(false);
                    
                    for (int i = 0; i < options.Length; i++)
                    {
                        _choiceObjs[i].SetActive(true);
                        _choiceTextBoxes[i].text = options[i];
                    }
                    
                    _next = State.PlayerSpeak;
                    break;
                case State.PlayerSpeak: // show player selection
                    foreach (GameObject choiceObj in _choiceObjs)
                    {
                        choiceObj.SetActive(false);
                    }

                    textBoxObj.SetActive(true);
                    _textBox.text = "You: " + options[_choice];
                    _dialogueReader.Choice(options[_choice]);
                    
                    _next = State.NpcSpeak;
                    break;
                case State.Finished: // dialogue is in default state
                    textBoxObj.SetActive(false);
                    foreach (GameObject choiceObj in _choiceObjs)
                    {
                        choiceObj.SetActive(false);
                    }
                    
                    UIStateManager.UISM.uIState = UIState.None;
                    break;
                default:
                    Debug.Log("An error occured updating the dialogue. Invalid State.");
                    break;
            }
        }
    }   
}
