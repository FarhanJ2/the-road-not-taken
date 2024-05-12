using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int Health { get; private set; }
    public int Damage { get; private set; }
    [SerializeField] private int maxHealth = 50;
    [SerializeField] private int maxDamage = 5;
    [SerializeField] private int xpOnKilled = 10;

    [SerializeField] Texture2D projectileTexture;

    private bool disabled = false;

    private void Awake()
    {
        Health = maxHealth;
        Damage = maxDamage;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Key down");
            Attack();
        }
    }

    private void Attack()
    {
        // attack player
        if (disabled)
            return;
        
        // deal damage to player
        // crreate a projectile that goes towards the player
        // var out = Instantiate(new GameObject(), transform.position, Quaternion.identity);

        // GameObject projectile = new GameObject();
        // Sprite projectile = Sprite.Create(projectileTexture, new Rect(0.0f, 0.0f, projectileTexture.width, projectileTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
        

        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        playerStats.TakeDamage(Damage);
    }

    private void SearchForPlayer()
    {
        // search for player
        if (disabled)
            return;

        // on sight, move towards player
        

    }

    private void TakeDamage(int damage)
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
        PlayerStats.Score += xpOnKilled;

        // Destroy self for now until animation is created
        Destroy(gameObject);
    }
}