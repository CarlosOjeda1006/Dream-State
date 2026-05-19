using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    private Animator animator;

    public CharacterController characterController;
    public Transform cameraHolder;
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;
    public float jumpHeight = 1.2f;
    public float gravity = -25f;
    public float groundedGravity = -5f;
    public float mouseSensitivity = 2f;
    public float lookXLimit = 80f;

    [Header("Stamina")]
    public float maxStamina = 5f;
    public float staminaDrain = 1f;
    public float staminaRegen = 0.7f;
    public float currentStamina;

    bool isSprinting;
    bool wasSprinting;

    [HideInInspector]
    public float speedMultiplier = 1f;
    
    public bool canMove = true;

    private bool playingFootsteps = false;
    public float footstepSpeed = 0.5f;

    string axisH = "Horizontal";
    string axisV = "Vertical";

    string axisX = "Mouse X";
    string axisY = "Mouse Y";

    Vector3 velocity;
    float cameraRotationX;
    bool cursorLocked = true;

    void Start()
    {
        if (characterController == null)
            characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        currentStamina = maxStamina;

        LockCursor(true);
    }

    void Update()
    {
        //PARA QUE NO SE MUEVA LA CÁMARA AL ABRIR EL MENU
        if (MenuController.isMenuOpen) return;

        HandleMouseLook();
        HandleMovement();
        HandleCursorToggle();

        if (!canMove)
            return;


        //animations
        animator.SetBool("isRunning", Input.GetAxisRaw("Vertical") != 0);
        animator.SetBool("isJumping", !characterController.isGrounded);
    }

    void OnDisable()
    {
        StopFootsteps();
    }

    void HandleMouseLook()
    {

        float mouseX = Input.GetAxisRaw(axisX) * mouseSensitivity * 100f * Time.deltaTime;
        float mouseY = Input.GetAxisRaw(axisY) * mouseSensitivity * 100f * Time.deltaTime;

        cameraRotationX -= mouseY;
        cameraRotationX = Mathf.Clamp(cameraRotationX, -lookXLimit, lookXLimit);

        cameraHolder.localRotation = Quaternion.Euler(cameraRotationX, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleMovement()
    {
        bool isGrounded = characterController.isGrounded;

        float moveX = Input.GetAxisRaw(axisH);
        float moveZ = Input.GetAxisRaw(axisV);

        Vector3 move = (transform.right * moveX + transform.forward * moveZ).normalized;
        bool wantsToSprint = Input.GetButton("Run") && moveZ > 0;

        isSprinting = wantsToSprint && currentStamina > 0f;

        float baseSpeed = isSprinting ? sprintSpeed : walkSpeed;

        float currentSpeed = baseSpeed * speedMultiplier;

        characterController.Move(move * currentSpeed * Time.deltaTime);
        
        if (isSprinting)
        {
            currentStamina -= staminaDrain * Time.deltaTime;
        }
        else
        {
            currentStamina += staminaRegen * Time.deltaTime;
        }

        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        if (isGrounded && velocity.y < 0f)
        {
            velocity.y = groundedGravity;
        }

        if (wasSprinting != isSprinting)
        {
            wasSprinting = isSprinting;

            if (playingFootsteps)
            {
                StopFootsteps();
                StartFootsteps();
            }
        }


        /*
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        */
        if (isGrounded && (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump")))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

        //FOOTSTEPS
        bool isMoving = moveX != 0 || moveZ != 0;

        if (isGrounded && isMoving)
        {
            if (!playingFootsteps)
            {
                StartFootsteps();
            }
        }
        else
        {
            if (playingFootsteps)
            {
                StopFootsteps();
            }
        }
    }

    void HandleCursorToggle()
    {
        if (Input.GetKeyDown(KeyCode.X))
            LockCursor(false);

        if (Input.GetMouseButtonDown(0) && !cursorLocked)
            LockCursor(true);
    }

    void LockCursor(bool locked)
    {
        cursorLocked = locked;
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }

    void StartFootsteps()
    {
        playingFootsteps = true;

        float interval = isSprinting ? 0.3f : footstepSpeed;

        InvokeRepeating(nameof(PlayFootstep), 0f, interval);
    }

    void StopFootsteps()
    {
        playingFootsteps = false;
        CancelInvoke(nameof(PlayFootstep));
    }
    void PlayFootstep()
    {
        SoundEffectManager.Play("Footstep");
    }

    
}