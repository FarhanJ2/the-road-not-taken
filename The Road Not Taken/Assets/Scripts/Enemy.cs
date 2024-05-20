using System.Collections;
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

    [SerializeField] private AudioSource deathGroan;
    [SerializeField] private AudioClip[] deathSounds;

    [SerializeField] private AudioSource fireSound;
    [SerializeField] private AudioClip[] fireSounds;

    public GameObject bulletPrefab;
    private Rigidbody2D rb;
    private float bulletLifetime = 2f;
    private float bulletForce = 20f;
    private bool disabled = false;

    private void Awake()
    {
        Health = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(AttackSequence());
    }

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Return))
        // {
        //     Debug.Log("Key down");
        //     Attack();
        // }
        if (PlayerMovement.playerPos.x > transform.position.x) {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        } else {
             transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    IEnumerator AttackSequence()
    {
        while (true)
        {
            Attack();
            yield return new WaitForSeconds(.5f);
            Attack();
            yield return new WaitForSeconds(.5f);
            Attack();
            yield return new WaitForSeconds(.5f);
            Attack();
            yield return new WaitForSeconds(.5f);
            Attack();
            yield return new WaitForSeconds(2f);
        }
    }

    private void Attack()
    {
        // attack player
        if (disabled) return;
        
        fireSound.clip = fireSounds[Random.Range(0, fireSounds.Length)];
        fireSound.Play();

        // deal damage to player
        Vector2 lookDir = PlayerMovement.playerPos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f; // atan2 returns angle between x axi and a 2d vector starting at 0,0 terminating at x, y
        firePoint.rotation = Quaternion.Euler(0, 0, angle);

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), bullet.GetComponent<Collider2D>());
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle + 135f);
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
        if (disabled) return;
        disabled = true;
        // PlayerStats.Score += xpOnKilled;
        PlayerStats.ChangeScore(xpOnKilled);

        deathGroan.clip = deathSounds[Random.Range(0, deathSounds.Length)];
        deathGroan.Play();

        // Destroy self for now until animation is created
        
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().color = new Color(140, 0, 0, 1);

        // Destroy(gameObject, 3f);
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