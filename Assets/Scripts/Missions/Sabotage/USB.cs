using UnityEngine;

namespace Missions.Sabotage
{
    public class USB : MonoBehaviour
    {
        public GameObject game;
        public GameObject canvas;
        public GameObject usbSlot;

        private VirusGame _gameScript;
        private Rigidbody2D _rigidbody2D;
        private Vector3 _clickPositionDiffVector;

        private bool _clicked;

        private void Start()
        {
            _gameScript = game.GetComponent<VirusGame>();
            _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        }

        public void Update()
        {
            if (!(Input.GetMouseButton(0) && _clicked)) return;
            
            gameObject.transform.position = Input.mousePosition - _clickPositionDiffVector;
        }

        public void Click()
        {
            if (!Input.GetMouseButtonDown(0)) return;
            
            _clickPositionDiffVector = Input.mousePosition - gameObject.transform.position;
            _clicked = true;
        }

        public void Release()
        {
            if (!Input.GetMouseButtonUp(0)) return;
            
            if (Vector3.Distance(usbSlot.transform.position, gameObject.transform.position) < 60)
            {
                _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
                transform.position = usbSlot.transform.position;
                transform.rotation = usbSlot.transform.rotation;

                _gameScript.usbAttached = true;
            }
            else
            {
                _rigidbody2D.constraints = RigidbodyConstraints2D.None;
                _gameScript.usbAttached = false;
            }

            _clicked = false;
        }
    }
}