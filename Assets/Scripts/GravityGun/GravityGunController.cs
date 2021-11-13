using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace GravityGun
{
    public class GravityGunController : MonoBehaviour
    {
        private Vector3 _currentPosition, _currentForward;
        private Rigidbody _pickedTarget;

        private void Update()
        {
            if (!_pickedTarget) return;

            var cameraTransform = Camera.main.transform;
            var cameraForward = cameraTransform.forward;
            var targetPosition = cameraTransform.position +
                                 cameraForward * GameSettings.Instance.gravityGunCarryingDistance;

            _pickedTarget.position =
                Vector3.SmoothDamp(_pickedTarget.position, targetPosition, ref _currentPosition,
                    GameSettings.Instance.gravityGunCarryingSmoothing);
            _pickedTarget.transform.forward =
                Vector3.SmoothDamp(_pickedTarget.transform.forward, cameraForward, ref _currentForward,
                    GameSettings.Instance.gravityGunCarryingSmoothing);
        }

        private void OnEnable()
        {
            EventManager.StartListening(Events.Instance.inputActions.onLeftClick, OnLeftClick);
            EventManager.StartListening(Events.Instance.inputActions.onRightClick, OnRightClick);
        }

        private void OnDisable()
        {
            EventManager.StopListening(Events.Instance.inputActions.onLeftClick, OnLeftClick);
            EventManager.StopListening(Events.Instance.inputActions.onRightClick, OnRightClick);
        }

        private void OnRightClick(Dictionary<string, object> message)
        {
            var isPressed = (bool) message["value"];

            if (!isPressed || !_pickedTarget) return;

            _pickedTarget.isKinematic = false;
            _pickedTarget = null;
        }

        private void OnLeftClick(Dictionary<string, object> message)
        {
            var isPressed = (bool) message["value"];

            if (!isPressed) return;

            if (!_pickedTarget)
            {
                var cameraTransform = Camera.main.transform;
                var cameraPosition = cameraTransform.position;
                var ray = new Ray(cameraPosition, cameraTransform.forward);

                if (!GravityGunUtilities.RayCastGravityObject(ray, out var hit)) return;

                _pickedTarget = hit.collider.gameObject.GetComponent<Rigidbody>();
                _pickedTarget.isKinematic = true;
            }
            else
            {
                GravityGunUtilities.ShootObject(_pickedTarget);
                _pickedTarget = null;
            }
        }
    }
}