using UnityEngine;
using UnityEngine.InputSystem;

public class RaketKontrol : MonoBehaviour
{
    public float hiz = 12f;
    public float sinirX = 7.3f;

    void Update()
    {
        if (Keyboard.current == null) return;

        float girdi = 0f;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) girdi -= 1f;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) girdi += 1f;

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x + girdi * hiz * Time.deltaTime, -sinirX, sinirX);
        transform.position = pos;
    }

    // Tetikleyici (Trigger) ile kristal yakalama[cite: 2, 13]
    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.CompareTag("Kristal") || c.name.Contains("Kristal"))
        {
            TopKontrol top = FindFirstObjectByType<TopKontrol>();
            if (top != null) top.HizArtir(1.5f);
            Destroy(c.gameObject);
            Debug.Log("Tetikleyici Kristal Alindi: Top Hizlandi!");
        }
    }
}