using UnityEngine;
using Utilities.Singleton;

namespace Portal
{
    public class PortalManager : SingletonMonoBehaviour<PortalManager>
    {
        private PortalData? _bluePortal, _orangePortal;
        
        public PortalData BluePortal
        {
            get
            {
                if (_bluePortal != null) return (PortalData) _bluePortal;
                
                var portal = GameObject.Find("Blue Portal");
                var preview = GameObject.Find("Blue Portal Preview");
                var bounds = preview.GetComponentInChildren<PortalBounds>();
                
                _bluePortal = new PortalData()
                {
                    Portal = portal,
                    Preview = preview,
                    Bounds = bounds
                };
                
                return (PortalData) _bluePortal;
            }
        }
        
        public PortalData OrangePortal
        {
            get
            {
                if (_orangePortal != null) return (PortalData) _orangePortal;
                
                var portal = GameObject.Find("Orange Portal");
                var preview = GameObject.Find("Orange Portal Preview");
                var bounds = preview.GetComponentInChildren<PortalBounds>();
                
                _orangePortal = new PortalData()
                {
                    Portal = portal,
                    Preview = preview,
                    Bounds = bounds
                };
                
                return (PortalData) _orangePortal;
            }
        }

        public struct PortalData
        {
            public GameObject Portal;
            public GameObject Preview;
            public PortalBounds Bounds;
        }
    }
}
