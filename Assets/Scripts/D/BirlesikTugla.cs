using UnityEngine;

public class BirlesikTugla : MonoBehaviour
{
    public int can = 1; // 1 = Normal, 2 = Zirhli
    public float inisHizi = 0.45f;
    public Color catlakRenk = new Color(1f, 0.5f, 0f);

    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (BirlesikOyuncu.oyunBitti) return;

        // Tugla dalgasi asagi dogru ilerler
        transform.Translate(Vector2.down * inisHizi * Time.deltaTime);

        // Tehlike cizgisine (-2.2) ulasirsa oyun biter
        if (transform.position.y <= -2.2f)
        {
            BirlesikOyuncu.oyunBitti = true;
        }
    }

    public bool HasarAl(int miktar)
    {
        can -= miktar;
        if (can == 1)
        {
            if (sr != null) sr.color = catlakRenk;
            return false;
        }
        else if (can <= 0)
        {
            Destroy(gameObject);
            return true;
        }
        return false;
    }
}