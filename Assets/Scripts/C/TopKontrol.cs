using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TopKontrol : MonoBehaviour
{
    public float hiz = 8f;
    public float tuglaHizArtisi = 0.15f; // Her tuglada zorluk artisi[cite: 17]

    private Rigidbody2D rb;
    private bool basladi;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        if (!basladi && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            basladi = true;
            rb.linearVelocity = new Vector2(0.4f, 1f).normalized * hiz;
        }

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void FixedUpdate()
    {
        if (!basladi) return;

        Vector2 v = rb.linearVelocity;
        // Yatay donguye sikismayi onleme
        if (Mathf.Abs(v.y) < 0.25f)
        {
            v.y = v.y >= 0f ? 0.25f : -0.25f;
        }
        rb.linearVelocity = v.normalized * hiz; // Hizi sabit tut[cite: 14, 16]
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        // 1. Tasarlanmis Carpisma: Raketin neresine degdigine gore aci ver[cite: 2, 15, 16]
        if (c.gameObject.CompareTag("Raket"))
        {
            float yari = c.collider.bounds.size.x / 2f;
            float fark = (transform.position.x - c.transform.position.x) / yari;
            rb.linearVelocity = new Vector2(fark, 1f).normalized * hiz;
        }
        // 2. Tugla Carpismasi[cite: 17]
        else if (c.gameObject.CompareTag("Tugla"))
        {
            Tugla tugla = c.gameObject.GetComponent<Tugla>();
            if (tugla != null) tugla.HasarAl();
            hiz += tuglaHizArtisi;
        }
    }

    // Tetikleyici (Trigger) ile alt sinirdan cikis[cite: 2, 13]
    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.name == "OlumBolgesi")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void HizArtir(float miktar)
    {
        hiz += miktar;
    }
}