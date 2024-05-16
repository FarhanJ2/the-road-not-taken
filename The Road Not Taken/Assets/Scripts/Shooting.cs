using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private int damage = 5;
    public Transform firePoint;
    public GameObject bulletPrefab;
    private Rigidbody2D rb;
    private float bulletLifetime = 2f;

    public float bulletForce = 20f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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

        Vector2 worldPoint = PlayerCamera.Instance.cam.ScreenToWorldPoint(Mouse.current.position.ReadValue()); // old ScreenToViewportPoint not working either
        Vector2 lookDir = worldPoint - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f; // atan2 returns angle between x axi and a 2d vector starting at 0,0 terminating at x, y
        firePoint.rotation = Quaternion.Euler(0, 0, angle);

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet pref = bullet.GetComponent<Bullet>();
        pref.Damage = damage;
        pref.BulletType = BulletType.Player;
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
        Destroy(bullet, bulletLifetime);
    }
}
