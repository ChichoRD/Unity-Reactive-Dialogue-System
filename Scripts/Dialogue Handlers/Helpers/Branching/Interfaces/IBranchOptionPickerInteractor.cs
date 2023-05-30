using System.Collections;
using UnityEngine.Events;

public interface IBranchOptionPickerInteractor : IDialogueContentInteractor
{
    UnityEvent OnPickedBranch { get; }
    bool IsShowingOptions { get; }
    bool HasPicked { get; }

    void HideOptions();
    void SetOptions(IDialogueBranchContent branchContent);

    IEnumerator ShowOptionsCoroutine(IDialogueBranchContent branchContent);
    IEnumerator SetOptionsAndShowCoroutine(IDialogueBranchContent branchContent);
}