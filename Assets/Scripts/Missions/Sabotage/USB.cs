using System;
using UnityEngine;

namespace Missions.Sabotage
{
    public class USB : MonoBehaviour
    {
        public GameObject game;

        private VirusGame _gameScript;
        private Rigidbody2D _rigidbody2D;
        private Vector3 _positionStart;
        private Quaternion _rotationStart;
        private Vector3 _clickPositionDiffVector;

        private void Start()
        {
            _gameScript = game.GetComponent<VirusGame>();
            _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
            _positionStart = transform.position;
            _rotationStart = transform.rotation;
        }

        public void Drag()
        {
            if (Input.GetMouseButton(0))
            {
                gameObject.transform.position = Input.mousePosition - _clickPositionDiffVector;
            }
        }

        public void Click()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _clickPositionDiffVector = Input.mousePosition - gameObject.transform.position;
            }
        }

        public void Release()
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (Vector3.Distance(_positionStart, gameObject.transform.position) < 60)
                {
                    _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
                    transform.position = _positionStart;
                    transform.rotation = _rotationStart;

                    _gameScript.usbAttached = true;
                }
                else
                {
                    _rigidbody2D.constraints = RigidbodyConstraints2D.None;
                    _gameScript.usbAttached = false;
                }
            }
        }
    }
}