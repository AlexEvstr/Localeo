using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonSoundPlayer : MonoBehaviour
{
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {   
        if (_button == null)
            _button = GetComponent<Button>();

        _button.onClick.RemoveListener(PlayClick);
        _button.onClick.AddListener(PlayClick);
    }

    private void OnDisable()
    {
        if (_button != null)
            _button.onClick.RemoveListener(PlayClick);
    }

    private void PlayClick()
    {
        if (SFXManager.Instance == null)
            Debug.LogWarning("SFXManager.Instance is null");
        else
            SFXManager.Instance.PlayClickSound();
    }
}
