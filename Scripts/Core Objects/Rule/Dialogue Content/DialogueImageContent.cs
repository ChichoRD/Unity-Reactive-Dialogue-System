using System;
using UnityEngine;

[Serializable]
public struct DialogueImageContent : IDialogueImageContent
{
    [SerializeField] private ImageDialogueUnit _imageUnit;
    public ImageDialogueUnit ImageUnit => _imageUnit;
}
