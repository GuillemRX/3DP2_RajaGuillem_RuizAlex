using System.Linq;
using UnityEngine;

namespace Portal
{
    public class PortalBounds : MonoBehaviour
    {
        private Transform _cameraTransform;
        private Transform[] _points;

        private void Awake()
        {
            _points = transform.Cast<Transform>().ToArray();
            _cameraTransform = Camera.main.transform;
        }

        public bool IsOnValidSurface()
        {
            var cameraPosition = _cameraTransform.position;

            foreach (var point in _points)
            {
                var direction = (point.position - cameraPosition).normalized;
                var pointRay = new Ray(cameraPosition, direction);
                if (!PortalUtilities.RayCastPortalWall(pointRay, out var pointHit)) return false;
                if (pointHit.normal != transform.forward) return false;
                if (Vector3.Distance(transform.position, pointHit.point) > GetMaxDistanceToCenter() + 0.01f) return false;
            }

            return true;
        }

        private float GetMaxDistanceToCenter()
        {
            return _points.Max(point => Vector3.Distance(transform.position, point.position));
        }
    }
}