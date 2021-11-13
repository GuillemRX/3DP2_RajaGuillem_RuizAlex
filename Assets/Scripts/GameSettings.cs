using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Singleton;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Scriptable Objects/Game Settings")]
public class GameSettings : SingletonScriptableObject<GameSettings>
{
    public float gravityGunGrabbingDistance;
    public float gravityGunCarryingDistance;
    public float gravityGunCarryingSmoothing;
    public float gravityGunShootingForce;
    public LayerMask portalSurface;
    public LayerMask surfacesToPlacePortals;
    public LayerMask player;
    public LayerMask entitiesAffectedByGravityGun;
}
