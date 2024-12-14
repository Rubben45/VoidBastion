using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }

    [SerializeField] private AudioSource sfxAudioSource;
    [SerializeField] private AudioClip clickClip, getDamage; 
    [FormerlySerializedAs("enemyHit")] [SerializeField] private AudioClip shootClip;
    [FormerlySerializedAs("goalSound")] [SerializeField] private AudioClip magFieldSound;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip assassinSound;

    private Dictionary<SoundType, AudioClip> soundEffects;
    
    private enum SoundType { Click, GetDamage, ShootSound, MagField, AssassinSound, HitSound };
    
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
        
        soundEffects = new Dictionary<SoundType, AudioClip>();
    }
    private void Start()
    {
        AddSoundEffect(SoundType.Click, clickClip);
        AddSoundEffect(SoundType.GetDamage, getDamage);
        AddSoundEffect(SoundType.ShootSound, shootClip);
        AddSoundEffect(SoundType.MagField, magFieldSound);
        AddSoundEffect(SoundType.AssassinSound, assassinSound);
        AddSoundEffect(SoundType.HitSound, hitSound);
    }

    // Add a new sound effect to the dictionary
    private void AddSoundEffect(SoundType soundType, AudioClip clip)
    {
        soundEffects[soundType] = clip;
    }

    // Play a sound effect by its name
    private void PlaySoundEffect(SoundType soundType)
    {
        if (soundEffects.TryGetValue(soundType, out var clip))
        {
            sfxAudioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("Sound effect not found: " + soundType);
        }
    }

    private void SetSFXVolume(float volume)
    {
        sfxAudioSource.volume = volume;
    }

    public void ClickSound()
    {
        PlaySoundEffect(SoundType.Click);
    }

    public void GetDamageSound()
    {
        PlaySoundEffect(SoundType.GetDamage);
    }

    public void ShootSound()
    {
        PlaySoundEffect(SoundType.ShootSound);
    }

    public void MagFieldSound()
    {
        PlaySoundEffect(SoundType.MagField);
    }

    public void AssassinSound()
    {
        PlaySoundEffect(SoundType.AssassinSound);
    }

    public void PlayHitSound()
    {
        PlaySoundEffect(SoundType.HitSound);
    }
    
}
