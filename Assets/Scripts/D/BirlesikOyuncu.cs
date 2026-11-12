using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class BirlesikOyuncu : MonoBehaviour
{
    [Header("Hareket Hissi Ayarlari")]
    public float baslangicAzamiHiz = 7.5f;
    public float ivme = 55f;
    public float fren = 75f;
    public float ziplamaHizi = 13f;
    public float coyoteSuresi = 0.12f;
    public float tamponSuresi = 0.14f;
    public float kisaZiplamaCarpani = 0.5f;

    [Header("Tek Tus Kinetik Darbe")]
    public float darbeHizi = 17f;
    public float sarjSuresiEsigi = 0.18f; // Bu sureden fazla basili tutunca darbe yuklenir

    [Header("Zemin Kontrolu")]
    public Transform ayakNoktasi;
    public float ayakYaricapi = 0.22f;
    public LayerMask zeminKatmani;

    [HideInInspector] public float guncelAzamiHiz;
    [HideInInspector] public bool kinetikDarbeAktif;
    [HideInInspector] public int skor;
    public static bool oyunBitti;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private float yatayGirdi;
    private float coyoteSayaci;
    private float tamponSayaci;
    private bool yerde;

    private bool havadaSarjEdiyor;
    private float sarjSayaci;
    private bool darbeHakkiVar;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        guncelAzamiHiz = baslangicAzamiHiz;
        oyunBitti = false;
        skor = 0;
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        if (oyunBitti)
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame || Keyboard.current.rKey.wasPressedThisFrame)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            return;
        }

        // 1. Yatay Girdi (Update icinde okunur)
        yatayGirdi = 0f;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) yatayGirdi -= 1f;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) yatayGirdi += 1f;

        // Zemin kontrolu
        yerde = Physics2D.OverlapCircle(ayakNoktasi.position, ayakYaricapi, zeminKatmani);

        if (yerde)
        {
            coyoteSayaci = coyoteSuresi;
            darbeHakkiVar = true;
            kinetikDarbeAktif = false;
        }
        else
        {
            coyoteSayaci -= Time.deltaTime;
        }

        // Ziplama Tamponu
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            tamponSayaci = tamponSuresi;

            // Havadaysa ve darbe hakki varsa Tek Tus sarjini baslat ("Simdi mi, sonra mi?")
            if (!yerde && coyoteSayaci <= 0f && darbeHakkiVar)
            {
                havadaSarjEdiyor = true;
                sarjSayaci = 0f;
            }
        }
        else
        {
            tamponSayaci -= Time.deltaTime;
        }

        // Havada basili tutma suresi
        if (havadaSarjEdiyor && Keyboard.current.spaceKey.isPressed)
        {
            sarjSayaci += Time.deltaTime;
        }

        // Tus birakildiginda
        if (Keyboard.current.spaceKey.wasReleasedThisFrame)
        {
            // Degisken yukseklikli normal ziplama kesmesi
            if (!havadaSarjEdiyor && rb.linearVelocity.y > 0f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * kisaZiplamaCarpani);
            }

            // Kinetik Darbe firlatmasi!
            if (havadaSarjEdiyor)
            {
                havadaSarjEdiyor = false;
                if (sarjSayaci >= sarjSuresiEsigi)
                {
                    kinetikDarbeAktif = true;
                    darbeHakkiVar = false;
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x * 1.15f, darbeHizi);
                }
            }
        }

        // Gorsel geri bildirim (Sarj = Sari, Darbe = Camgobegi, Normal = Beyaz/Mavi)
        if (sr != null)
        {
            if (kinetikDarbeAktif) sr.color = Color.cyan;
            else if (havadaSarjEdiyor && sarjSayaci >= sarjSuresiEsigi) sr.color = Color.yellow;
            else sr.color = new Color(0.3f, 0.9f, 0.5f);
        }

        // Asagi duserse kaybet
        if (transform.position.y < -6f)
        {
            oyunBitti = true;
        }
    }

    void FixedUpdate()
    {
        if (oyunBitti)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // Ivme ve Fren ile yatay hareket
        float hedefHiz = yatayGirdi * guncelAzamiHiz;
        float oran = Mathf.Abs(yatayGirdi) > 0.01f ? ivme : fren;
        float yeniX = Mathf.MoveTowards(rb.linearVelocity.x, hedefHiz, oran * Time.fixedDeltaTime);
        float yeniY = rb.linearVelocity.y;

        // Havada sarj ederken dususu yavaslat (nisan alma penceresi)
        if (havadaSarjEdiyor && yeniY < 0f)
        {
            yeniY *= 0.82f;
        }

        rb.linearVelocity = new Vector2(yeniX, yeniY);

        // Coyote ve Tampon ile ziplama
        if (tamponSayaci > 0f && coyoteSayaci > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, ziplamaHizi);
            tamponSayaci = 0f;
            coyoteSayaci = 0f;
        }
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.CompareTag("Tugla"))
        {
            BirlesikTugla tugla = c.gameObject.GetComponent<BirlesikTugla>();
            int hasar = kinetikDarbeAktif ? 2 : 1; // Bag 2: Kinetik darbe zirhli tuglayi tekte kirar

            if (tugla != null)
            {
                bool kirildi = tugla.HasarAl(hasar);
                if (kirildi)
                {
                    skor += (hasar == 2) ? 25 : 10;
                    // Bag 3: Kirilan tugla hizi artirir ve havadaki darbe hakkini yeniler
                    guncelAzamiHiz = Mathf.Min(guncelAzamiHiz + 0.25f, 15f);
                    ivme += 1.5f;
                    darbeHakkiVar = true;
                }
            }

            // Bag 1 (Tasarlanmis Carpisma): Yatay hiza (ivmeye) gore sekme acisi
            float yatayOran = Mathf.Clamp(rb.linearVelocity.x / guncelAzamiHiz, -1f, 1f);
            Vector2 sekmeYonu = new Vector2(yatayOran * 0.8f, -1f).normalized;
            rb.linearVelocity = sekmeYonu * (kinetikDarbeAktif ? 11f : 8f);
            kinetikDarbeAktif = false;
        }
    }

    void OnGUI()
    {
        GUIStyle stil = new GUIStyle(GUI.skin.label);
        stil.fontSize = 20;
        stil.normal.textColor = Color.white;

        GUI.Label(new Rect(20, 15, 500, 35), $"Skor: {skor}  |  Azami Hiz: {guncelAzamiHiz:F1}", stil);

        if (oyunBitti)
        {
            stil.fontSize = 28;
            stil.normal.textColor = Color.red;
            GUI.Label(new Rect(Screen.width / 2f - 220, Screen.height / 2f - 30, 500, 60),
                "OYUN BITTI! Tekrar icin SPACE", stil);
        }
    }
}