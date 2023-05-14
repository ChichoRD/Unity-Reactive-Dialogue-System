using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class TypewritingSoundDialogueHandler : MonoBehaviour, IDialogueHandler
{
    private readonly HashSet<char> _muteSounds = new() { ' ', '\n', '\t' };

    [RequireInterface(typeof(ITypewriter))]
    [SerializeField] private Object _typewriterObject;
    private ITypewriter Typewriter => _typewriterObject as ITypewriter;
    [SerializeField] private AudioSource _audioSource;
    private AudioDialogueUnit _currentTypingSound;

    public bool IsHandling => _audioSource.isPlaying;

    public bool TryHandle(RuleEntryObject ruleEntryObject)
    {
        if (ruleEntryObject.GetContent() is not IDialogueAudioContent content ||
            content is not DialogueCharacterisedContent) return false;

        _currentTypingSound = content.AudioUnit;
        Typewriter.OnTyped.RemoveListener(PlayTypingSound);
        Typewriter.OnTyped.AddListener(PlayTypingSound);

        return true;
    }

    public void StopHandling()
    {
        _audioSource.Stop();
    }

    private void PlayTypingSound(StringBuilder stringBuilder)
    {
        var typedText = stringBuilder.ToString();
        if (string.IsNullOrEmpty(typedText) || _muteSounds.Contains(typedText[^1])) return;

        _audioSource.PlayOneShot(_currentTypingSound.Audio, _currentTypingSound.GetVolumeWithVariance(), _currentTypingSound.GetPitchWithVariance());
    }
}
