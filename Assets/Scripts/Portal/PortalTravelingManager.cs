using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Portal
{
    public class PortalTravelingManager : ExtendedMonoBehaviour
    {
        private Transform _bluePortal, _orangePortal;

        private void Start()
        {
            _bluePortal = GameObject.Find("Blue Portal").transform;
            _orangePortal = GameObject.Find("Orange Portal").transform;
        }

        private void OnEnable()
        {
            EventManager.StartListening(Events.Instance.onTeleportObject, OnTeleportObject);
        }

        private void OnDisable()
        {
            EventManager.StopListening(Events.Instance.onTeleportObject, OnTeleportObject);
        }

        private void OnTeleportObject(Dictionary<string, object> message)
        {
            var from = (Transform) message["from"];
            var obj = (GameObject) message["object"];

            var rigidBody = obj.GetComponentInChildren<Rigidbody>();

            Coroutine().WaitForEndOfFrame().Invoke(() =>
            {
                if (_bluePortal.Equals(from))
                {
                    PortalUtilities.Translate(_bluePortal, _orangePortal, obj.transform, obj.transform);
                    rigidBody.velocity =
                        PortalUtilities.GetTranslatedDirection(_bluePortal, _orangePortal, rigidBody.velocity);
                }
                else
                {
                    PortalUtilities.Translate(_orangePortal, _bluePortal, obj.transform, obj.transform);
                    rigidBody.velocity =
                        PortalUtilities.GetTranslatedDirection(_orangePortal, _bluePortal, rigidBody.velocity);
                }
            }).Run();
        }
        
        
    }
}