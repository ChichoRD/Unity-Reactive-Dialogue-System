using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputtedTypewritingWaitInteractor : MonoBehaviour, ITypewritingInteractor
{
    [RequireInterface(typeof(ITypewritingInteractor))]
    [SerializeField] private Object beforeInputInteractorObject;
    private ITypewritingInteractor BeforeInputInteractor => beforeInputInteractorObject as ITypewritingInteractor;
    [SerializeField] private InputActionReference _continueTypewritingAction;

    public bool CanInteract(IDialogueContent content)
    {
        return BeforeInputInteractor == null ? content is IDialogueSpeechContent : BeforeInputInteractor.CanInteract(content);
    }

    public IEnumerator OnTypewritingAllCoroutine(RuleEntryObject ruleEntry)
    {
        yield return BeforeInputInteractor?.OnTypewritingAllCoroutine(ruleEntry);
    }

    public IEnumerator OnTypewritingStepCoroutine(SpeechDialogueUnit speechUnit, IDialogueSpeechContent content)
    {
        yield return BeforeInputInteractor?.OnTypewritingStepCoroutine(speechUnit, content);
    }

    public IEnumerator OnTypewrittenAllCoroutine(RuleEntryObject ruleEntry)
    {
        yield return BeforeInputInteractor?.OnTypewrittenAllCoroutine(ruleEntry);
    }

    public IEnumerator OnTypewrittenStepCoroutine(SpeechDialogueUnit speechUnit, IDialogueSpeechContent content)
    {
        bool previousState = _continueTypewritingAction.action.enabled;
        SetContinuingActionState(true);

        yield return BeforeInputInteractor?.OnTypewrittenStepCoroutine(speechUnit, content);
        yield return new WaitUntil(_continueTypewritingAction.action.IsPressed);

        SetContinuingActionState(previousState);
    }

    private void SetContinuingActionState(bool enable)
    {
        if (enable)
        {
            _continueTypewritingAction.action.Enable();
            return;
        }
        _continueTypewritingAction.action.Disable();
    }
}
