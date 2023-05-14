using System.Collections;
using UnityEngine;

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

    public void StopHandling()
    {
        if (!IsHandling) return;

        StopCoroutine(_typewritingCoroutine);
        _typewritingCoroutine = null;
    }

    public bool TryHandle(RuleEntryObject ruleEntryObject)
    {
        if (ruleEntryObject.GetContent() is not IDialogueSpeechContent content) return false;
        _typewritingCoroutine = StartCoroutine(TypewriteSequential(ruleEntryObject, content));
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
        if (_clearTextOnFinished) Typewriter.ClearTypedText();
    }

    private IEnumerator TypewriteSequential(RuleEntryObject ruleEntryObject, IDialogueSpeechContent content)
    {
        yield return TypewritingInteractor?.OnTypewritingAllCoroutine(ruleEntryObject);

        foreach (var speechUnit in content.Speech)
            yield return TypewriteSingle(speechUnit, content);

        yield return TypewritingInteractor?.OnTypewrittenAllCoroutine(ruleEntryObject);

        _typewritingCoroutine = null;
        ruleEntryObject.RaiseCascadingEvents();
    }
}