using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movement")]
    public float moveSpeed = 5f;
    public float runSpeed = 8f;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private PlayerController playerController;

    // Untuk input dari button UI
    private float mobileInputX = 0f;
    private float mobileInputY = 0f;

    private Vector2 moveInput;

    private enum MovementState { idle, walk, run }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        // ✅ Ambil PlayerController dari GameObject yang sama
        playerController = GetComponent<PlayerController>();
    }

    private void OnEnable()
    {
        if (playerController != null)
        {
            playerController.enabled = true; // aktifkan script PlayerController

            // Pastikan playerControl tidak null
            var playerControl = GetPlayerControl();
            if (playerControl != null)
            {
                playerControl.Movement.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
                playerControl.Movement.Move.canceled += ctx => moveInput = Vector2.zero;
                playerControl.Enable();
            }
        }
        else
        {
            Debug.LogError("PlayerController tidak ditemukan di GameObject ini!");
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
        else
        {
            var playerControl = GetPlayerControl();
            if (playerControl != null)
                moveInput = playerControl.Movement.Move.ReadValue<Vector2>();
        }
    }

    private void FixedUpdate()
    {
        Vector2 targetVelocity = new Vector2((moveInput.x + mobileInputX) * moveSpeed,
                                             (moveInput.y + mobileInputY) * moveSpeed);
        rb.velocity = targetVelocity;

        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        MovementState state;

        float horizontal = moveInput.x != 0 ? moveInput.x : mobileInputX;
        float vertical = moveInput.y != 0 ? moveInput.y : mobileInputY;

        if (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f)
        {
            state = MovementState.walk;

            if (horizontal > 0f)
                sprite.flipX = false;
            else if (horizontal < 0f)
                sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        anim.SetInteger("state", (int)state);
    }

    public void MoveRight(bool isPressed)
    {
        mobileInputX = isPressed ? 1f : (mobileInputX == 1f ? 0f : mobileInputX);
    }

    public void MoveLeft(bool isPressed)
    {
        mobileInputX = isPressed ? -1f : (mobileInputX == -1f ? 0f : mobileInputX);
    }

    public void MoveUp(bool isPressed)
    {
        mobileInputY = isPressed ? 1f : (mobileInputY == 1f ? 0f : mobileInputY);
    }

    public void MoveDown(bool isPressed)
    {
        mobileInputY = isPressed ? -1f : (mobileInputY == -1f ? 0f : mobileInputY);
    }

    // 🔄 Helper untuk ambil PlayerControl dari PlayerController
    private PlayerControl GetPlayerControl()
    {
        return playerController != null ? typeof(PlayerController)
            .GetField("playerControl", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(playerController) as PlayerControl : null;
    }
}
