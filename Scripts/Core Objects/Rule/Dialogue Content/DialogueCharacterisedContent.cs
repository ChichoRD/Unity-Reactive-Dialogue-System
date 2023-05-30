using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct DialogueCharacterisedContent : IDialogueSpeechContent, IDialogueImageContent, IDialogueAudioContent
{
    [SerializeField] private DialogueSpeechContent _speech;
    [SerializeField] private DialogueImageContent _portrait;
    [SerializeField] private DialogueAudioContent _typingSound;

    public IEnumerable<SpeechDialogueUnit> Speech => ((IDialogueSpeechContent)_speech).Speech;
    public bool Skippable => ((IDialogueSpeechContent)_speech).Skippable;
    public ImageDialogueUnit ImageUnit => ((IDialogueImageContent)_portrait).ImageUnit;
    public AudioDialogueUnit AudioUnit => ((IDialogueAudioContent)_typingSound).AudioUnit;
}