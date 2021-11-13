using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities;

namespace Portal
{
    public class PortalSurface : MonoBehaviour
    {
        [SerializeField] private LayerMask disableCollisions;
        [SerializeField] private LayerMask detectCollisions;
        [SerializeField] private Transform portalTransform;

        private Collider _collider;
        private List<Collider> _toDisable, _toDetect;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _toDisable = new List<Collider>();
            _toDetect = new List<Collider>();
        }

        private void FixedUpdate()
        {
            var bounds = _collider.bounds;
            var toDisable = Physics.OverlapBox(bounds.center, bounds.extents, transform.rotation, disableCollisions)
                .ToList();
            var toDetect = Physics.OverlapBox(bounds.center, bounds.extents, transform.rotation, detectCollisions)
                .ToList();

            foreach (var coll1 in toDetect)
            foreach (var coll2 in toDisable)
                Physics.IgnoreCollision(coll1, coll2, true);

            foreach (var coll1 in _toDetect)
            foreach (var coll2 in _toDisable.Where(coll2 => !toDetect.Contains(coll1) || !toDisable.Contains(coll2)))
                Physics.IgnoreCollision(coll1, coll2, false);

            _toDetect = toDetect;
            _toDisable = toDisable;

            var portalPlane = new Plane(portalTransform.forward, portalTransform.position);

            foreach (
                var coll in from coll in _toDetect
                let t = coll.transform
                where portalPlane.GetDistanceToPoint(t.position) < 0
                select coll)
                EventManager.TriggerEvent(Events.Instance.onTeleportObject, new Dictionary<string, object>
                {
                    {"from", portalTransform},
                    {"object", coll.gameObject}
                });
        }
    }
}