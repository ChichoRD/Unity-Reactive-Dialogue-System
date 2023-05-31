using UnityEngine;
using UnityEngine.Events;

public class AudioDialogueHandler : MonoBehaviour, IDialogueHandler
{
    [SerializeField] private AudioSource _audioSource;

    public bool IsHandling => _audioSource.isPlaying;
    [field: SerializeField] public UnityEvent<RuleEntryObject> OnHandlingStarted { get; private set; }
    [field: SerializeField] public UnityEvent OnHandlingStopped { get; private set; }

    public bool TryHandle(RuleEntryObject ruleEntryObject)
    {
        if (ruleEntryObject.GetContent() is not IDialogueAudioContent content ||
            content is DialogueCharacterisedContent ||
            content.AudioUnit.Audio == null) return false;

        _audioSource.PlayOneShot(content.AudioUnit.Audio, content.AudioUnit.GetVolumeWithVariance(), content.AudioUnit.GetPitchWithVariance());
        OnHandlingStarted?.Invoke(ruleEntryObject);
        return true;
    }

    public void StopHandling()
    {
        _audioSource.Stop();
        OnHandlingStopped?.Invoke();
    }
}
