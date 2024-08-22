using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

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
        var counter = 0;
        var DollarPlaceIndex = 0;

        yield return new WaitForSecondsRealtime(2);

        while (counter < PaperPlace.childCount)
        {
            GameObject NewDollar = Instantiate(Dollar, new Vector3(DollarPlace.GetChild(DollarPlaceIndex).position.x,
                    YAxis, DollarPlace.GetChild(DollarPlaceIndex).position.z),
                DollarPlace.GetChild(DollarPlaceIndex).rotation);

            NewDollar.transform.DOScale(new Vector3(0.4f, 0.4f, 0.6f), 0.5f).SetEase(Ease.OutElastic);

            if (DollarPlaceIndex < DollarPlace.childCount - 1)
            {
                DollarPlaceIndex++;
            }
            else
            {
                DollarPlaceIndex = 0;
                YAxis += 0.5f;
            }

            yield return new WaitForSecondsRealtime(3f);
        }
    }

    void DOSubmitPapers()
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

            var Desk = transform.parent;
            Desk.GetChild(Desk.childCount - 1).GetComponent<Renderer>().enabled = true;

            StopCoroutine(makeMoneyIE);
            YAxis = 0f;
            CancelInvoke("DOSubmitPapers");
        }
    }
}
