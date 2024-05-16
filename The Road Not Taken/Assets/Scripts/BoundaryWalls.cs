using UnityEngine;

public class BoundaryWall : MonoBehaviour
{
    [SerializeField] private bool overrideTag = false;
    private Color gizmoColor = Color.yellow;

    private void Awake()
    {
        if (!overrideTag)
        {
            gameObject.tag = "Wall";
        }

        gameObject.AddComponent<BoxCollider2D>();
        gameObject.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
