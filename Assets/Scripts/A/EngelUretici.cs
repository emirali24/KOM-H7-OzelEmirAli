using UnityEngine;

public class EngelUretici : MonoBehaviour
{
    public GameObject engelPrefab;
    public float baslangicHizi = 6f;
    public float hizArtisi = 0.15f; // Zorluk artışı değişkeni
    public float minAralik = 1.0f;
    public float maxAralik = 1.9f;

    private float guncelHiz;
    private float sayac;
    private int gecilenEngel;

    void Start()
    {
        guncelHiz = baslangicHizi;
        sayac = 1f;
    }

    void Update()
    {
        if (TekTusOyuncu.oyunBitti) return;

        sayac -= Time.deltaTime;
        if (sayac <= 0f)
        {
            EngelUret();
            // Hız arttıkça üretim aralığı da hafifçe kısalır
            float carpan = baslangicHizi / guncelHiz;
            sayac = Random.Range(minAralik, maxAralik) * Mathf.Clamp(carpan, 0.55f, 1f);
        }
    }

    void EngelUret()
    {
        float yKonum = Random.value > 0.3f ? -2.35f : -1.4f; // Bazen yerden, bazen hafif yüksekten gelir
        GameObject yeniEngel = Instantiate(engelPrefab, new Vector3(10f, yKonum, 0f), Quaternion.identity);
        EngelHareket hareket = yeniEngel.AddComponent<EngelHareket>();
        hareket.hiz = guncelHiz;

        gecilenEngel++;
        guncelHiz += hizArtisi; // Her engelde hız artar[cite: 53]
    }
}

public class EngelHareket : MonoBehaviour
{
    public float hiz;

    void Update()
    {
        if (TekTusOyuncu.oyunBitti) return;

        transform.Translate(Vector2.left * hiz * Time.deltaTime);

        if (transform.position.x < -12f)
        {
            Destroy(gameObject);
        }
    }
}