using System;
using System.Diagnostics;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public enum RollDirection { PosX, NegX, PosZ, NegZ }    

public class CubeController : MonoBehaviour
{
    [SerializeField] private float rotAngle = 90f;
    [SerializeField] private int rotUnite = 1;
    [SerializeField] private float rotTime = 1.5f;

    private Sequence rollSequence;

    private void Move(RollDirection pDir)
    {
        if (rollSequence != null && rollSequence.IsActive()) return;
        
        Vector3 lWorldDir = transform.TransformDirection(ToLocalVector(pDir));
        
        Vector3 lAxis = Vector3.Cross(Vector3.up, lWorldDir);
        
    }

    private Vector3 ToLocalVector(RollDirection pDir) => pDir switch
    {
        RollDirection.PosX => Vector3.right,
        RollDirection.NegX => Vector3.left,
        RollDirection.PosZ => Vector3.forward,
        RollDirection.NegZ => Vector3.back,
        _ => throw new System.ArgumentOutOfRangeException(nameof(pDir), pDir, null)
    };

}
