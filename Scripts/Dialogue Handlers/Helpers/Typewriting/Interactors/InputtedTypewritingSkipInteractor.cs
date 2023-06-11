using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputtedTypewritingSkipInteractor : MonoBehaviour, ITypewritingInteractor
{
    [SerializeField] private bool _skipTypewritingInstantly;
    [SerializeField] [Min(float.Epsilon)] private float _skipTime = 0.5f;
    [SerializeField] private InputActionReference _skipTypewritingAction;
    private bool _previousActionState;
    private float _previousTypingSpeed;
    private string _currentDialogueText;

    [RequireInterface(typeof(ITypewritingInteractor))]
    [SerializeField] private Object _afterSkippingInteractorObject;
    private ITypewritingInteractor AfterSkippingInteractor => _afterSkippingInteractorObject as ITypewritingInteractor;

    [RequireInterface(typeof(IAbortableTypewriter))]
    [SerializeField] private Object _typewriterObject;
    private IAbortableTypewriter Typewriter => _typewriterObject as IAbortableTypewriter;

    public IEnumerator OnTypewritingAllCoroutine(RuleEntryObject ruleEntry, IDialogueSpeechContent content)
    {
        yield return AfterSkippingInteractor?.OnTypewritingAllCoroutine(ruleEntry, content);
    }

    public IEnumerator OnTypewritingStepCoroutine(SpeechDialogueUnit speechUnit, IDialogueSpeechContent content)
    {
        if (CanInteract(content))
        {
            _previousTypingSpeed = Typewriter.CharactersPerSecond;

            _previousActionState = _skipTypewritingAction.action.enabled;
            _currentDialogueText = speechUnit.Message;

            _skipTypewritingAction.action.performed += SkipActionPerformed;
            SetContinuingActionState(true);
        }

        yield return AfterSkippingInteractor?.OnTypewritingStepCoroutine(speechUnit, content);
    }

    public IEnumerator OnTypewrittenStepCoroutine(SpeechDialogueUnit speechUnit, IDialogueSpeechContent content)
    {
        if (CanInteract(content))
        {
            Typewriter.CharactersPerSecond = _previousTypingSpeed;

            _skipTypewritingAction.action.performed -= SkipActionPerformed;
            SetContinuingActionState(_previousActionState);
        }

        yield return AfterSkippingInteractor?.OnTypewrittenStepCoroutine(speechUnit, content);
    }

    public IEnumerator OnTypewrittenAllCoroutine(RuleEntryObject ruleEntry, IDialogueSpeechContent content)
    {
        yield return AfterSkippingInteractor?.OnTypewrittenAllCoroutine(ruleEntry, content);
    }

    public bool CanInteract(IDialogueContent content)
    {
        return content is IDialogueSpeechContent speechContent && speechContent.Skippable;
    }

    private void SetContinuingActionState(bool enable)
    {
        if (enable)
        {
            _skipTypewritingAction.action.Enable();
            return;
        }
        _skipTypewritingAction.action.Disable();
    }

    private void SkipActionPerformed(InputAction.CallbackContext obj)
    {
        _skipTypewritingAction.action.performed -= SkipActionPerformed;

        if (_skipTypewritingInstantly)
        {
            Typewriter.SkipTypingToCompletion();
            return;
        }

        Typewriter.CharactersPerSecond = GetCharactersPerSecondToSkipInTime(_skipTime, Typewriter.GetTypedText(), _currentDialogueText);
    }

    private float GetCharactersPerSecondToSkipInTime(float time, string typed, string fullMessage) => (fullMessage.Length - typed.Length) / time;
}