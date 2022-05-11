using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameObject Camera;

    [Header("Movement")]
    [SerializeField]
    private float maxMovementSpeed = 5;
    private float currentMovementSpeed = 10;
    [SerializeField, Min(0.01f)]
    private float movSmoothLerp = 0.03f;

    private Vector3 direction;
    private Rigidbody myRigidbody;

    private Vector2 input;
    private bool jump;
    private bool interact;
    private bool attack;

    [Header("Player Stuff")]

    private bool PlayerID;

    //Player stuff
    private bool isAlive = true;
    //health
    private float currenthealth = 100;
    public float CurrentHealth => currenthealth;
    [SerializeField]
    private int maxHealth = 100;
    public int MaxHealth => maxHealth;
   




    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();


    }

    // Update is called once per frame
    void Update()
    {
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (!jump)
        {
            jump = Input.GetButtonDown("Jump");
        }
        if (!attack)
        {
            attack = Input.GetButtonDown("Fire1");
        }
        if (!interact)
        {
            interact = Input.GetButtonDown("Interact");
        }
    }

    private void FixedUpdate()
    {
        input = Vector2.ClampMagnitude(input, 1);
        float dirY = myRigidbody.velocity.y;
        direction = Camera.transform.right * input.x + Camera.transform.forward * input.y;       
        direction *= currentMovementSpeed;
        direction.y = dirY;

        myRigidbody.velocity = direction;

        //make player look where they go
        Vector3 lookDir = new Vector3(direction.x, 0, direction.z);
        Quaternion rotation = Quaternion.LookRotation(lookDir, transform.up);
        transform.rotation = rotation;

        //make player look at mouse
        Vector2 mousePos = Input.mousePosition;



        ResetInput();
    }

    private void ResetInput()
    {
        jump = false;
        attack = false;
        interact = false;
    }

}
