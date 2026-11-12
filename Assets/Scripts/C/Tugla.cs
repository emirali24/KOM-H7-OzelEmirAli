using UnityEngine;

public class Tugla : MonoBehaviour
{
    public int can = 1; // 1 = Normal, 2 = Iki Vurusluk Ozel Tugla[cite: 2, 19]
    public Color catlakRenk = Color.yellow;
    public GameObject kristalPrefab;

    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void HasarAl()
    {
        can--;
        if (can == 1)
        {
            // Ilk vurusta catladi, rengi degisti
            if (sr != null) sr.color = catlakRenk;
        }
        else if (can <= 0)
        {
            // Iki vurusluk ozel tugla kirilirsa tetikleyici kristal dusurur[cite: 2, 13]
            if (kristalPrefab != null)
            {
                Instantiate(kristalPrefab, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }
}