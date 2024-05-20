using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int scorePerCollection = 5;
    public AudioSource pickUpSound;
    private SpriteRenderer spriteRenderer;
    public new bool enabled = true;
    private Color lerpColor = Color.white;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public IEnumerator FadeOut()
    {
        enabled = false;
        while (spriteRenderer.color.a > 0.01)
        {
            lerpColor = Color.Lerp(lerpColor, new Color(255, 255, 255, 0), Time.deltaTime * 5);
            spriteRenderer.color = lerpColor;
            yield return null;
        }
        
        Destroy(gameObject);
    }
}
