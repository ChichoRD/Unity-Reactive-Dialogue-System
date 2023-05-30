using UnityEngine;
using UnityEngine.Events;

public class BranchDialogueHandler : MonoBehaviour, IDialogueHandler
{
    [RequireInterface(typeof(IBranchOptionPickerInteractor))]
    [SerializeField] private Object _branchOptionPickerObject;
    private IBranchOptionPickerInteractor BranchOptionPicker => _branchOptionPickerObject as IBranchOptionPickerInteractor;

    [field: SerializeField] public UnityEvent<RuleEntryObject> OnHandlingStarted { get; private set; }
    [field: SerializeField] public UnityEvent OnHandlingStopped { get; private set; }

    public bool IsHandling => BranchOptionPicker.IsShowingOptions && !BranchOptionPicker.HasPicked;

    public void StopHandling()
    {
        if (!IsHandling) return;

        BranchOptionPicker.HideOptions();
    }

    public bool TryHandle(RuleEntryObject ruleEntryObject)
    {
        if (ruleEntryObject.GetContent() is not IDialogueBranchContent content) return false;

        StartCoroutine(BranchOptionPicker.SetOptionsAndShowCoroutine(content));
        return true;
    }
}
