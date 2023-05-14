using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct DialogueSpeechContent : IDialogueSpeechContent
{
    [SerializeField] private SpeechDialogueUnit[] _dialogue;
    [SerializeField] private bool _skippable;

    public IEnumerable<SpeechDialogueUnit> Speech => _dialogue;
    public bool Skippable => _skippable;
}