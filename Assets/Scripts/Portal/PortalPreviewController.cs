using System.Collections.Generic;
using System.Numerics;
using DG.Tweening;
using UnityEngine;
using Utilities;
using Utilities.Singleton;
using Vector3 = UnityEngine.Vector3;

namespace Portal
{
    public class PortalPreviewController : SingletonMonoBehaviour<PortalPreviewController>
    {
        private Vector3 _originalBluePortalLocalScale, _originalOrangePortalLocalScale;
        private bool _previewBluePortal, _previewOrangePortal;

        private void Awake()
        {
            _originalBluePortalLocalScale = PortalManager.Instance.BluePortal.Portal.transform.localScale;
            _originalOrangePortalLocalScale = PortalManager.Instance.OrangePortal.Portal.transform.localScale;
        }

        private void Update()
        {
            if (_previewBluePortal)
                PlacePreview(PortalManager.Instance.BluePortal.Preview, PortalManager.Instance.BluePortal.Bounds);
            else PortalManager.Instance.BluePortal.Preview.SetActive(false);
            if (_previewOrangePortal)
                PlacePreview(PortalManager.Instance.OrangePortal.Preview, PortalManager.Instance.OrangePortal.Bounds);
            else PortalManager.Instance.OrangePortal.Preview.SetActive(false);
        }

        private void OnEnable()
        {
            EventManager.StartListening(Events.Instance.inputActions.onLeftClick, OnLeftClick);
            EventManager.StartListening(Events.Instance.inputActions.onRightClick, OnRightClick);
            EventManager.StartListening(Events.Instance.inputActions.onScroll, OnScroll);
        }

        private void OnDisable()
        {
            EventManager.StopListening(Events.Instance.inputActions.onLeftClick, OnLeftClick);
            EventManager.StopListening(Events.Instance.inputActions.onRightClick, OnRightClick);
            EventManager.StopListening(Events.Instance.inputActions.onScroll, OnScroll);
        }

        private void OnScroll(Dictionary<string, object> message)
        {
            var value = (float) message["value"];
            var orangePreview = PortalManager.Instance.OrangePortal.Preview;
            var bluePreview = PortalManager.Instance.BluePortal.Preview;

            if (orangePreview.activeSelf)
            {
                if (value > 0) orangePreview.transform.localScale += Vector3.one * 0.05f;
                if (value < 0) orangePreview.transform.localScale -= Vector3.one * 0.05f;
            }
            else if (bluePreview.activeSelf)
            {
                if (value > 0) bluePreview.transform.localScale += Vector3.one * 0.05f;
                if (value < 0) bluePreview.transform.localScale -= Vector3.one * 0.05f;
            }
        }

        private void OnRightClick(Dictionary<string, object> message)
        {
            _previewOrangePortal = (bool) message["value"];

            var preview = PortalManager.Instance.OrangePortal.Preview;
            var portal = PortalManager.Instance.OrangePortal.Portal;

            if (_previewOrangePortal)
            {
                _previewBluePortal = false;
                preview.transform.localScale = _originalOrangePortalLocalScale;
            }
            else if (preview.activeSelf)
            {
                portal.transform.position = preview.transform.position;
                portal.transform.rotation = preview.transform.rotation;
                portal.transform.localScale = Vector3.one * 0.01f;
                portal.transform.DOScale(preview.transform.localScale, 0.3f);
            }
        }

        private void OnLeftClick(Dictionary<string, object> message)
        {
            _previewBluePortal = (bool) message["value"];

            var preview = PortalManager.Instance.BluePortal.Preview;
            var portal = PortalManager.Instance.BluePortal.Portal;

            if (_previewBluePortal)
            {
                _previewOrangePortal = false;
                preview.transform.localScale = _originalBluePortalLocalScale;
            }
            else if (preview.activeSelf)
            {
                portal.transform.position = preview.transform.position;
                portal.transform.rotation = preview.transform.rotation;
                portal.transform.localScale = Vector3.one * 0.01f;
                portal.transform.DOScale(preview.transform.localScale, 0.3f);
            }
        }

        private static void PlacePreview(GameObject portalPreview, PortalBounds bounds)
        {
            var cameraTransform = Camera.main.transform;
            var cameraPosition = cameraTransform.position;
            var ray = new Ray(cameraPosition, cameraTransform.forward);

            if (!PortalUtilities.RayCastPortalWall(ray, out var hit))
            {
                portalPreview.SetActive(false);
                return;
            }

            portalPreview.transform.position = hit.point;
            portalPreview.transform.forward = hit.normal;

            portalPreview.SetActive(bounds.IsOnValidSurface());
        }
    }
}