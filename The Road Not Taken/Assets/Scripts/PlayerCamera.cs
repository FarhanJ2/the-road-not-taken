using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform player;

    private void Awake()
    {
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        transform.position = new Vector3(player.position.x, player.position.y, -10);
    }
}
