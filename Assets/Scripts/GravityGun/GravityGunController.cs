using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace GravityGun
{
    public class GravityGunController : MonoBehaviour
    {
        private Vector3 _currentPosition, _currentForward;
        private Rigidbody _pickedTarget;
        private PickableObject _pickedTargetState;

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

            EventManager.TriggerEvent(Events.Instance.playerActions.onGravityGunDrop, new Dictionary<string, object>
            {
                {"source", gameObject}
            });

            if (_pickedTargetState) _pickedTargetState.stopHolding();
            _pickedTargetState = null;
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
                _pickedTargetState = hit.collider.gameObject.GetComponent<PickableObject>();
                if (_pickedTargetState) _pickedTargetState.startHolding();

                _pickedTarget.isKinematic = true;
                EventManager.TriggerEvent(Events.Instance.playerActions.onGravityGunGrab, new Dictionary<string, object>
                {
                    {"source", gameObject}
                });
            }
            else
            {
                GravityGunUtilities.ShootObject(_pickedTarget);
                Turret turret = _pickedTarget.gameObject.GetComponent<Turret>();
                if(turret) turret.StopTurret();
                _pickedTarget = null;
                EventManager.TriggerEvent(Events.Instance.playerActions.onGravityGunThrow,
                    new Dictionary<string, object>
                    {
                        {"source", gameObject}
                    });
            }
        }
    }
}