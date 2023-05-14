using System.Collections;
using UnityEngine;

public class TypewritingWaitInteractor : MonoBehaviour, ITypewritingInteractor
{
    public bool CanInteract(IDialogueContent content) => content is IDialogueSpeechContent;

    public IEnumerator OnTypewritingAllCoroutine(RuleEntryObject ruleEntry)
    {
        yield return null;
    }

    public IEnumerator OnTypewritingStepCoroutine(SpeechDialogueUnit speechUnit, IDialogueSpeechContent content)
    {
        yield return null;
    }

    public IEnumerator OnTypewrittenAllCoroutine(RuleEntryObject ruleEntry)
    {
        yield return null;
    }

    public IEnumerator OnTypewrittenStepCoroutine(SpeechDialogueUnit speechUnit, IDialogueSpeechContent content)
    {
        yield return new WaitForSeconds(speechUnit.WaitTime);
    }
}
