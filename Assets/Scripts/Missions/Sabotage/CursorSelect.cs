using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Missions.Sabotage
{
    public class CursorSelect : MonoBehaviour
    {
        public GameObject selection;
        public GameObject cursor;

        public void Select()
        {
            selection.transform.SetParent(gameObject.transform);
            selection.transform.localPosition = new Vector3(0, 0, 0);
            cursor.GetComponent<Image>().sprite = gameObject.GetComponent<Image>().sprite;
        }
    }
}
