using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int Health { get; private set; }
    [SerializeField] private int damage = 5;
    [SerializeField] private int maxHealth = 50;
    [SerializeField] private int xpOnKilled = 10;

    [SerializeField] Texture2D projectileTexture;
    [SerializeField] private Transform firePoint;

    [SerializeField] private Texture2D cursorCrosshair;
    [SerializeField] private CursorMode cursorMode = CursorMode.Auto;
    [SerializeField] private Canvas healthBarCanvas;

    public GameObject bulletPrefab;
    private Rigidbody2D rb;
    private float bulletLifetime = 2f;
    private float bulletForce = 20f;
    private bool disabled = false;

    private void Awake()
    {
        Health = maxHealth;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("Key down");
            Attack();
        }
    }

    private void Attack()
    {
        // attack player
        if (disabled) return;
        
        // deal damage to player
        Vector2 lookDir = PlayerMovement.playerPos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f; // atan2 returns angle between x axi and a 2d vector starting at 0,0 terminating at x, y
        firePoint.rotation = Quaternion.Euler(0, 0, angle);

        // fix so that it doesnt hit its own collider
        // GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        // bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
        // Destroy(bullet, bulletLifetime);

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet pref = bullet.GetComponent<Bullet>();
        pref.Damage = damage;
        pref.BulletType = BulletType.Enemy;
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
        Destroy(bullet, bulletLifetime);
    }

    private void SearchForPlayer()
    {
        // search for player
        if (disabled)
            return;

        // on sight, move towards player
        
        
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        disabled = true;
        // PlayerStats.Score += xpOnKilled;
        PlayerStats.ChangeScore(xpOnKilled);

        // Destroy self for now until animation is created
        Destroy(gameObject);
    }

    private void OnMouseEnter() {
        // show health bar


        // set cursor to crosshair
        Cursor.SetCursor(cursorCrosshair, Vector2.zero, cursorMode);
    }

    private void OnMouseExit() {
        // hide health bar


        // null sets to default cursor
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }
}