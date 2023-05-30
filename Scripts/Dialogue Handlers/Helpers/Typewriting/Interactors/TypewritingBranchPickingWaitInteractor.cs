using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class TypewritingBranchPickingWaitInteractor : MonoBehaviour, ITypewritingInteractor, IBranchOptionPickerInteractor
{
    [RequireInterface(typeof(IBranchOptionPickerInteractor))]
    [SerializeField] private Object _branchOptionPickerObject;
    private IBranchOptionPickerInteractor BranchOptionPicker => _branchOptionPickerObject as IBranchOptionPickerInteractor;
    private bool _typewrittenAll;

    public UnityEvent OnPickedBranch => BranchOptionPicker.OnPickedBranch;
    public bool IsShowingOptions => BranchOptionPicker.IsShowingOptions;
    public bool HasPicked => BranchOptionPicker.HasPicked;

    public bool CanInteract(IDialogueContent content) => content is IDialogueBranchContent /*&& content is IDialogueSpeechContent*/;

    public IEnumerator OnTypewritingAllCoroutine(RuleEntryObject ruleEntry, IDialogueSpeechContent content)
    {
        if (!CanInteract(content) || BranchOptionPicker == null)
        {
            yield return null;
            yield break;
        }

        _typewrittenAll = false;
        yield return null;
    }

    public IEnumerator OnTypewritingStepCoroutine(SpeechDialogueUnit speechUnit, IDialogueSpeechContent content)
    {
        yield return null;
    }

    public IEnumerator OnTypewrittenAllCoroutine(RuleEntryObject ruleEntry, IDialogueSpeechContent content)
    {
        if (!CanInteract(content) || BranchOptionPicker == null)
        {
            yield return null;
            yield break;
        }

        _typewrittenAll = true;
        yield return null;
        yield return new WaitUntil(() => HasPicked || !IsShowingOptions);
        _typewrittenAll = false;
        yield return null;
    }

    public IEnumerator OnTypewrittenStepCoroutine(SpeechDialogueUnit speechUnit, IDialogueSpeechContent content)
    {
        yield return null;
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
        //Called a lot! Very weird
        Debug.Log($"Has typewritten all: {_typewrittenAll}");
        yield return new WaitUntil(() => _typewrittenAll);
        yield return BranchOptionPicker.ShowOptionsCoroutine(branchContent);
    }
}