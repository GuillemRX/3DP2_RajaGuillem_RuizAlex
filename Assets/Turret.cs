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
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit raycastHit;
        Physics.Raycast(new Ray(laserOutput.transform.position, laserOutput.transform.forward),
            out raycastHit, maxDistance, collisionLayerMask.value);

        if(raycastHit.transform !=null){
            endRaycastPosition = raycastHit.transform.position;
        }else{
            endRaycastPosition= laserOutput.transform.TransformDirection(Vector3.forward) * maxDistance;
        }
        

        lineRenderer.SetPosition(0,laserOutput.transform.position);
        lineRenderer.SetPosition(1,endRaycastPosition);
    
    }
}
