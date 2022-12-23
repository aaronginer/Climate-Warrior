using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Triggers
{
    public class SceneTrigger : Trigger
    {
        public string sceneName; // scene name, dialogue path, etc
        public Vector3 startPosition; // starting position in new scene
    }
}
