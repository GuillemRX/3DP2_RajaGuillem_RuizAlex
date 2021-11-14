using UnityEngine;

namespace Player
{
    public class GroundDetector : MonoBehaviour
    {
        private Collider _collider;
        public bool IsGrounded { get; private set; }

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _collider.enabled = false;
        }

        private void FixedUpdate()
        {
            var bounds = _collider.bounds;
            var results = new Collider[1];
            var size = Physics.OverlapBoxNonAlloc(bounds.center, bounds.extents, results, transform.rotation,
                GameSettings.Instance.playerGround);

            IsGrounded = size != 0;
        }
    }
}