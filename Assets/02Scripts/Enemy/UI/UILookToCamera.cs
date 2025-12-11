using UnityEngine;

public class UILookToCamera : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    private Vector3 direction;
    // Update is called once per frame
    void Update()
    {
        direction = (transform.position - mainCamera.transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
