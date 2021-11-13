using UnityEngine;

namespace Player
{
    public class RigidBodySpeedLimiter : MonoBehaviour
    {
        [SerializeField] private float maxSpeed = 100f;
        
        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            var speed = Vector3.Magnitude(_rigidbody.velocity);

            if (!(speed > maxSpeed)) return;
            
            var brakeSpeed = speed - maxSpeed;

            var normalisedVelocity = _rigidbody.velocity.normalized;
            var brakeVelocity = normalisedVelocity * brakeSpeed;

            _rigidbody.AddForce(-brakeVelocity);
        }
    }
}