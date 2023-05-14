using System;
using UnityEngine;

[Serializable]
public struct DialogueAudioContent : IDialogueAudioContent
{
    [SerializeField] private AudioDialogueUnit _audioUnit;
    public AudioDialogueUnit AudioUnit => _audioUnit;
}