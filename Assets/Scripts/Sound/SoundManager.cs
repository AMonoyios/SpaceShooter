using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoPersistentSingleton<SoundManager>
{
    public enum SoundType
    {
        Click,
        Shoot,
        Explode,
        Completed,
        Upgrade
    }

    [System.Serializable]
    public class SoundVariant
    {
        public SoundType type;
        public AudioClip sound;
    }

    public SoundVariant[] sounds;

    public void PlaySound(SoundType sound)
    {
        if (!DataManager.Instance.playerData.isAudioOn)
        {
            return;
        }

        GameObject soundGO = new GameObject(sound.ToString());

        AudioSource audioSource = soundGO.AddComponent<AudioSource>();
        AudioClip audioClip = GetAudioClip(sound);
        soundGO.AddComponent<DestroyAfterTime>().time = audioClip.length;
        audioSource.PlayOneShot(audioClip);
    }

    private AudioClip GetAudioClip(SoundType sound)
    {
        foreach (SoundVariant soundVariant in sounds)
        {
            if (soundVariant.type == sound)
            {
                if (soundVariant.sound != null)
                {
                    return soundVariant.sound;
                }

                Debug.LogError($"Sound reference for type {sound} is null!");
            }
        }

        Debug.LogError($"Sound for type {sound} was not found!");
        return null;
    }
}
