using UnityEngine;
using UnityEngine.UI;

public class ResourceMeter : MonoBehaviour
{
    [SerializeField] private Image _boostBarImage;

    [Header("Listening To")]
    [SerializeField] private FloatEventChannelSO _onMeterValueChanged;

    public float maxValue = 100;

    private void OnEnable()
    {
        _onMeterValueChanged.OnEventRaised += UpdateResourceMeter;
    }

    private void OnDisable()
    {
        _onMeterValueChanged.OnEventRaised -= UpdateResourceMeter;
    }

    private void UpdateResourceMeter(float newLevel)
    {
        var clampValue = Mathf.Clamp(newLevel / maxValue, 0f, 1f);
        _boostBarImage.fillAmount = clampValue;
    }
}
