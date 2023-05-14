using UnityEngine;

public class AudioDialogueHandler : MonoBehaviour, IDialogueHandler
{
    [SerializeField] private AudioSource _audioSource;

    public bool IsHandling => _audioSource.isPlaying;

    public bool TryHandle(RuleEntryObject ruleEntryObject)
    {
        if (ruleEntryObject.GetContent() is not IDialogueAudioContent content ||
            content is DialogueCharacterisedContent) return false;
        _audioSource.PlayOneShot(content.AudioUnit.Audio, content.AudioUnit.GetVolumeWithVariance(), content.AudioUnit.GetPitchWithVariance());
        return true;
    }

    public void StopHandling()
    {
        _audioSource.Stop();
    }
}
