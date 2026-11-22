using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TickDurSlider : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    [SerializeField] private Slider slider;

    [SerializeField] private TMP_Text label;

    private void Start()
    {
        slider.minValue = GameManager.MIN_TICK_DURATION;
        slider.maxValue = GameManager.MAX_TICK_DURATION;
        slider.value = gameManager.TickDuration;
        slider.onValueChanged.AddListener(OnSliderChanged);
        UpdateLabel();
    }

    private void OnSliderChanged(float pValue)
    {
        gameManager.SetTickDuration(pValue);
        UpdateLabel();
    }

    private void UpdateLabel()
    {
        if (label != null)
        {
            label.text = "Tick : " + gameManager.TickDuration.ToString("0.0") + " s";
        }
    }
}
