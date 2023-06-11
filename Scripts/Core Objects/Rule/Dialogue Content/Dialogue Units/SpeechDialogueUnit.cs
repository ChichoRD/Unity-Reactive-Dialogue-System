using System;
using UnityEngine;

[Serializable]
public struct SpeechDialogueUnit
{
    public SpeechDialogueUnit(string message, float waitTime, bool overrideTypingSpeed, float typingSpeed)
    {
        Message = message;
        WaitTime = waitTime;
        OverrideTypingSpeed = overrideTypingSpeed;
        TypingSpeed = typingSpeed;
    }

    [field: SerializeField] [field: TextArea] public string Message { get; private set; }
    [field: SerializeField][field: Min(0.0f)] public float WaitTime { get; private set; }
    [field: SerializeField] public bool OverrideTypingSpeed { get; private set; }
    [field: SerializeField] [field: Min(0.0f)] public float TypingSpeed { get; private set; }
}