using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Transform target;

    private float smoothSpeed = 0.125f;
    [SerializeField]
    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = target.position + new Vector3(8, 10, -7);
        transform.rotation = Quaternion.identity;
        transform.rotation = Quaternion.Euler(45, -45, 0);

        offset = transform.position - target.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    private void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            transform.position = smoothedPosition;
        }


    }
}
