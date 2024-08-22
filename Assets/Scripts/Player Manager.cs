using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayerManager : MonoBehaviour
{
    private Vector3 direction;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float speed = 5f;
    [SerializeField] private Vector3 cameraOffset; // Kamera ile oyuncu arasındaki sabit mesafe
    private float fixedYPosition;
    private Animator animator;
    [SerializeField]public Transform PaperPlace;
    [SerializeField]private List<Transform> papers = new List<Transform>();
    
    

    private void Start() 
    {
        fixedYPosition = transform.position.y; 
        animator = GetComponent<Animator>();
        
        // Varsayılan olarak kamera oyuncunun biraz arkasında ve yukarısında konumlanır
        if (mainCamera != null)
            cameraOffset = mainCamera.transform.position - transform.position;
        
        
    }

    private void Update() 
    {
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
                animator.SetBool("carry",false);
                animator.SetBool("RunWithPapers",true);
            }
            else
            {
                animator.SetBool("run",true);
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            animator.SetBool("run",false);

            if (papers.Count > 1)
            {
                animator.SetBool("carry",true);
                animator.SetBool("RunWithPapers",false);
            }

        }
            


        if(papers.Count > 1)
        {
            for(int i = 1; i < papers.Count; i++)
            {
                var firstPaper = papers.ElementAt(i-1);
                var secondPaper = papers.ElementAt(i);

                secondPaper.position = new Vector3(Mathf.Lerp(secondPaper.position.x, firstPaper.position.x, Time.deltaTime * 15f),
                                                Mathf.Lerp(secondPaper.position.y, firstPaper.position.y, Time.deltaTime * 15f),
                                                firstPaper.position.z);
            }
        }
            
        if(Physics.Raycast(transform.position,  transform.forward, out var hit, 2f))
        {
            Debug.DrawRay(transform.position,  transform.forward, Color.red);
            if(hit.collider.CompareTag("table") && papers.Count < 21)
            {   
                if(hit.collider.transform.childCount > 2)
                {
                    var paper = hit.collider.transform.GetChild(1);
                    Debug.Log("Paper Name: " + paper.name);
                    papers.Add(paper);
                    paper.parent = PaperPlace;
                }
                
                animator.SetBool("carry",true);
                animator.SetBool("RunWithPapers",true);
                animator.SetBool("run",false);
            }
        }
        else
        {
            Debug.DrawRay(transform.position, Vector3.down + transform.forward, Color.red);
        }

    }

    private void LateUpdate()
    {
        // Kamera, oyuncunun hareketine göre güncellenir ve offset sabit kalır
        if (mainCamera != null)
        {
            mainCamera.transform.position = transform.position + cameraOffset;
            mainCamera.transform.LookAt(transform.position); // Kamera sürekli oyuncuya bakar
        }
    }
}
