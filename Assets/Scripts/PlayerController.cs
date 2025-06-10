using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public bool FacingLeft { get { return facingLeft; } set { facingLeft = value; } }

    [SerializeField] private float moveSpeed = 1f;
    public PlayerControl playerControl { get; private set; }

    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator anim;
    public SpriteRenderer sprite;
    private bool facingLeft = false;

    private void Awake()
    {
        if (playerControl == null)
            playerControl = new PlayerControl();

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        Debug.Log("PlayerController Enabled");

        if (playerControl != null)
        {
            Debug.Log("PlayerControl ditemukan");

            playerControl.Movement.Move.performed += ctx => movement = ctx.ReadValue<Vector2>();
            playerControl.Movement.Move.canceled += ctx => movement = Vector2.zero;
            playerControl.Enable();
        }
        else
        {
            Debug.LogError("PlayerControl NULL di OnEnable");
        }
    }

    private void OnDisable()
    {
        if (playerControl != null)
        {
            playerControl.Disable();
        }
    }

    private void Update()
    {
        anim.SetFloat("moveX", movement.x);
        anim.SetFloat("moveY", movement.y);
    }

    private void FixedUpdate()
    {
        AdjustPlayerFacingDirection();
        Move();
    }

    private void Move()
    {
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }

    private void AdjustPlayerFacingDirection()
    {
        if (movement.x != 0f)
        {
            sprite.flipX = movement.x < 0f;
            FacingLeft = true;
        }
        else if (movement.y != 0f)
        {
            sprite.flipX = false;
            FacingLeft = false;
        }
    }
}
