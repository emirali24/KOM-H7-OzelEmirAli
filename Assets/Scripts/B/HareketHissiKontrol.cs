using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class HareketHissiKontrol : MonoBehaviour
{
    [System.Serializable]
    public struct HareketProfili
    {
        public string profilAdi;
        public float azamiHiz;
        public float ivme;
        public float fren;
        public float ziplamaHizi;
        public float coyoteSuresi;
        public float tamponSuresi;
        public float kisaZiplamaCarpani;
        public Color karakterRengi;
    }

    [Header("Profiller (Tab ile Degisir)")]
    public HareketProfili tuyProfili = new HareketProfili
    {
        profilAdi = "Tuy (Hafif - Celeste Tarzi)",
        azamiHiz = 9f,
        ivme = 90f,
        fren = 120f,
        ziplamaHizi = 14f,
        coyoteSuresi = 0.12f,
        tamponSuresi = 0.15f,
        kisaZiplamaCarpani = 0.45f,
        karakterRengi = new Color(0.2f, 0.8f, 1f) // Acik Mavi
    };

    public HareketProfili tasProfili = new HareketProfili
    {
        profilAdi = "Tas (Agir - Kaygan)",
        azamiHiz = 5.5f,
        ivme = 22f,
        fren = 25f,
        ziplamaHizi = 11f,
        coyoteSuresi = 0.05f,
        tamponSuresi = 0.06f,
        kisaZiplamaCarpani = 0.75f,
        karakterRengi = new Color(1f, 0.5f, 0.2f) // Turuncu
    };

    [Header("Zemin Kontrolu")]
    public Transform ayakNoktasi;
    public float ayakYaricapi = 0.15f;
    public LayerMask zeminKatmani;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private HareketProfili aktifProfil;
    private bool tuyAktif = true;

    private float yatayGirdi;
    private float coyoteSayaci;
    private float tamponSayaci;
    private bool yerde;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        ProfilUygula(tuyProfili);
    }

    void ProfilUygula(HareketProfili yeniProfil)
    {
        aktifProfil = yeniProfil;
        if (sr != null) sr.color = aktifProfil.karakterRengi;
        Debug.Log("Aktif Profil: " + aktifProfil.profilAdi);
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        // Tab tusu ile iki profil arasinda gecis yap[cite: 2]
        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            tuyAktif = !tuyAktif;
            ProfilUygula(tuyAktif ? tuyProfili : tasProfili);
        }

        // Yatay girdi (A/D veya Sol/Sag Ok)
        yatayGirdi = 0f;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) yatayGirdi -= 1f;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) yatayGirdi += 1f;

        // Zemin kontrolu
        yerde = Physics2D.OverlapCircle(ayakNoktasi.position, ayakYaricapi, zeminKatmani);

        // Coyote Time sayaci[cite: 33]
        if (yerde)
            coyoteSayaci = aktifProfil.coyoteSuresi;
        else
            coyoteSayaci -= Time.deltaTime;

        // Ziplama Tamponu (Jump Buffer) sayaci[cite: 34]
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
            tamponSayaci = aktifProfil.tamponSuresi;
        else
            tamponSayaci -= Time.deltaTime;

        // Degisken yukseklik: tus erken birakilirsa yukselisi kes[cite: 32]
        if (Keyboard.current.spaceKey.wasReleasedThisFrame && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * aktifProfil.kisaZiplamaCarpani);
        }

        // Asagi duserse yeniden baslat
        if (transform.position.y < -7f || Keyboard.current.rKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void FixedUpdate()
    {
        // Ivme ve Fren ile yatay hareket[cite: 37]
        float hedefHiz = yatayGirdi * aktifProfil.azamiHiz;
        float oran = Mathf.Abs(yatayGirdi) > 0.01f ? aktifProfil.ivme : aktifProfil.fren;
        float yeniX = Mathf.MoveTowards(rb.linearVelocity.x, hedefHiz, oran * Time.fixedDeltaTime);

        rb.linearVelocity = new Vector2(yeniX, rb.linearVelocity.y);

        // Coyote ve Tampon penceresi uygunsa zipla[cite: 33, 34]
        if (tamponSayaci > 0f && coyoteSayaci > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, aktifProfil.ziplamaHizi);
            tamponSayaci = 0f;
            coyoteSayaci = 0f;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (ayakNoktasi != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(ayakNoktasi.position, ayakYaricapi);
        }
    }
}