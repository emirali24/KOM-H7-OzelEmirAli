# KOM-H7-OzelEmirAli — Üç Mekanik, Dört Oyun

İstanbul Nişantaşı Üniversitesi — Dijital Oyun Tasarımı  
**Ders:** Karmaşık Oyun Mekanikleri (Hafta 7 Ödevi)  
**Öğrenci:** Emir Ali Özel (20246000047)

## Sahneler Nasıl Açılır?
Unity 6 (2D URP) ile proje açıldıktan sonra `Assets/Scenes/` klasöründen ilgili sahneye çift tıklanıp **Play** tuşuna basılır. (`D_Birlesik` oyununun doğrudan çalıştırılabilir Windows sürümü `Build/D_Birlesik/` klasöründedir.)

## Sahneler ve Kontroller

1. **`A_TekTus.unity` (Tek Tuş Mekaniği):**
   - **Kontroller:** `Space` veya `Sol Fare Tıkı`
   - **Oynanış:** Kısa basış ile alçak zıplama, basılı tutma ile yüksek zıplama; havadayken tekrar basılı tutarak hızlı dalış. Her engelde hız artar. Engele çarpınca aynı tuşla yeniden başlar.

2. **`B_HareketHissi.unity` (Hareket ve Oyun Hissi):**
   - **Kontroller:** `A / D` veya `Sol / Sağ Ok` (Hareket), `Space` (Zıplama), `Tab` (Profil Değiştir), `R` (Yeniden Başlat)
   - **Oynanış:** İvme, fren, değişken zıplama yüksekliği, *coyote time* ve *zıplama tamponu* içerir. `Tab` tuşuyla **Tüy (Hafif)** ve **Taş (Ağır)** profilleri arasında geçiş yapılır.

3. **`C_Carpisma.unity` (Çarpışma ve Fizik Tepkisi):**
   - **Kontroller:** `A / D` veya `Sol / Sağ Ok` (Raket), `Space` (Topu Fırlat), `R` (Yeniden Başlat)
   - **Oynanış:** Topun rakete çarpma noktasına göre sekme açısı hesaplanır. Normal tuğlalar ve 2 vuruşluk özel tuğlalar bulunur. Özel tuğlalardan düşen tetikleyici (`Is Trigger`) kristaller topu hızlandırır.

4. **`D_Birlesik.unity` (Birleşik Oyun — Kinetik Çekirdek):**
   - **Kontroller:** `A / D` veya `Sol / Sağ Ok` (Platform hareketi), `Space` (Yerde zıpla; havada basılı tutup bırakarak Kinetik Darbe fırlat)
   - **Oynanış:** Yatay hız sekme açısını belirler; havada şarj edilen Kinetik Darbe zırhlı tuğlaları tek vuruşta kırar; kırılan tuğlalar karakterin azami hızını artırır ve darbe hakkını yeniler.