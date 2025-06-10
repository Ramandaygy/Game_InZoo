using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movement")]
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private PlayerController playerController;

    private Vector2 moveInput;
    private Vector2 currentInput;

    private float mobileInputX = 0f;
    private float mobileInputY = 0f;

    private enum MovementState { idle, walk }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        playerController = GetComponent<PlayerController>();
    }

    private void OnEnable()
    {
        if (playerController != null)
        {
            var playerControl = GetPlayerControl();
            if (playerControl != null)
            {
                playerControl.Movement.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
                playerControl.Movement.Move.canceled += ctx => moveInput = Vector2.zero;
                playerControl.Enable();
            }
        }
    }

    private void OnDisable()
    {
        var playerControl = GetPlayerControl();
        if (playerControl != null)
        {
            playerControl.Disable();
        }
    }

    private void Update()
    {
        if (Application.isMobilePlatform)
        {
            moveInput = new Vector2(mobileInputX, mobileInputY);
        }
    }

    private void FixedUpdate()
    {
        currentInput = new Vector2(
            Mathf.Clamp(moveInput.x + mobileInputX, -1f, 1f),
            Mathf.Clamp(moveInput.y + mobileInputY, -1f, 1f)
        );

        rb.velocity = currentInput * moveSpeed;
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        MovementState state;

        float horizontal = currentInput.x;
        float vertical = currentInput.y;

        if (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f)
        {
            state = MovementState.walk;
            sprite.flipX = horizontal > 0f;
        }
        else
        {
            state = MovementState.idle;
        }

        anim.SetInteger("state", (int)state);
    }

    public void MoveRight(bool isPressed) => mobileInputX = isPressed ? 1f : (mobileInputX == 1f ? 0f : mobileInputX);
    public void MoveLeft(bool isPressed) => mobileInputX = isPressed ? -1f : (mobileInputX == -1f ? 0f : mobileInputX);
    public void MoveUp(bool isPressed) => mobileInputY = isPressed ? 1f : (mobileInputY == 1f ? 0f : mobileInputY);
    public void MoveDown(bool isPressed) => mobileInputY = isPressed ? -1f : (mobileInputY == -1f ? 0f : mobileInputY);

    private PlayerControl GetPlayerControl()
    {
        return playerController != null ? playerController.playerControl : null;
    }
}
