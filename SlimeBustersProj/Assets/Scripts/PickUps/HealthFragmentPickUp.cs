using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthFragmentPickUp : MonoBehaviour
{
    private Rigidbody rb;

   

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        Vector3 launchDir = Vector3.up;
        Vector3 rotationVector = Vector3.right;
        rotationVector = Quaternion.AngleAxis(UnityEngine.Random.Range(0, 360), Vector3.up) * rotationVector;
        launchDir = Quaternion.AngleAxis(UnityEngine.Random.Range(0, 45), rotationVector) * launchDir;
        launchDir.Normalize();


        rb.AddForce(launchDir * 10f, ForceMode.VelocityChange);
    }

    // Update is called once per frame
    void Update()
    {

        transform.Rotate(transform.up, 1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().AddHealthFragment(1);
            AkSoundEngine.PostEvent("Play_FragmentPickUp", gameObject);
            Destroy(this.gameObject);
        }
    }
}
