using UnityEngine;
using UnityEngine.Rendering;

namespace Portal
{
    public class PortalRenderingManager : MonoBehaviour
    {
        [SerializeField] private int portalRenderingIterations = 5;
        
        private PortalData _bluePortal, _orangePortal;
        private Camera _playerCamera;

        private void Start()
        {
            _playerCamera = Camera.main;

            var bluePortal = GameObject.Find("Blue Portal");

            _bluePortal = new PortalData
            {
                Transform = bluePortal.transform,
                PortalRenderer = bluePortal.GetComponentInChildren<PortalRenderer>(),
                MeshRenderer = bluePortal.GetComponentInChildren<MeshRenderer>()
            };

            var orangePortal = GameObject.Find("Orange Portal");

            _orangePortal = new PortalData
            {
                Transform = orangePortal.transform,
                PortalRenderer = orangePortal.GetComponentInChildren<PortalRenderer>(),
                MeshRenderer = orangePortal.GetComponentInChildren<MeshRenderer>()
            };

            _bluePortal.MeshRenderer.material.mainTexture = _orangePortal.PortalRenderer.RenderTexture;
            _orangePortal.MeshRenderer.material.mainTexture = _bluePortal.PortalRenderer.RenderTexture;
        }

        private void OnEnable()
        {
            RenderPipelineManager.beginCameraRendering += RenderPortals;
        }

        private void OnDisable()
        {
            RenderPipelineManager.beginCameraRendering -= RenderPortals;
        }

        private void RenderPortals(ScriptableRenderContext src, Camera cam)
        {
            if (cam != _playerCamera) return;

            _orangePortal.MeshRenderer.enabled = false;
            _bluePortal.MeshRenderer.enabled = false;

            PortalUtilities.Translate(
                _bluePortal.Transform,
                _orangePortal.Transform,
                _playerCamera.transform,
                _orangePortal.PortalRenderer.PortalCamera.transform);
            
            _orangePortal.PortalRenderer.Render(src);
            
            PortalUtilities.Translate(
                _orangePortal.Transform,
                _bluePortal.Transform,
                _playerCamera.transform,
                _bluePortal.PortalRenderer.PortalCamera.transform);
            
            _bluePortal.PortalRenderer.Render(src);
            
            _orangePortal.MeshRenderer.enabled = true;
            _bluePortal.MeshRenderer.enabled = true;
        }

        private bool IsVisible(Renderer portalMeshRenderer)
        {
            var frustumPlanes = GeometryUtility.CalculateFrustumPlanes(_playerCamera);
            return GeometryUtility.TestPlanesAABB(frustumPlanes, portalMeshRenderer.bounds);
        }

        private struct PortalData
        {
            public Transform Transform;
            public PortalRenderer PortalRenderer;
            public MeshRenderer MeshRenderer;
        }
    }
}