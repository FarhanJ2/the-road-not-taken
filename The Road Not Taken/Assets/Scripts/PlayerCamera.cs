using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera Instance { get; private set; }
    [HideInInspector] public Camera cam;
    [SerializeField] private Transform player;

    private void Awake()
    {
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
        cam = GetComponent<Camera>();

        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void FixedUpdate()
    {
        transform.position = new Vector3(player.position.x, player.position.y, -10);
    }
}
