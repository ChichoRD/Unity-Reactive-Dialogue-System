using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct DialogueCharacterisedBranchContent : IDialogueSpeechContent, IDialogueImageContent, IDialogueAudioContent, IDialogueBranchContent
{
    [SerializeField] private DialogueCharacterisedContent _characterisedContent;
    [SerializeField] private DialogueBranchContent _branchContent;

    public IEnumerable<SpeechDialogueUnit> Speech => ((IDialogueSpeechContent)_characterisedContent).Speech;
    public bool Skippable => ((IDialogueSpeechContent)_characterisedContent).Skippable;
    public ImageDialogueUnit ImageUnit => ((IDialogueImageContent)_characterisedContent).ImageUnit;
    public AudioDialogueUnit AudioUnit => ((IDialogueAudioContent)_characterisedContent).AudioUnit;
    public IEnumerable<BranchingDialogueUnit> Branches => ((IDialogueBranchContent)_branchContent).Branches;
}