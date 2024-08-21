using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private Vector3 targetPosition;
    private Camera mainCamera;
    [SerializeField] private float speed = 5f;
    private float fixedYPosition;

    private void Start() 
    {
        mainCamera = Camera.main;
        fixedYPosition = transform.position.y; 
    }

    private void Update() 
    {
        if (Input.GetMouseButton(0))
        {
            Plane groundPlane = new Plane(Vector3.up, transform.position);
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (groundPlane.Raycast(ray, out var distance)) 
                targetPosition = ray.GetPoint(distance);

            
            Vector3 newPosition = new Vector3(targetPosition.x, fixedYPosition, targetPosition.z);
            transform.position = Vector3.MoveTowards(transform.position, newPosition, speed * Time.deltaTime);
        }
    }
}
