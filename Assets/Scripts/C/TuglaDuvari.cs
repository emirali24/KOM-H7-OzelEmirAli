using UnityEngine;

public class TuglaDuvari : MonoBehaviour
{
    public GameObject normalTuglaPrefab;
    public GameObject ozelTuglaPrefab;
    public int satir = 4;
    public int sutun = 8;
    public Vector2 baslangic = new Vector2(-6.3f, 3.5f);
    public Vector2 aralik = new Vector2(1.8f, 0.7f);

    void Start()
    {
        for (int y = 0; y < satir; y++)
        {
            for (int x = 0; x < sutun; x++)
            {
                Vector2 p = baslangic + new Vector2(x * aralik.x, -y * aralik.y);
                // Ustteki 2. satiri iki vurusluk ozel tugla yap[cite: 2, 19]
                GameObject secilenPrefab = (y == 1) ? ozelTuglaPrefab : normalTuglaPrefab;
                Instantiate(secilenPrefab, p, Quaternion.identity, transform);
            }
        }
    }
}