using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public CharacterController characterController;
    public Transform cameraHolder;
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;
    public float jumpHeight = 1.2f;
    public float gravity = -25f;
    public float groundedGravity = -5f;
    public float mouseSensitivity = 2f;
    public float lookXLimit = 80f;

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

        LockCursor(true);
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
        HandleCursorToggle();
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
            velocity.y = groundedGravity;

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
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
}