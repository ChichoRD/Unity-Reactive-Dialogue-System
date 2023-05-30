using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SpeechToTypewritingDialogueHandler : MonoBehaviour, IDialogueHandler
{
    [RequireInterface(typeof(ITypewriter))]
    [SerializeField] private Object _typewriterObject;
    private ITypewriter Typewriter => _typewriterObject as ITypewriter;

    [RequireInterface(typeof(ITypewritingInteractor))]
    [SerializeField] private Object _typewritingInteractorObject;
    private ITypewritingInteractor TypewritingInteractor => _typewritingInteractorObject as ITypewritingInteractor;

    [SerializeField] private bool _clearTextOnFinished = true;
    private Coroutine _typewritingCoroutine;
    public bool IsHandling => _typewritingCoroutine != null;
    [field: SerializeField] public UnityEvent<RuleEntryObject> OnHandlingStarted { get; private set; }
    [field: SerializeField] public UnityEvent OnHandlingStopped { get; private set; }

    public void StopHandling()
    {
        if (!IsHandling) return;

        if (_clearTextOnFinished) Typewriter.ClearTypedText();

        StopCoroutine(_typewritingCoroutine);
        _typewritingCoroutine = null;
        OnHandlingStopped?.Invoke();
    }

    public bool TryHandle(RuleEntryObject ruleEntryObject)
    {
        if (ruleEntryObject.GetContent() is not IDialogueSpeechContent content) return false;
        _typewritingCoroutine = StartCoroutine(TypewriteSequential(ruleEntryObject, content));
        OnHandlingStarted?.Invoke(ruleEntryObject);
        return true;
    }

    private IEnumerator TypewriteSingle(SpeechDialogueUnit speechUnit, IDialogueSpeechContent content)
    {
        yield return TypewritingInteractor?.OnTypewritingStepCoroutine(speechUnit, content);
        float initialCharactersPerSecond = Typewriter.CharactersPerSecond;
        Typewriter.CharactersPerSecond = speechUnit.OverrideTypingSpeed ? speechUnit.TypingSpeed : Typewriter.CharactersPerSecond;

        yield return Typewriter.TypeCoroutine(speechUnit.Message);
        Typewriter.CharactersPerSecond = initialCharactersPerSecond;

        yield return TypewritingInteractor?.OnTypewrittenStepCoroutine(speechUnit, content);
    }

    private IEnumerator TypewriteSequential(RuleEntryObject ruleEntryObject, IDialogueSpeechContent content)
    {
        yield return TypewritingInteractor?.OnTypewritingAllCoroutine(ruleEntryObject, content);

        foreach (var speechUnit in content.Speech)
            yield return TypewriteSingle(speechUnit, content);

        yield return TypewritingInteractor?.OnTypewrittenAllCoroutine(ruleEntryObject, content);

        StopHandling();
        ruleEntryObject.RaiseCascadingEvents();
    }
}