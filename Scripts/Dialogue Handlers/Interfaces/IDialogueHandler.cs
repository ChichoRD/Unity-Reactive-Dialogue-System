using UnityEngine.Events;

public interface IDialogueHandler
{
    UnityEvent<RuleEntryObject> OnHandlingStarted { get; }
    UnityEvent OnHandlingStopped { get; }

    bool IsHandling { get; }
    bool TryHandle(RuleEntryObject ruleEntryObject);
    void StopHandling();
}
