using System;

namespace Catastrophes
{
    [Serializable]
    public class CatastropheState
    {
        public States state;
        public enum States
        {
            None,
            Flooding,
            Drought
        }

        public CatastropheState()
        {
            state = States.None;
        }
    }
}
