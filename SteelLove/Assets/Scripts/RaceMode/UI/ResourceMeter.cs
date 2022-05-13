using UnityEngine;
using UnityEngine.UI;

public class ResourceMeter : MonoBehaviour
{
    [SerializeField] private Image _boostBarImage;

    [Header("Listening To")]
    [SerializeField] private FloatEventChannelSO _onMeterValueChanged;

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
        var clampValue = Mathf.Clamp(newLevel / 100.0f, 0f, 1f);
        _boostBarImage.fillAmount = clampValue;
    }
}
