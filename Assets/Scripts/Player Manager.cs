using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    private Vector3 direction;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float speed = 5f;
    [SerializeField] private Vector3 cameraOffset; // Kamera ile oyuncu arasındaki sabit mesafe
    private float fixedYPosition;
    private Animator animator;
    [SerializeField] public Transform PaperPlace;
    [SerializeField] private List<Transform> papers = new List<Transform>();
    private float YAxis;
    private float delay = 0.2f;
    [SerializeField] private TextMeshProUGUI moneyCounter;
    public int dollar = 0;
    
    private bool isPlacingPapers = false; // Kağıt yerleştirme durumu kontrolü

    private void Start() 
    {
        fixedYPosition = transform.position.y; 
        animator = GetComponent<Animator>();

        if (mainCamera != null)
            cameraOffset = mainCamera.transform.position - transform.position;
    }

    private void Update() 
    {
        if (isPlacingPapers) return; // Eğer kağıt yerleştiriliyorsa inputları durdur

        if (Input.GetMouseButton(0))
        {
            Plane groundPlane = new Plane(Vector3.up, transform.position);
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (groundPlane.Raycast(ray, out var distance)) 
                direction = ray.GetPoint(distance);

            Vector3 newPosition = new Vector3(direction.x, fixedYPosition, direction.z);
            transform.position = Vector3.MoveTowards(transform.position, newPosition, speed * Time.deltaTime);

            var offset = direction - transform.position;

            if (offset.magnitude > 1f)
                transform.LookAt(direction);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (papers.Count > 1)
            {
                animator.SetBool("carry", false);
                animator.SetBool("RunWithPapers", true);
            }
            else
            {
                animator.SetBool("run", true);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            animator.SetBool("run", false);

            if (papers.Count > 1)
            {
                animator.SetBool("carry", true);
                animator.SetBool("RunWithPapers", false);
            }
        }

        if (papers.Count > 1)
        {
            for (int i = 1; i < papers.Count; i++)
            {
                var firstPaper = papers.ElementAt(i - 1);
                var secondPaper = papers.ElementAt(i);

                secondPaper.position = new Vector3(
                    Mathf.Lerp(secondPaper.position.x, firstPaper.position.x, Time.deltaTime * 15f),
                    Mathf.Lerp(secondPaper.position.y, firstPaper.position.y, Time.deltaTime * 15f),
                    firstPaper.position.z);
            }
        }

        if (Physics.Raycast(transform.position, transform.forward, out var hit, 2f))
        {
            Debug.DrawRay(transform.position, transform.forward, Color.red);
            if (hit.collider.CompareTag("table") && papers.Count < 21)
            {   
                if (hit.collider.transform.childCount > 2)
                {
                    var paper = hit.collider.transform.GetChild(1);

                    papers.Add(paper);
                    paper.parent = PaperPlace;
                }

                animator.SetBool("carry", true);
                animator.SetBool("RunWithPapers", true);
                animator.SetBool("run", false);
            }

            if (hit.collider.CompareTag("pp") && papers.Count > 1)
            {
                Debug.Log("pp");
                var workDesk = hit.collider.transform;
                isPlacingPapers = true; // Kağıt yerleştirme başlıyor, inputlar durdurulmalı

                if (workDesk.childCount > 0) 
                    YAxis = workDesk.GetChild(workDesk.childCount - 1).position.y + 0.17f;
                else 
                    YAxis = workDesk.position.y;

                for (var index = papers.Count - 1; index >= 1; index--)
                {
                    papers[index].DOJump(new Vector3(workDesk.position.x, YAxis, workDesk.position.z), 2f, 1, 0.2f).SetDelay(delay).SetEase(Ease.Flash);
                    papers.ElementAt(index).parent = workDesk;
                    papers.RemoveAt(index);

                    YAxis += 0.17f;
                    delay += 0.02f;
                }

                workDesk.parent.GetChild(workDesk.parent.childCount - 1).GetComponent<Renderer>().enabled = false;

                if (papers.Count <= 1)
                {
                    animator.SetBool("carry", false);
                    animator.SetBool("RunWithPapers", false);
                    animator.SetBool("idle", true); // idle animation
                }

                isPlacingPapers = false; // Kağıt yerleştirme bitti, inputlar tekrar aktif
            }
        }
        else
        {
            Debug.DrawRay(transform.position, Vector3.down + transform.forward, Color.red);
        }
    }

    private void LateUpdate()
    {
        if (mainCamera != null)
        {
            mainCamera.transform.position = transform.position + cameraOffset;
            mainCamera.transform.LookAt(transform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("table"))
        {
            if (papers.Count > 1)
            {
                animator.SetBool("carry", false);
                animator.SetBool("RunWithPapers", true);
            }
            else
            {
                animator.SetBool("run", true);
            }
        }

        if (other.CompareTag("pp"))
        {
            other.GetComponent<WorkDesk>().Work();
        }

        if(other.CompareTag("dollar"))
        {
            Debug.Log("Dollar++");
            Destroy(other.gameObject);
            dollar += 5;
            moneyCounter.text = "$"+dollar.ToString();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("table"))
        {
            if (papers.Count > 1)
            {
                animator.SetBool("carry", false);
                animator.SetBool("RunWithPapers", true);
            }
            else
            {
                animator.SetBool("run", false);
            }
        }
    }
}
