using System.Collections;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Vector3 offset = new Vector3(0 , 0 , -10);
    public static PlayerCamera Instance { get; private set; }
    [HideInInspector] public Camera cam;
    [SerializeField] private Transform player;
    [HideInInspector]
	public float shakeValue = 0.0f;
	[HideInInspector]
	public bool onShaking = false;
	private float shakingv = 0.0f;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
		if(!player){
			player = GameObject.FindWithTag("Player").transform;
		}

        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
        cam = GetComponent<Camera>();

        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void FixeddUpdate()
    {
        // transform.position = new Vector3(player.position.x, player.position.y, -10); old

        if (onShaking && GlobalStatus.freezeCam){
			shakeValue = Random.Range(-shakingv , shakingv)* 0.2f;
			transform.position += new Vector3(0,shakeValue,0);
		}
		if (!player || GlobalStatus.freezeCam){
			return;
		}
		
		if (Time.timeScale == 0.0f){
			return;
		}
		transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, offset.z);
		if (onShaking){
			shakeValue = Random.Range(-shakingv , shakingv)* 0.2f;
			transform.position += new Vector3(0 , shakeValue , 0);
		}
    }

    public void Shake(float val , float dur){
		if(onShaking){
			return;
		}
		shakingv = val;
		StartCoroutine(Shaking(dur));
	}
	
	public IEnumerator Shaking(float dur){
		onShaking = true;
		yield return new WaitForSeconds(dur);
		shakingv = 0;
		shakeValue = 0;
		onShaking = false;
	}
	
	public void SetNewTarget(Transform p){
		player = p;
	}
	
	void OnEnable(){
		shakingv = 0;
		shakeValue = 0;
		onShaking = false;
	}
}
