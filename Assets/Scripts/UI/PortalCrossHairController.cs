using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace UI
{
    public class PortalCrossHairController : MonoBehaviour
    {
        [SerializeField] private Sprite blueCrossHair;
        [SerializeField] private Sprite orangeCrossHair;

        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        private void OnEnable()
        {
            EventManager.StartListening(Events.Instance.playerActions.onPlaceBluePortal, OnPlaceBluePortal);
            EventManager.StartListening(Events.Instance.playerActions.onPlaceOrangePortal, OnPlaceOrangePortal);
        }

        private void OnPlaceOrangePortal(Dictionary<string, object> obj)
        {
            _image.sprite = orangeCrossHair;
        }

        private void OnPlaceBluePortal(Dictionary<string, object> obj)
        {
            _image.sprite = blueCrossHair;
        }
    }
}