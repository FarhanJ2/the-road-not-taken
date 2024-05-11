using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerManager : MonoBehaviour
{
    public static bool disabled = false;

    [SerializeField] private float moveSpeed = 5f;
    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;

    private void Awake()
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();
    }

    private void Move()
    {
        // legacy movement
        // rb.MovePosition(rb.position + movement * (moveSpeed * Time.deltaTime));

        // new smooth curve movement
        if (!disabled)
        {
            Vector2 targetVelocity = movement * moveSpeed;
            Vector2 currentVelocity = rb.velocity;
            Vector2 smoothVelocity = Vector2.Lerp(currentVelocity, targetVelocity, 0.25f); // lerp from curr to targ by 0.25 to smooth out velocity
            rb.velocity = smoothVelocity;
        } else 
            rb.velocity = Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D col) {
        Debug.Log("Player hit something");
        if (col.gameObject.CompareTag("Enemy")) {
            Debug.Log("Player hit by enemy");
            PlayerStats.TakeDamage(10);
        }    
    }
}
