using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }
    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}
