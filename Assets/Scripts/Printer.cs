using System.Collections;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class Printer : MonoBehaviour
{
    [SerializeField] private Transform[] PapersPlace = new Transform[10];
    [SerializeField] private GameObject paper;
    [SerializeField] private float paperDeliveryTime;
    [SerializeField] private float yAxis;
    private List<Transform> papers = new List<Transform>();
    [SerializeField] private Transform paperPlace;


    private void Start()
    {
        for(int i = 0; i < PapersPlace.Length; i++)
        {
            PapersPlace[i] = transform.GetChild(0).GetChild(i);
        }

        StartCoroutine(PrintPaper(paperDeliveryTime));

        papers.Add(paperPlace);
    }

    public IEnumerator PrintPaper(float time)
    {
        int countPapers = 0;
        int ppIndex = 0;

        while(countPapers < 100)
        {
            GameObject NewPaper = Instantiate(paper, new Vector3(transform.position.x, -3f, transform.position.z), Quaternion.identity,transform.GetChild(1));
            NewPaper.transform.DOJump(new Vector3(PapersPlace[ppIndex].position.x,
                                      PapersPlace[ppIndex].position.y + yAxis, 
                                      PapersPlace[ppIndex].position.z), 2f, 1, 0.5f).SetEase(Ease.OutQuad);

            if(ppIndex < 9)
            {
                ppIndex++;
            }   
            else
            {
                ppIndex = 0;
                yAxis += 0.05f;
            }
                
            yield return new WaitForSecondsRealtime(time);

        }

    }
}
