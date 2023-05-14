using System.Collections.Generic;

public interface IDialogueSpeechContent : IDialogueContent
{
    IEnumerable<SpeechDialogueUnit> Speech { get; }
    bool Skippable { get; }
}
