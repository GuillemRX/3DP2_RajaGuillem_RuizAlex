using System.Collections;
using System.Collections.Generic;
using Audio;
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
    public LayerMask playerGround;
    public Sound bluePortalSFX;
    public Sound orangePortalSFX;
    public Sound travelPortalSFX;
    public Sound gravityGunGrabSFX;
    public Sound gravityGunDropSFX;
    public Sound gravityGunThrowSFX;
    public Sound buttonPressedSFX;
    public Sound buttonReleasedSFX;
    public Sound[] collisionSFX;
}
