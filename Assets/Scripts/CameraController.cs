using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 pivotOffset = Vector3.up;
    [SerializeField] private float radius = 20f;

    [SerializeField] private float keyboardSpeed = 90f;
    [SerializeField] private float mouseSensitivity = 0.2f;
    [SerializeField] private float rotSmoothing = 10f;

    [SerializeField] private float startLongitude = 0f;
    [SerializeField] private float startLatitude = 30f;
    [SerializeField] private float minLatitude = 10f;
    [SerializeField] private float maxLatitude = 85f;

    private float longTheta;
    private float latPhi;
    private Quaternion currentRotation;

    private void Start()
    {
        longTheta = startLongitude;
        latPhi = startLatitude;
        currentRotation = Quaternion.Euler(latPhi, longTheta, 0f);
        PlaceCamera();
    }

    private void LateUpdate()
    {
        longTheta -= Input.GetAxis("Horizontal") * keyboardSpeed * Time.deltaTime;
        latPhi += Input.GetAxis("Vertical") * keyboardSpeed * Time.deltaTime;

        if (Input.GetMouseButton(1) && Mouse.current != null)
        {
            Vector2 lMouseDelta = Mouse.current.delta.ReadValue();
            longTheta += lMouseDelta.x * mouseSensitivity;
            latPhi -= lMouseDelta.y * mouseSensitivity;
        }

        latPhi = Mathf.Clamp(latPhi, minLatitude, maxLatitude);

        Quaternion lTargetRotation = Quaternion.Euler(latPhi, longTheta, 0f);
        currentRotation = Quaternion.Slerp(currentRotation, lTargetRotation, rotSmoothing * Time.deltaTime);
        
        PlaceCamera();
    }

    private void PlaceCamera()
    {
        Vector3 lPivot = target.position + pivotOffset;
        transform.rotation = currentRotation;
        transform.position = lPivot + currentRotation * new Vector3(0f, 0f, -radius);
    }
}
