using UnityEngine;

namespace Triggers
{
    public class Trigger : MonoBehaviour
    {
        public string scene; // the scene the trigger is positioned in
        public bool manual; // is the trigger a manual trigger?
        public string text; // text to be displayed when trigger is manual
        public MiniGame requireCompleted = MiniGame.None; // requirement of completed Minigame
    }
}