using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Player
{
    public class FPSCameraController : MonoBehaviour
    {
        [SerializeField] [Range(0, 100)] private float sensitivity;
        [SerializeField] private Transform pitch;
        [SerializeField] private Range<float> pitchRange;

        private Vector2 _input;

        private void Awake()
        {
            _input = Vector2.zero;
            transform.localEulerAngles = pitch.localEulerAngles = Vector3.zero;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            var rotation = _input * (sensitivity * Time.deltaTime);
            transform.Rotate(Vector3.up * rotation.x);
            pitch.Rotate(Vector3.right * rotation.y);

            // _targetRotation += _input * (sensitivity * Time.deltaTime);
            // _targetRotation.y = Mathf.Clamp(_targetRotation.y, pitchRange.min, pitchRange.max);
            // transform.localEulerAngles = Vector3.up * _targetRotation.x;
            // pitch.localEulerAngles = Vector3.right * _targetRotation.y;
        }

        private void OnEnable()
        {
            EventManager.StartListening(Events.Instance.inputActions.onLook, OnLook);
        }

        private void OnDisable()
        {
            EventManager.StopListening(Events.Instance.inputActions.onLook, OnLook);
        }

        private void OnLook(Dictionary<string, object> message)
        {
            _input = (Vector2) message["value"];
        }
    }
}