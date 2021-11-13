using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Portal
{
    public class PortalRenderer : MonoBehaviour
    {
        [SerializeField] private Camera portalCamera;
        [SerializeField] private float clipPlaneOffset = 0.05f;

        public RenderTexture RenderTexture { get; private set; }
        public Camera PortalCamera => portalCamera;

        private void Awake()
        {
            RenderTexture = new RenderTexture(Screen.width, Screen.height, 32);
            PortalCamera.targetTexture = RenderTexture;
            PortalCamera.enabled = false;
        }

        public void Render(ScriptableRenderContext src)
        {
            PortalCamera.projectionMatrix = GetNewProjectionMatrix();
            UniversalRenderPipeline.RenderSingleCamera(src, PortalCamera);
        }

        private Matrix4x4 GetNewProjectionMatrix()
        {
            var t = transform;
            var forward = t.forward;
            var offset = forward * clipPlaneOffset;
            var portalPlane = new Plane(forward, t.position + offset);
            var portalCameraTransform = PortalCamera.transform;
            var clipPlaneDistance = portalCameraTransform.forward * PortalCamera.nearClipPlane;
            var clipPlanePosition = portalCameraTransform.position + clipPlaneDistance;

            if (portalPlane.GetDistanceToPoint(clipPlanePosition) >= 0)
            {
                portalPlane.SetNormalAndPosition(forward, clipPlanePosition + offset);
            }
            
            var clipPlane = new Vector4(
                portalPlane.normal.x,
                portalPlane.normal.y,
                portalPlane.normal.z,
                portalPlane.distance);
            var clipPlaneCameraSpace =
                Matrix4x4.Transpose(Matrix4x4.Inverse(PortalCamera.worldToCameraMatrix)) * clipPlane;

            return PortalCamera.CalculateObliqueMatrix(clipPlaneCameraSpace);
        }
    }
}