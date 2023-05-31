using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class TypewritingBranchPickingWaitInteractor : MonoBehaviour, ITypewritingInteractor, IBranchOptionPickerInteractor
{
    [RequireInterface(typeof(IBranchOptionPickerInteractor))]
    [SerializeField] private Object _branchOptionPickerObject;
    private IBranchOptionPickerInteractor BranchOptionPicker => _branchOptionPickerObject as IBranchOptionPickerInteractor;

    [RequireInterface(typeof(ITypewritingInteractor))]
    [SerializeField] private Object _alternativeTypewritingInteractorObject;
    private ITypewritingInteractor AlternativeTypewritingInteractor => _alternativeTypewritingInteractorObject as ITypewritingInteractor;
    private bool _typewrittenAll;
    private bool _branchingRequested;

    public UnityEvent<object> OnPickedBranch => BranchOptionPicker.OnPickedBranch;
    public bool IsShowingOptions => BranchOptionPicker.IsShowingOptions;
    public bool HasPicked => BranchOptionPicker.HasPicked;

    private void Awake()
    {
        OnPickedBranch.AddListener(_ => _branchingRequested = false);
    }

    public bool CanInteract(IDialogueContent content) => _branchingRequested; //content is IDialogueBranchContent /*&& content is IDialogueSpeechContent*/;

    public IEnumerator OnTypewritingAllCoroutine(RuleEntryObject ruleEntry, IDialogueSpeechContent content)
    {
        if (!CanInteract(content) || BranchOptionPicker == null)
        {
            yield return AlternativeTypewritingInteractor?.OnTypewritingAllCoroutine(ruleEntry, content);
            yield break;
        }

        _typewrittenAll = false;
        yield return null;
    }

    public IEnumerator OnTypewritingStepCoroutine(SpeechDialogueUnit speechUnit, IDialogueSpeechContent content)
    {
        yield return AlternativeTypewritingInteractor?.OnTypewritingStepCoroutine(speechUnit, content);
    }

    public IEnumerator OnTypewrittenAllCoroutine(RuleEntryObject ruleEntry, IDialogueSpeechContent content)
    {
        if (!CanInteract(content) || BranchOptionPicker == null)
        {
            yield return AlternativeTypewritingInteractor?.OnTypewrittenAllCoroutine(ruleEntry, content);
            yield break;
        }
        Debug.Log(nameof(OnTypewrittenAllCoroutine));

        _typewrittenAll = true;
        yield return null;

        yield return new WaitUntil(() => HasPicked || !IsShowingOptions);
        _typewrittenAll = false;

        yield return null;
    }

    public IEnumerator OnTypewrittenStepCoroutine(SpeechDialogueUnit speechUnit, IDialogueSpeechContent content)
    {
        yield return AlternativeTypewritingInteractor?.OnTypewrittenStepCoroutine(speechUnit, content);
    }

    public void HideOptions()
    {
        BranchOptionPicker.HideOptions();
    }

    public void SetOptions(IDialogueBranchContent branchContent)
    {
        BranchOptionPicker.SetOptions(branchContent);
    }

    public IEnumerator SetOptionsAndShowCoroutine(IDialogueBranchContent branchContent)
    {
        SetOptions(branchContent);
        yield return ShowOptionsCoroutine(branchContent);
    }

    public IEnumerator ShowOptionsCoroutine(IDialogueBranchContent branchContent)
    {
        _branchingRequested = true;
        yield return new WaitUntil(() => _typewrittenAll);
        yield return BranchOptionPicker.ShowOptionsCoroutine(branchContent);
    }
}