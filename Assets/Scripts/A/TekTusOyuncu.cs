using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TekTusOyuncu : MonoBehaviour
{
    [Header("Ayarlar")]
    public float ziplamaHizi = 13f;
    public float kisaZiplamaCarpani = 0.5f;
    public float havadaDalisHizi = -18f;

    private Rigidbody2D rb;
    private bool yerde;
    private bool havadaTusaBasildi;
    private bool dalisYapiyor;
    public static bool oyunBitti;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        oyunBitti = false;
        Time.timeScale = 1f;
    }

    void Update()
    {
        bool basildi = (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame) ||
                       (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame);

        bool basiliTutuluyor = (Keyboard.current != null && Keyboard.current.spaceKey.isPressed) ||
                               (Mouse.current != null && Mouse.current.leftButton.isPressed);

        bool birakildi = (Keyboard.current != null && Keyboard.current.spaceKey.wasReleasedThisFrame) ||
                         (Mouse.current != null && Mouse.current.leftButton.wasReleasedThisFrame);

        // Oyun bittiyse aynı tek tuşla yeniden başlat
        if (oyunBitti)
        {
            if (basildi)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            return;
        }

        // 1. Zamanlama Biçimi: BAS / BASILI TUT (Zıplama)
        if (basildi && yerde)
        {
            rb.linearVelocity = new Vector2(0f, ziplamaHizi);
            yerde = false;
            havadaTusaBasildi = false;
        }
        else if (basildi && !yerde)
        {
            // Havadayken ikinci kez basıldı
            havadaTusaBasildi = true;
        }

        // Erken bırakılırsa kısa zıpla
        if (birakildi && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y * kisaZiplamaCarpani);
        }

        // 2. Zamanlama Biçimi: HAVADA TUT (Hızlı Dalış)
        dalisYapiyor = (!yerde && havadaTusaBasildi && basiliTutuluyor);
    }

    void FixedUpdate()
    {
        if (oyunBitti) return;

        if (dalisYapiyor)
        {
            rb.linearVelocity = new Vector2(0f, havadaDalisHizi);
        }
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.CompareTag("Engel"))
        {
            oyunBitti = true;
            Time.timeScale = 0f;
            Debug.Log("Oyun Bitti! Yeniden baslamak icin Space/Tikla.");
        }
        else
        {
            yerde = true;
            havadaTusaBasildi = false;
            dalisYapiyor = false;
        }
    }
}