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

        LockCursor(true);
    }

    void Update()
    {
        //PARA QUE NO SE MUEVA LA CÁMARA AL ABRIR EL MENU
        if (MenuController.isMenuOpen) return;

        HandleMouseLook();
        HandleMovement();
        HandleCursorToggle();


        //animations
        animator.SetBool("isRunning", Input.GetAxisRaw("Vertical") != 0);
        animator.SetBool("isJumping", !characterController.isGrounded);
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
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;

        characterController.Move(move * currentSpeed * Time.deltaTime);

        if (isGrounded && velocity.y < 0f)
        {
            velocity.y = groundedGravity;
        }



        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

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
        if (Input.GetKeyDown(KeyCode.Escape))
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
        InvokeRepeating(nameof(PlayFootstep), 0f, footstepSpeed);
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