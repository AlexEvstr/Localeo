using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clickClip;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void PlayClickSound()
    {
        if (clickClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickClip);
        }
    }
}
