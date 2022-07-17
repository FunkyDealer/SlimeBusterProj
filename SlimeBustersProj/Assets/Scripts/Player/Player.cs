using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameObject myCamera;
    [SerializeField]
    GameObject FreeCamera;

    [Header("Movement")]
    [SerializeField]
    private float maxMovementSpeed = 5;
    private float currentMovementSpeed = 10;
    [SerializeField, Min(0.01f)]
    private float movSmoothLerp = 0.03f;
    [SerializeField]
    private float jumpForce = 5f;
    private bool jumping = false; //is the playing curenttly in a jump?

    private bool beingLaunched = false;
    private bool isGrounded = false;

    private Vector3 direction;
    private Rigidbody myRigidbody;
    private Collider myCollider;
    private AkAudioListener myAudioListener;
    private Animator myAnimator;

    private Vector2 movementInput = Vector2.zero;
    private Vector2 lookInput = Vector2.zero;
    private Vector3 lookDir = Vector3.zero;
    private bool jump;
    private bool interact;
    private bool attack;
    private bool frozen = false;

    [SerializeField]
    private LayerMask mousePointLayerMask;

    [Header("Player Stuff")]
    //Player stuff
    private bool isAlive = true;
    //health
    private int currenthealth = 3;
    public int CurrentHealth => currenthealth;
    [SerializeField]
    private int defaultHealth = 3;

    private int currentHealthFragments = 0;
    [SerializeField]
    private int neededHealthFragments = 4;

    bool canBeDamaged = true;
    float InvincibleTime = 3;

    [SerializeField]
    private VacuumCleaner cleaner;
    public VacuumCleaner Cleaner => cleaner;

    [Header("Hud Connections")]
    [SerializeField]
    private HUD_HealthDisplay healthDisplay;
    [SerializeField]
    private HUD_RemainingSlimesDisplay remainingSlimesDisplay;
    [SerializeField]
    private HUD_FragmentDisplay fragmentDisplay;

    private float distanceGround;
    [SerializeField]
    private LayerMask groundMask;


    private void Awake()
    {
        currenthealth = defaultHealth;
        canBeDamaged = true;

        myRigidbody = GetComponent<Rigidbody>();
        myCollider = GetComponent<Collider>();
        myAudioListener = GetComponent<AkAudioListener>();
        myAnimator = GetComponent<Animator>();
        distanceGround = myCollider.bounds.extents.y;

    }

    // Start is called before the first frame update
    void Start()
    {
               

        healthDisplay.UpdateHealthDisplay(currenthealth);
        fragmentDisplay.UpdateFragDisplay(currentHealthFragments);

        isGrounded = CheckIsGround();
    }

    // Update is called once per frame
    void Update()
    {
        if (!frozen)
        {
            if (isAlive)
            {
                movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                lookInput = new Vector2(Input.GetAxis("Right Horizontal"), Input.GetAxis("Right Vertical"));

                if (!jump && !jumping && isGrounded)
                {
                    jump = Input.GetButtonDown("Jump");
                }
                if (!attack)
                {
                    attack = Input.GetButton("Fire1");
                    if (lookInput.magnitude > 0.1f) attack = true;
                }
                if (!interact)
                {
                    interact = Input.GetButtonDown("Interact");
                }
                if (Input.GetKey(KeyCode.P))
                {
                    this.gameObject.SetActive(false);
                    myCamera.gameObject.SetActive(false);
                    FreeCamera.gameObject.SetActive(true);
                }

            }
            else
            {
                ResetInput();
                direction = new Vector3(0, direction.y, 0);
            }
        }
    }

    internal void returnToPlayer()
    {
        this.gameObject.SetActive(true);
        myCamera.gameObject.SetActive(true);
    }

    private void FixedUpdate()
    {
        isGrounded = CheckIsGround();
        if (isGrounded)
        {
            // if (jumping) AkSoundEngine.PostEvent(3297008698, gameObject); //Play_Jump_Land event
            Debug.DrawLine(transform.position + Vector3.up * 0.5f, transform.position - Vector3.up * .8f, Color.green);
            jumping = false;
        }
        else
        {
            Debug.DrawLine(transform.position + Vector3.up * 0.5f, transform.position - Vector3.up * .8f, Color.red);
        }

        if (!beingLaunched)
        { 
        movementInput = Vector2.ClampMagnitude(movementInput, 1);
        float dirY = myRigidbody.velocity.y;
        direction = myCamera.transform.right * movementInput.x + myCamera.transform.forward * movementInput.y;
        direction *= currentMovementSpeed;
        direction.y = dirY;

        myRigidbody.velocity = direction;

            if (jump) {
                myRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
                AkSoundEngine.PostEvent("Play_Jump", gameObject);
                jump = false;
                jumping = true;
            }
    
        if (!attack)
        {
            //make player look where they go        
            lookDir = new Vector3(direction.x, 0, direction.z);
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
            lookDir = Vector3.zero;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 500, mousePointLayerMask, QueryTriggerInteraction.Ignore))
            {
                lookDir = raycastHit.point - transform.position;                  
            }

            lookDir = new Vector3(lookDir.x, 0, lookDir.z);
            Quaternion rotation = Quaternion.LookRotation(lookDir, transform.up);
            transform.rotation = rotation;       
                
            if (lookInput.magnitude > 0) //Controller
                {                    
                    lookInput = Vector2.ClampMagnitude(lookInput, 1);
                    Vector3 lookDirection = myCamera.transform.right * lookInput.x + myCamera.transform.forward * lookInput.y;

                    lookDir = new Vector3(lookDirection.x, 0, lookDirection.z);
                    if (lookDir != Vector3.zero)
                    {
                        rotation = Quaternion.LookRotation(lookDir, transform.up);
                        transform.rotation = rotation;
                    }
                }          

            if (!cleaner.Active) cleaner.Activate();
        }
    }
        else
        {

        }

        PlayAnimations(movementInput);

        ResetInput();
    }

    private bool CheckIsGround()
    {
        
        return (Physics.Raycast(transform.position + Vector3.up * 0.5f, -Vector3.up, 0.8f, groundMask));
    }

    private void ResetInput()
    {
        jump = false;
        attack = false;
        interact = false;
        lookInput = Vector3.zero;  
    }

    private void PlayAnimations(Vector2 movementInput)
    {
        if (!attack)
        {
            if (movementInput.magnitude > 0)
            {
                myAnimator.SetBool("WalkingForward", true);
                myAnimator.SetBool("WalkingBackWards", false);
            }
            else
            {
                myAnimator.SetBool("WalkingForward", false);
                myAnimator.SetBool("WalkingBackWards", false);

            }
        }
        else
        {
            if (movementInput.magnitude > 0)
            {
                float angle = Vector3.Angle(direction, lookDir);
                if (angle > 90)
                {
                    myAnimator.SetBool("WalkingForward", false);
                    myAnimator.SetBool("WalkingBackWards", true);
                }
                else
                {
                    myAnimator.SetBool("WalkingForward", true);
                    myAnimator.SetBool("WalkingBackWards", false);
                }
            }
            else
            {
                myAnimator.SetBool("WalkingForward", false);
                myAnimator.SetBool("WalkingBackWards", false);
            }
        }


    }

    public void GetDamage(int damage, Vector3 enemyPosition)
    {
        if (canBeDamaged)
        {
            AkSoundEngine.PostEvent("Play_Slime_Impacts", gameObject);
            StartCoroutine(PlayPlayerHurtSound());

            canBeDamaged = false;
            currenthealth -= damage;
            healthDisplay.UpdateHealthDisplay(currenthealth);

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

    private IEnumerator PlayPlayerHurtSound()
    {
        yield return new WaitForSeconds(0.2f);
        AkSoundEngine.PostEvent("Play_Player_Hurt_Voice", gameObject);
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
        StartCoroutine(GameManager.RestartLevel());
        //Debug.Log("Player Died");
        isAlive = false;
        
    }

    public void GetRemainingSlimes(int n)
    {
        remainingSlimesDisplay.UpdateSlimesDisplay(n);
    }

    public void AddHealthCommand(int ammount)
    {
        StartCoroutine(AddHealth(ammount));
    }

    private IEnumerator AddHealth(int ammount)
    {
        yield return new WaitForSeconds(1f);

        currenthealth += ammount;
        healthDisplay.UpdateHealthDisplay(currenthealth);
        AkSoundEngine.PostEvent("Play_HealthUp", gameObject);
    }

    public void AddHealthFragment(int ammount)
    {
        currentHealthFragments += ammount;

        if (currentHealthFragments >= neededHealthFragments)
        {
            currentHealthFragments -= neededHealthFragments;
            StartCoroutine(AddHealth(1));
        }

        fragmentDisplay.UpdateFragDisplay(currentHealthFragments);
    }

    public void Freeze()
    {
        frozen = true;
        //myCollider.enabled = false;
        //myRigidbody.isKinematic = true;
        myAudioListener.enabled = false;
    }

    public void Unfreeze()
    {
        frozen = false;
        //myCollider.enabled = true;
        //myRigidbody.isKinematic = false;
        myAudioListener.enabled = true;
    }

    public void PlayFootSteps()
    {
        if (isGrounded)
        AkSoundEngine.PostEvent("Play_FootSteps", gameObject);
    }
}
