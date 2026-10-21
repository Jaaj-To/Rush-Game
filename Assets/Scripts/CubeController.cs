using Unity.Mathematics;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    [SerializeField] private float rotAngle = 90f;
    [SerializeField] private int rotUnite = 1;
    
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    private void Move(Vector3 pDir)
    {
        quaternion lRot = Quaternion.AngleAxis(rotAngle, Vector3.up);
        
    }
    
}
