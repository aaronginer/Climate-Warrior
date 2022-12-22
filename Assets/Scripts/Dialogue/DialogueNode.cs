using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Dialogue
{
    public class DialogueNode
    {
        private int _id;
        private readonly string _message;
        private readonly Dictionary<string, int> _options;
        private readonly string[] _actions = {"", "", "", ""};

        public DialogueNode(int id, string message, Dictionary<string, int> options)
        {
            _id = id;
            _message = message;
            _options = options;
        }

        public void UpdateActions(string[] actions)
        {
            for (int i = 0; i < actions.Length && i < 4; i++)
            {
                _actions[i] = actions[i];
            }
        }

        public string[] GetOptions()
        {
            return _options.Keys.ToArray();
        }
        
        public string[] GetActions()
        {
            return _actions;
        }
    
        /*
         * Return the id of the next dialogue node depending on <option>
         */
        public int GetChoiceId(string option)
        {
            return _options[option];
        }

        public string GetMessage()
        {
            return _message;
        }
    }   
}
