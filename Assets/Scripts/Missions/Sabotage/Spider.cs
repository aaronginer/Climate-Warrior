using System.Numerics;
using UnityEngine;
using UnityEngine.Experimental.AI;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

namespace Missions.Sabotage
{
    public class Spider : MonoBehaviour
    {
        private RectTransform _rectTransform;
        private Rigidbody2D _rigidbody2D;
        private float _changeDirectionCountdown = 2;

        private float speed = 1f;
        
        private void Awake()
        {
            _rectTransform = gameObject.GetComponent<RectTransform>();
            _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            UpdateSpiderOrientationAndVelocity();
        }

        void Update()
        {
            _changeDirectionCountdown -= Time.deltaTime;

            if (!(_changeDirectionCountdown <= 0)) return;
            
            _changeDirectionCountdown = 0.5f;
            
           UpdateSpiderOrientationAndVelocity();
        }

        private void UpdateSpiderOrientationAndVelocity()
        {
            Quaternion randRot = Quaternion.Euler(0, 0, Random.value * 360);
            _rectTransform.localRotation = randRot;
            
            Vector3 move = new Vector3(0, -speed, 0);
            
            var localPosition = _rigidbody2D.transform.localPosition;
            Vector3 newLocalPos = localPosition + move;
            Vector3 moveLocalVector = newLocalPos - localPosition;
            
            Vector3 worldDirection = transform.TransformVector(moveLocalVector);
            _rigidbody2D.velocity = worldDirection * (100 +Random.value * 200);
        }
    }
}
