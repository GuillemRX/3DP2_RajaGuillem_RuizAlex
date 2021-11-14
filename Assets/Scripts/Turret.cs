using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField]
    LineRenderer lineRenderer;

    [SerializeField]
    LayerMask collisionLayerMask;

    [SerializeField]
    float maxDistance;

    Vector3 endRaycastPosition;

    [SerializeField]
    Transform laserOutput;

    bool isBeamWorking;

    // Start is called before the first frame update
    void Start()
    {
        isBeamWorking = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isBeamWorking)
        {
            RaycastHit raycastHit;
            Physics.Raycast(new Ray(laserOutput.transform.position, laserOutput.transform.TransformDirection(Vector3.forward)),
            out raycastHit, maxDistance, collisionLayerMask.value);

            if (raycastHit.transform != null)
            {
                endRaycastPosition = raycastHit.point;
                handleBeamDamage(raycastHit.transform);
            }
            else
            {
                endRaycastPosition = laserOutput.transform.TransformDirection(Vector3.forward) * maxDistance;
            }


            lineRenderer.SetPosition(0, laserOutput.transform.position);
            lineRenderer.SetPosition(1, endRaycastPosition);
        }else{
            lineRenderer.enabled = false;
        }



    }

    void handleBeamDamage(Transform target)
    {
        if (target.gameObject.tag == "Player")
        {
            target.gameObject.GetComponent<SimpleHealthSystem>().Kill();
        }

        if (target.gameObject.tag == "Turret")
        {
            target.gameObject.SetActive(false);
        }
    }

    void OnCollisionEnter(Collision other)
    {

        if (other.gameObject.tag == "Companion" || other.gameObject.tag == "Turret")
        {
            isBeamWorking = false;
        }

    }
}
