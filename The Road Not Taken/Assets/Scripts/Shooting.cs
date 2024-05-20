using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private int damage = 5;
    [SerializeField] AudioSource fire;
    [SerializeField] AudioClip[] fireSounds;
    public Transform firePoint;
    public GameObject bulletPrefab;
    private Rigidbody2D rb;
    public float bulletForce = 20f;

    private LineRenderer laserLine; // temp linerenderer for raycast

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        laserLine = GetComponent<LineRenderer>();

        // Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }

    private void OnEnable()
    {
        InputManager.Instance.playerControls.Attack.Fire.started += OnFireStart;
    }

    private void OnDisable()
    {
        InputManager.Instance.playerControls.Attack.Fire.started -= OnFireStart;
    }

    private void Update() {}

    private void OnFireStart(InputAction.CallbackContext context)
    {
        if (PlayerMovement.disabled)
            return;

        // switching to ray cast checks
        fire.clip = fireSounds[Random.Range(0, fireSounds.Length)];
        fire.Play();

        Ray ray = PlayerCamera.Instance.cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
        if (hit.collider != null)
        {
            // Debug.Log(hit.collider.name);
            if (hit.collider.CompareTag("Enemy"))
            {
                hit.collider.GetComponent<Enemy>().TakeDamage(damage);
            }
        }

        // keep for muzzle flash direction
        Vector2 worldPoint = PlayerCamera.Instance.cam.ScreenToWorldPoint(Mouse.current.position.ReadValue()); // old ScreenToViewportPoint not working either
        Vector2 lookDir = worldPoint - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f; // atan2 returns angle between x axi and a 2d vector starting at 0,0 terminating at x, y
        firePoint.rotation = Quaternion.Euler(0, 0, angle);


        // GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        // Bullet pref = bullet.GetComponent<Bullet>();
        // pref.Damage = damage;
        // pref.BulletType = BulletType.Player;
        // bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
        // Destroy(bullet, bulletLifetime);

        // Vector2 lookDir = PlayerMovement.playerPos - rb.position;
        // float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f; // atan2 returns angle between x axi and a 2d vector starting at 0,0 terminating at x, y
        // firePoint.rotation = Quaternion.Euler(0, 0, angle);


        // Debug.Log("Attacking player at angle: " + angle);
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), bullet.GetComponent<Collider2D>());
        // Debug.Log("Quaternion: " + quaternion);
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle + 135f);
        Bullet pref = bullet.GetComponent<Bullet>();
        pref.Damage = damage;
        pref.BulletType = BulletType.Player;
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
        Destroy(bullet, 2f);
    }
}
