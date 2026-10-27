using UnityEngine;
using UnityEngine.Serialization;

public class CameraController : MonoBehaviour
{
    private Vector3 distanceToCenter;
    private float longTheta;
    private float latPhi;
    [SerializeField] private float radius = 20f; 
    [SerializeField] private float cameraSpeed = 2f;
    
    void Start()
    {
        transform.position = new Vector3(0, 0, -radius);
        distanceToCenter = transform.position;
    }
    
    void Update()
    {
        longTheta += Input.GetAxis("Horizontal") * cameraSpeed  * Time.deltaTime;
        latPhi -= Input.GetAxis("Vertical") * cameraSpeed * Time.deltaTime;
        latPhi = Mathf.Clamp(latPhi, -45f, 90f);

        Quaternion lCamRot = Quaternion.Euler(latPhi, longTheta, 0);
        transform.rotation = lCamRot;
        
        transform.position = lCamRot * distanceToCenter ;
    }
}
