using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameObject camera;

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
    private int currenthealth = 3;
    public int CurrentHealth => currenthealth;
    [SerializeField]
    private int maxHealth = 3;
    public int MaxHealth => maxHealth;

    bool canBeDamaged = true;
    float InvincibleTime = 3;
    bool beingLaunched = false;


    [SerializeField]
    VacuumCleaner cleaner;


    [Header("Hud Connections")]
    [SerializeField]
    private HUD_HealthDisplay healthDisplay;
    [SerializeField]
    private HUD_RemainingSlimesDisplay remainingSlimesDisplay;


    private void Awake()
    {
        currenthealth = maxHealth;
        canBeDamaged = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();

        

        healthDisplay.UpdateHealthDisplay(currenthealth, maxHealth);
        remainingSlimesDisplay.UpdateSlimesDisplay(99);

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
            attack = Input.GetButton("Fire1");
        }
        if (!interact)
        {
            interact = Input.GetButtonDown("Interact");
        }
    }

    private void FixedUpdate()
    {
        if (!beingLaunched)
        { 
        input = Vector2.ClampMagnitude(input, 1);
        float dirY = myRigidbody.velocity.y;
        direction = camera.transform.right * input.x + camera.transform.forward * input.y;
        direction *= currentMovementSpeed;
        direction.y = dirY;

        myRigidbody.velocity = direction;

        if (!attack)
        {
            //make player look where they go        
            Vector3 lookDir = new Vector3(direction.x, 0, direction.z);
            if (lookDir != Vector3.zero)
            {
                Quaternion rotation = Quaternion.LookRotation(lookDir, transform.up);
                transform.rotation = rotation;
            }


            if (cleaner.Active) cleaner.Deactivate();
        }
        else
        {
            //make player look at point where mouse is clicking
            Vector3 lookDir = Vector3.zero;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit))
            {
                lookDir = raycastHit.point - transform.position;
            }

            lookDir = new Vector3(lookDir.x, 0, lookDir.z);
            Quaternion rotation = Quaternion.LookRotation(lookDir, transform.up);
            transform.rotation = rotation;


            if (!cleaner.Active) cleaner.Activate();
        }
    }
        else
        {

        }

        ResetInput();
    }

    private void ResetInput()
    {
        jump = false;
        attack = false;
        interact = false;
    }

    public void GetDamage(int damage, Vector3 enemyPosition)
    {
        if (canBeDamaged)
        {
            canBeDamaged = false;
            currenthealth -= damage;
            healthDisplay.UpdateHealthDisplay(currenthealth, maxHealth);

            if (currenthealth > 0)
            {
                StartCoroutine(RegainVulnerability());
            }
            else
            {
                Die();
            }
        }

        Vector3 launchDirection = transform.position - enemyPosition;
        launchDirection.Normalize();

        myRigidbody.AddForce(launchDirection * 10, ForceMode.VelocityChange);
        beingLaunched = true;
        StartCoroutine(RegainMotion());
    }

    private IEnumerator RegainVulnerability()
    {
        yield return new WaitForSeconds(InvincibleTime);

        canBeDamaged = true;        
    }

    private IEnumerator RegainMotion()
    {
        yield return new WaitForSeconds(1);

        beingLaunched = false;
    }

    private void Die()
    {
        Debug.Log("Player Died");
    }

}
