using Portal;
using UnityEngine;

namespace GravityGun
{
    public class GravityGunUtilities : MonoBehaviour
    {
        public static bool RayCastGravityObject(Ray ray, out RaycastHit hit)
        {
            return Physics.Raycast(ray, out hit, GameSettings.Instance.gravityGunGrabbingDistance) && PortalUtilities.MatchLayers(hit.collider.gameObject.layer,
                GameSettings.Instance.entitiesAffectedByGravityGun);
        }

        public static void ShootObject(Rigidbody rigidbody)
        {
            rigidbody.isKinematic = false;
            rigidbody.AddForce(Camera.main.transform.forward * GameSettings.Instance.gravityGunShootingForce);
        }
    }
}