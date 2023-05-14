using System;
using UnityEngine;

[Serializable]
public struct ImageDialogueUnit
{
    [field: SerializeField] public Sprite Image { get; private set; }
    [field: SerializeField] public bool UseShowTime { get; private set; }
    [field: SerializeField][field: Min(0.0f)] public float ShowTime { get; private set; }
}
