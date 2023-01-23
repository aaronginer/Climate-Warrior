using UnityEngine;

namespace InventorySystem
{
    public class HandScript : MonoBehaviour
    {
        private void Update()
        {
            var mPos = Input.mousePosition;
            transform.position = mPos;
        }

        public void UpdatePosition()
        {
            var mPos = Input.mousePosition;
            transform.position = mPos;
        }
    }
}

