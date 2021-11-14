using System.Collections.Generic;
using UnityEngine;
using Utilities;
using Utilities.Pooling;
using Utilities.Singleton;

namespace Audio
{
    public class AudioManager : SingletonMonoBehaviour<AudioManager>
    {
        private IObjectPool<AudioSource> audioSources;

        private void Awake()
        {
            audioSources =
                new QueuePool<AudioSource>(() => new GameObject().AddComponent<AudioSource>(), null, null, 20);
        }

        private void OnEnable()
        {
            EventManager.StartListening(Events.Instance.playerActions.onPlaceBluePortal, OnPlaceBluePortal);
            EventManager.StartListening(Events.Instance.playerActions.onPlaceOrangePortal, OnPlaceOrangePortal);
            EventManager.StartListening(Events.Instance.playerActions.onGravityGunGrab, OnGravityGunGrab);
            EventManager.StartListening(Events.Instance.playerActions.onGravityGunDrop, OnGravityGunDrop);
            EventManager.StartListening(Events.Instance.playerActions.onGravityGunThrow, OnGravityGunThrow);
            EventManager.StartListening(Events.Instance.onTeleportObject, OnTeleportObject);
            EventManager.StartListening(Events.Instance.onObjectCollision, OnObjectCollision);
            EventManager.StartListening(Events.Instance.onButtonPressed, OnButtonPressed);
            EventManager.StartListening(Events.Instance.onButtonReleased, OnButtonReleased);
        }

        private void OnButtonReleased(Dictionary<string, object> message)
        {
            var source = (GameObject) message["source"];

            Play(GameSettings.Instance.buttonReleasedSFX, source.gameObject);
        }

        private void OnButtonPressed(Dictionary<string, object> message)
        {
            var source = (GameObject) message["source"];

            Play(GameSettings.Instance.buttonPressedSFX, source.gameObject);
        }

        private void OnGravityGunThrow(Dictionary<string, object> message)
        {
            var source = (GameObject) message["source"];

            Play(GameSettings.Instance.gravityGunThrowSFX, source.gameObject);
        }

        private void OnGravityGunDrop(Dictionary<string, object> message)
        {
            var source = (GameObject) message["source"];

            Play(GameSettings.Instance.gravityGunDropSFX, source.gameObject);
        }

        private void OnGravityGunGrab(Dictionary<string, object> message)
        {
            var source = (GameObject) message["source"];

            Play(GameSettings.Instance.gravityGunGrabSFX, source.gameObject);
        }

        private void OnObjectCollision(Dictionary<string, object> message)
        {
            var source = (GameObject) message["source"];
            var collisions = GameSettings.Instance.collisionSFX;

            Play(collisions[Random.Range(0, collisions.Length)], source);
        }

        private void OnTeleportObject(Dictionary<string, object> message)
        {
            var source = (Transform) message["from"];

            Play(GameSettings.Instance.travelPortalSFX, source.gameObject);
        }

        private void OnPlaceOrangePortal(Dictionary<string, object> message)
        {
            var source = (GameObject) message["source"];

            Play(GameSettings.Instance.orangePortalSFX, source);
        }

        private void OnPlaceBluePortal(Dictionary<string, object> message)
        {
            var source = (GameObject) message["source"];

            Play(GameSettings.Instance.bluePortalSFX, source);
        }

        private void Play(Sound sound, GameObject source)
        {
            var audioSource = audioSources.Get();

            audioSource.volume = sound.volume;
            audioSource.loop = sound.loop;
            audioSource.clip = sound.audioClip;
            audioSource.transform.position = source.transform.position;

            audioSource.Play();
            audioSources.Return(audioSource);
        }
    }
}