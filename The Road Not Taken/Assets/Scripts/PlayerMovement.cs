using UnityEngine.Scripting.APIUpdating;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static bool disabled = false;

    private float startSpeed = 5f;
    private float terminalSpeed = 8f;
    private float moveSpeed;
    private PlayerControls playerControls;
    private Vector2 movement;
    public static Vector2 worldPoint;
    public static Vector2 playerPos;
    private Rigidbody2D rb;
    private Animator animator;
    public static Vector3 deathPos;
    public static Vector3 spawnPoint;
    private new Transform transform;
    private void Awake()
    {
        moveSpeed = startSpeed;
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        transform = GetComponent<Transform>();

        transform.position = spawnPoint;
    }

    private void OnEnable()
    {
        playerControls.Enable();
        PlayerStats.onPlayerDeath += OnPlayerDeath;
    }

    private void OnDisable()
    {
        playerControls.Disable();
        PlayerStats.onPlayerDeath -= OnPlayerDeath;
    }

    private void Update()
    {
        PlayerInput();
        RampSpeed();

        playerPos = transform.position;
    }

    private void FixedUpdate()
    {
        Move();
    }

    [SerializeField] private AudioSource flappingWings;
    private void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();

        if (movement.sqrMagnitude > .01) // allows the idle animation to work as it ends with which way the player was facing
        {
            flappingWings.Play();

            animator.SetFloat("horizontal", movement.x);
            animator.SetFloat("vertical", movement.y);
            animator.SetFloat("speed", movement.sqrMagnitude); // gets sqaure magnitude of movement vector, length of movement vector squared
        } else {
            flappingWings.Stop();
            animator.SetFloat("speed", movement.sqrMagnitude);
        }
    }

    private void Move()
    {
        // legacy movement
        // rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));

        // new smooth curve movement
        if (!disabled)
        {
            Vector2 targetVelocity = movement * moveSpeed;
            Vector2 currentVelocity = rb.velocity;
            Vector2 smoothVelocity = Vector2.Lerp(currentVelocity, targetVelocity, 0.25f); // lerp from curr to targ by 0.25 to smooth out velocity
            rb.velocity = smoothVelocity;
        }
        else
            rb.velocity = Vector2.zero;
    }

    private void RampSpeed()
    {
        if (movement != Vector2.zero) // check if moving
        {
            StopCoroutine(ResetMoveSpeedAfterDelay(3f)); // stop the coroutine if the player starts moving
            moveSpeed += Time.deltaTime * 0.8f; // increase moveSpeed by the time passed since the last frame
            moveSpeed = Mathf.Clamp(moveSpeed, 0f, terminalSpeed); // cap moveSpeed at terminalSpeed
        }
        else if (!IsCoroutineRunning("ResetMoveSpeedCoroutine")) // check if the coroutine is already running
        {
            StartCoroutine(ResetMoveSpeedAfterDelay(3f)); // wait for 3 seconds before resetting moveSpeed
        }
    }

    private bool IsCoroutineRunning(string coroutineName)
    {
        var methods = typeof(MonoBehaviour).GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        foreach (var method in methods)
        {
            if (method.Name == "StopCoroutine" || method.Name == "StopCoroutineFromEnumerator")
            {
                var enumerator = method.GetParameters()[0].ParameterType.BaseType;
                if (enumerator != null && enumerator.Name.Contains("IEnumerator"))
                {
                    var runningCoroutines = (List<object>)method.Invoke(this, new object[] { coroutineName });
                    if (runningCoroutines != null && runningCoroutines.Count > 0)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private IEnumerator ResetMoveSpeedAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        moveSpeed = startSpeed; // reset moveSpeed to startSpeed
    }

    private void OnPlayerDeath()
    {
        disabled = true;

        // death animation

        deathPos = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("Player hit something");
        if (col.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Player hit a wall!");

            // calculate health penalty by speed of impact
            // calculate speed of impact
            float speed = col.relativeVelocity.magnitude;

            // use the speed to influence velocity
            Debug.Log(speed);


            // Vector2 direction = (rb.position - col.contacts[0].point).normalized;
            // rb.velocity = direction * speed;


            PlayerStats playerStats = GetComponent<PlayerStats>();
            playerStats.TakeDamage((int)speed); // cast as an int
        }
    }
}
