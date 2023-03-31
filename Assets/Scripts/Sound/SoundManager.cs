using UnityEngine;

/// <summary>
///     Class that manages all logic for sounds
/// </summary>
public class SoundManager : MonoPersistentSingleton<SoundManager>
{
    /// <summary>
    ///     Enum type of music
    /// </summary>
    public enum SoundType
    {
        Click,
        Shoot,
        Explode,
        Completed,
        Upgrade,
        Music
    }

    [System.Serializable]
    public class SoundVariant
    {
        public SoundType type;
        public AudioClip sound;
    }

    public SoundVariant[] sounds;

    /// <summary>
    ///     Play specific enum type sound.
    /// </summary>
    /// <param name="sound"></param>
    /// <param name="isLooping">If true then the audio will be looped and not despawn</param>
    public void PlaySound(SoundType sound, bool isLooping = false)
    {
        if (!DataManager.Instance.playerData.isAudioOn)
        {
            return;
        }

        GameObject soundGO = new GameObject(sound.ToString());
        AudioSource audioSource = soundGO.AddComponent<AudioSource>();

        AudioClip audioClip = GetAudioClip(sound);
        audioSource.clip = audioClip;
        if (isLooping)
        {
            audioSource.loop = true;
        }
        else
        {
            soundGO.AddComponent<DestroyAfterTime>().time = audioClip.length;
        }

        audioSource.Play();
    }

    /// <summary>
    ///     Trys to find the audio source of a sepcific sound type
    /// </summary>
    /// <param name="sound"></param>
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

    /// <summary>
    ///     Hacky way to find all music currently playing and stop them. This can be avoided if used coroutine for sound and then store them.
    /// </summary>
    public void StopAll()
    {
        foreach (AudioSource audio in FindObjectsOfType<AudioSource>())
        {
            audio.Stop();
        }
    }
}
