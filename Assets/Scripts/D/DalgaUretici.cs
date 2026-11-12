using UnityEngine;

public class DalgaUretici : MonoBehaviour
{
    public GameObject normalTuglaPrefab;
    public GameObject zirhliTuglaPrefab;
    public float dalgaAraligi = 4.5f;
    public float baslangicInisHizi = 0.45f;

    private float sayac;
    private float zorlukArtisi = 0f;

    void Start()
    {
        DalgaUret();
        sayac = dalgaAraligi;
    }

    void Update()
    {
        if (BirlesikOyuncu.oyunBitti) return;

        sayac -= Time.deltaTime;
        if (sayac <= 0f)
        {
            zorlukArtisi += 0.06f; // Zorluk zamanla artar
            DalgaUret();
            sayac = Mathf.Max(2.4f, dalgaAraligi - zorlukArtisi * 2f);
        }
    }

    void DalgaUret()
    {
        int sutun = 7;
        float baslangicX = -6f;
        float aralikX = 2.0f;

        for (int x = 0; x < sutun; x++)
        {
            if (Random.value < 0.15f) continue; // Aralarda bosluk birak

            Vector2 pos = new Vector2(baslangicX + x * aralikX, 4.3f);
            bool zirhliOlsun = Random.value < 0.35f;
            GameObject prefab = zirhliOlsun ? zirhliTuglaPrefab : normalTuglaPrefab;

            GameObject yeni = Instantiate(prefab, pos, Quaternion.identity, transform);
            BirlesikTugla bt = yeni.GetComponent<BirlesikTugla>();
            if (bt != null) bt.inisHizi = baslangicInisHizi + zorlukArtisi;
        }
    }
}