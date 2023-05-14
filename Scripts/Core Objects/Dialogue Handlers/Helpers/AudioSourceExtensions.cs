using System.Threading.Tasks;
using UnityEngine;

public static class AudioSourceExtensions
{
    public static async void PlayOneShot(this AudioSource audioSource, AudioClip clip, float pitch = 1.0f)
    {
        float basePitch = audioSource.pitch;
        float clipDuration = clip.length / Mathf.Abs(pitch);

        audioSource.pitch = pitch;
        audioSource.PlayOneShot(clip);

        await Task.Delay((int)(clipDuration * 1000));

        audioSource.pitch = basePitch;
    }

    public static async void PlayOneShot(this AudioSource audioSource, AudioClip clip, float volume = 1.0f, float pitch = 1.0f)
    {
        float basePitch = audioSource.pitch;
        float clipDuration = clip.length / Mathf.Abs(pitch);

        audioSource.pitch = pitch;
        audioSource.PlayOneShot(clip, volume);

        await Task.Delay((int)(clipDuration * 1000));

        audioSource.pitch = basePitch;
    }
}