using UnityEngine;

namespace Triggers
{
    public class SceneTrigger : Trigger
    {
        public string sceneName; // scene name, dialogue path, etc
        public bool setStartPosition = true;
        public Vector3 startPosition; // starting position in new scene
    }
}
