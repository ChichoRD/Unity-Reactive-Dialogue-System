public interface ISpeechUnitAnalyser
{
    SpeechDialogueUnit Analyse(SpeechDialogueUnit speechDialogueUnit);
    ISpeechUnitAnalyser Next { get; }
}
