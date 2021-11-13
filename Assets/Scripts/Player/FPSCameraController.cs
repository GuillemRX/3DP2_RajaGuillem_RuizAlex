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
            transform.Rotate(0, rotation.x, 0);
            var pitchRotation = pitch.rotation.eulerAngles + new Vector3(rotation.y, 0f, 0f);
            pitchRotation.x = ClampAngle(pitchRotation.x, pitchRange.min, pitchRange.max);
            pitch.eulerAngles = pitchRotation;
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

        private float ClampAngle(float angle, float min, float max)
        {
            if (angle < 0f) angle = 360 + angle;
            if (angle > 180f) return Mathf.Max(angle, 360 + min);
            return Mathf.Min(angle, max);
        }
    }
}