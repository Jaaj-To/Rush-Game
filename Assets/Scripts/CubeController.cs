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
}
