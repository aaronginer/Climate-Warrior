using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using UnityEditor.VersionControl;
using UnityEngine;

namespace Dialogue
{
    public class DialogueNode
    {
        private int _id;
        private readonly string _message;
        private readonly Dictionary<string, int> _options;

        public DialogueNode(int id, string message, Dictionary<string, int> options)
        {
            _id = id;
            _message = message;
            _options = options;
        }

        public string[] GetOptions()
        {
            return _options.Keys.ToArray();
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
