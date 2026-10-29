using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class CameraController : MonoBehaviour
{
    private Vector3 distanceToCenter;
    private float longTheta;
    private float latPhi;
    private float minClamp = -45f;
    private float maxClamp = 90f;
    private Vector3 lastMousePos;
    [SerializeField] private float radius = 20f; 
    [SerializeField] private float cameraSpeed = 2f;
    
    void Start()
    {
        transform.position = new Vector3(0, 0, -radius);
        distanceToCenter = transform.position;
    }
    
    void LateUpdate()
    {
        longTheta -= Input.GetAxis("Horizontal") * cameraSpeed  * Time.deltaTime;
        latPhi += Input.GetAxis("Vertical") * cameraSpeed * Time.deltaTime;
        Vector3 lMouseDelta = Input.mousePosition - lastMousePos;
        lastMousePos = Mouse.current.position.ReadValue();

        if (Input.GetMouseButton(1))
        {
            longTheta += lMouseDelta.x * cameraSpeed * Time.deltaTime;
            latPhi -= lMouseDelta.y * cameraSpeed * Time.deltaTime;
        } 
        
        latPhi = Mathf.Clamp(latPhi, minClamp, maxClamp);

        Quaternion lCamRot = Quaternion.Euler(latPhi, longTheta, 0);
        transform.rotation = lCamRot;
        
        transform.position = lCamRot * distanceToCenter ;
    }
}
