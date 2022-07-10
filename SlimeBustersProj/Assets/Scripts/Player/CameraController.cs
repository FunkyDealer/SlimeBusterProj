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

    [SerializeField]
    LayerMask propMask;
    [SerializeField]
    Material transperancyMat;

    Dictionary<int, List<Material[]>> savedMats;

    private void Awake()
    {
        savedMats = new Dictionary<int, List<Material[]>>();

        transform.position = target.position + new Vector3(8, 10, -7);
        transform.rotation = Quaternion.identity;
        transform.rotation = Quaternion.Euler(45, -45, 0);

        offset = transform.position - target.position;
    }

    // Start is called before the first frame update
    void Start()
    {
       
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

    private void OnTriggerEnter(Collider other)
    {
        if (propMask.Includes(other.gameObject.layer))
        {

            MakePropTransparent(other);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (propMask.Includes(other.gameObject.layer))
        {

            MakePropVisible(other);

        }
    }

    //make a prop that is blocking the player visible
    private void MakePropTransparent(Collider other)
    {
        //Get to the top of the hierarchy of the object
        GameObject top = other.gameObject;
        while (top.transform.parent.name != "Props")
        {
            top = top.transform.parent.gameObject;
        }

        int id = top.GetInstanceID();

        //Debug.Log($"Entered - object name: {top.name}");          
        //Get all the renderers in the object
        Renderer[] renderers = top.GetComponentsInChildren<Renderer>();

        // lets save  all the materials per renderer list (A list where each element is an array of materials of one of the renderers)
        List<Material[]> materialPerRendererList = new List<Material[]>();
        int totalMaterials = 0;
        foreach (var r in renderers)
        {
            materialPerRendererList.Add(r.materials);
            totalMaterials = totalMaterials + r.materials.Length;
        }

        // make each material in each renderer Transparent
        //Debug.Log($"ammount of renderers in object: {renderers.Length}");
        // Debug.Log($"ammount of Materials in object: {totalMaterials}");
        for (int i = 0; i < renderers.Length; i++)
        {
            Material[] materials = renderers[i].materials; //this return a copy and not the actual array

            for (int e = 0; e < materials.Length; e++)
            {
                materials[e] = transperancyMat; //change the copy array
            }

            renderers[i].materials = materials; //make the original array into the copy one

        }

        //place renderers and materials per renderer in the dictionary
        if (!savedMats.ContainsKey(id)) savedMats.Add(id, materialPerRendererList);
    }

    //Make a prop visible again
    private void MakePropVisible(Collider other) 
    {
        //Get to the top of the hierarchy of the object
        GameObject top = other.gameObject;
        while (top.transform.parent.name != "Props")
        {
            top = top.transform.parent.gameObject;
        }

        int id = top.GetInstanceID(); //get the Id of the object that's blocking

        //Debug.Log($"Entered - object name: {top.name}");            
        //Get all the renderers in the object
        Renderer[] renderers = top.GetComponentsInChildren<Renderer>();

        if (savedMats.ContainsKey(id))
        {
            //Debug.Log($"Exiting Prop: {top.name}");
            for (int i = 0; i < renderers.Length; i++)
            {
                Material[] materials = renderers[i].materials; //this return a copy and not the actual array

                for (int e = 0; e < materials.Length; e++)
                {
                    materials[e] = savedMats[id][i][e]; //change the copy array
                }

                renderers[i].materials = materials; //make the original array into the copy one
            }

            savedMats.Remove(id); //remove the object from the savedMats dictionary as we don't need it anymore
        }
    }


    //private void CheckIfPlayerBlocked()
    //{
    //    Ray ray = new Ray(transform.position, target.position - transform.position);
    //    Debug.DrawRay(transform.position, target.position - transform.position);
    //    if (Physics.Raycast(ray, out RaycastHit raycastHit, Vector3.Distance(transform.position, target.position), PropMask)) {

    //        Renderer renderer = raycastHit.collider.gameObject.GetComponent<Renderer>();
    //        if (renderer != null)
    //        {
    //            //blockingObjs.Add(raycastHit.collider.gameObject);
    //        }

    //        Material mat = renderer.material;

    //        renderer.material = transperancyMat;

    //        StartCoroutine(SetColorBack(renderer, mat));
    //    }
    //}

    //private IEnumerator SetColorBack(Renderer renderer ,Material originalMat)
    //{
    //    yield return new WaitForSeconds(0.1f);


    //    renderer.material = originalMat;
    //}



    

    
    
}
