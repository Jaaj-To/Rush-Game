using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Image = UnityEngine.UI.Image;

namespace UI
{
    public class HUDTile : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text count;

        private float stockDir;
        private Transform cameraTransform;
        public AltStock Stock { get; private set; }

        private void Awake()
        {
            cameraTransform = Camera.main?.transform;
        }

        public void SetTile(AltStock pStock, int pCount)
        {
            Stock = pStock;
            icon.sprite = pStock.Icon;
            count.text = pCount.ToString();
            stockDir = Mathf.Atan2(pStock.GlobalDir.x, pStock.GlobalDir.z) * Mathf.Rad2Deg;
        }

        private void LateUpdate()
        {
            float lAngle = cameraTransform.eulerAngles.y - stockDir;
            icon.rectTransform.localEulerAngles = new Vector3(0f, 0f, lAngle);
        }
    }
}
