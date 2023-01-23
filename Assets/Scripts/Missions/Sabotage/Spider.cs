using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

namespace Missions.Sabotage
{
    public class Spider : MonoBehaviour
    {
        public Sprite squishedSprite;
        public VirusGame gameScript;
        
        private RectTransform _rectTransform;
        private Rigidbody2D _rigidbody2D;
        private float _changeDirectionCountdown = 2;
        private bool _squished;
        
        private const float Speed = 1f;

        private void Awake()
        {
            _squished = false;
            _rectTransform = gameObject.GetComponent<RectTransform>();
            _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            UpdateSpiderOrientationAndVelocity();
        }

        public void Squish()
        {
            if (!gameScript.knifeSelected) return;

            gameScript.Squish();
            
            _squished = true;
            
            Destroy(_rigidbody2D);
            Destroy(gameObject.GetComponent<Animator>());
            Destroy(gameObject.GetComponent<Button>());
            
            gameObject.GetComponent<Image>().sprite = squishedSprite;
            
            StartCoroutine(SquishDestroy());
        }

        IEnumerator SquishDestroy()
        {
            yield return new WaitForSeconds(1f);
            Destroy(gameObject);
        }
        
        void Update()
        {
            if (_squished) return;
            
            _changeDirectionCountdown -= Time.deltaTime;

            if (!(_changeDirectionCountdown <= 0)) return;
            
            _changeDirectionCountdown = 0.5f;
            
           UpdateSpiderOrientationAndVelocity();
        }

        private void UpdateSpiderOrientationAndVelocity()
        {
            Quaternion randRot = Quaternion.Euler(0, 0, Random.value * 360);
            _rectTransform.localRotation = randRot;
            
            Vector3 move = new Vector3(0, -Speed, 0);
            
            var localPosition = _rigidbody2D.transform.localPosition;
            Vector3 newLocalPos = localPosition + move;
            Vector3 moveLocalVector = newLocalPos - localPosition;
            
            Vector3 worldDirection = transform.TransformVector(moveLocalVector);
            _rigidbody2D.velocity = worldDirection * (100 +Random.value * 200);
        }
    }
}
