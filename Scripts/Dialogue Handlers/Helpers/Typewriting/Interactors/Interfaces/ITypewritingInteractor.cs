using System.Collections;

public interface ITypewritingInteractor : IDialogueContentInteractor
{
    IEnumerator OnTypewritingAllCoroutine(RuleEntryObject ruleEntry, IDialogueSpeechContent content);
    IEnumerator OnTypewritingStepCoroutine(SpeechDialogueUnit speechUnit, IDialogueSpeechContent content);
    IEnumerator OnTypewrittenStepCoroutine(SpeechDialogueUnit speechUnit, IDialogueSpeechContent content);
    IEnumerator OnTypewrittenAllCoroutine(RuleEntryObject ruleEntry, IDialogueSpeechContent content);
}
