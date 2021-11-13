using System.Collections.Generic;
using UnityEngine;

namespace Portal
{
    public class PortalUtilities : MonoBehaviour
    {
        public static void Translate(Transform from, Transform to, Transform reference, Transform affected)
        {
            from.Rotate(0, 180, 0);

            var referenceParent = reference.parent;

            reference.parent = from;

            var position = reference.localPosition;
            var rotation = reference.localRotation;
            var localScale = reference.localScale;

            reference.parent = referenceParent;

            var affectedParent = affected.parent;

            affected.parent = to;

            affected.localPosition = position;
            affected.localRotation = rotation;
            affected.localScale = localScale;

            affected.parent = affectedParent;

            from.Rotate(0, -180, 0);
        }

        public static Vector3 GetTranslatedDirection(Transform from, Transform to, Vector3 direction)
        {
            from.Rotate(0, 180, 0);
            
            var relativeDirection = from.InverseTransformDirection(direction);
            var translatedDirection = to.TransformDirection(relativeDirection);
            
            from.Rotate(0, -180, 0);

            return translatedDirection;
        }
        
        public static bool MatchLayers(int layer, LayerMask layerMask)
        {
            return ((1 << layer) & layerMask) != 0;
        }

        public static bool RayCastPortalWall(Ray ray, out RaycastHit hit)
        {
            return Physics.Raycast(ray, out hit) && MatchLayers(hit.collider.gameObject.layer,
                GameSettings.Instance.surfacesToPlacePortals);
        }
    }
}