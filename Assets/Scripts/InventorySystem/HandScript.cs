using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEditor;
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

