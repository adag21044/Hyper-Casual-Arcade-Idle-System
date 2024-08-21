using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private Vector3 direction;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float speed = 5f;
    [SerializeField] private Vector3 cameraOffset; // Kamera ile oyuncu arasındaki sabit mesafe
    private float fixedYPosition;
    private Animator animator;

    private void Start() 
    {
        fixedYPosition = transform.position.y; 
        animator = GetComponent<Animator>();
        
        // Varsayılan olarak kamera oyuncunun biraz arkasında ve yukarısında konumlanır
        if (mainCamera != null)
            cameraOffset = mainCamera.transform.position - transform.position;
        
        animator = GetComponent<Animator>();
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
            animator.SetBool("run", true);
        else if (Input.GetMouseButtonUp(0))
            animator.SetBool("run", false);
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
