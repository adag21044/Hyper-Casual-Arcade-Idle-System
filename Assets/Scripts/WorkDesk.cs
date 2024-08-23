using System.Collections;
using UnityEngine;
using DG.Tweening;

public class WorkDesk : MonoBehaviour
{
    public Animator female_anim;
    [SerializeField] private Transform DollarPlace;
    [SerializeField] private GameObject Dollar;
    [SerializeField] private Transform PaperPlace; // PaperPlace (WorkDesk) referansı
    private float YAxis;
    private IEnumerator makeMoneyIE;
    private bool isWorking = false;

    private void Start()
    {
        makeMoneyIE = MakeMoney();
    }

    private void Update()
    {
        // PaperPlace'in child sayısını sürekli kontrol eder ve duruma göre Work veya StopWork çalıştırılır.
        if (PaperPlace.childCount > 0 && !isWorking)
        {
            Work();
        }
        else if (PaperPlace.childCount == 0 && isWorking)
        {
            StopWork();
        }
    }

    public void Work()
    {
        if (!isWorking) // Eğer zaten çalışıyorsa tekrar başlatma
        {
            Debug.Log("Female anim is working");
            female_anim.SetBool("work", true);
            isWorking = true;
            InvokeRepeating("DOSubmitPapers", 2f, 1f);
            StartCoroutine(makeMoneyIE);
        }
    }

    private IEnumerator MakeMoney()
    {
        int DollarPlaceIndex = 0;
        yield return new WaitForSecondsRealtime(2);

        while (PaperPlace.childCount > 0)
        {
            // Zemin Yüksekliğini Ayarlayın
            float yOffset = Dollar.GetComponent<Renderer>().bounds.extents.y; // Objeyi tam zemine yerleştirmek için gereken offset

            GameObject NewDollar = Instantiate(Dollar, new Vector3(DollarPlace.GetChild(DollarPlaceIndex).position.x,
                                            DollarPlace.GetChild(DollarPlaceIndex).position.y + yOffset, // Yüksekliği düzenledik
                                            DollarPlace.GetChild(DollarPlaceIndex).position.z),
                                            DollarPlace.GetChild(DollarPlaceIndex).rotation);

            // Objeyi uygun boyutta ölçeklendirme
            NewDollar.transform.DOScale(new Vector3(0.1f, 0.1f, 0.15f), 0.5f).SetEase(Ease.OutElastic);

            if (DollarPlaceIndex < DollarPlace.childCount - 1)
            {
                DollarPlaceIndex++;
            }
            else
            {
                DollarPlaceIndex = 0;
                YAxis += 0.2f;
            }

            yield return new WaitForSecondsRealtime(3f);
        }
    }

    private void DOSubmitPapers()
    {
        if (PaperPlace.childCount > 0)
        {
            Destroy(PaperPlace.GetChild(PaperPlace.childCount - 1).gameObject, 1f);
        }
    }

    private void StopWork()
    {
        if (isWorking)
        {
            female_anim.SetBool("work", false);
            isWorking = false;

            // Para oluşturma sürecini durdur
            StopCoroutine(makeMoneyIE);
            YAxis = 0f;
            CancelInvoke("DOSubmitPapers");
        }
    }
}
