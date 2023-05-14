public interface IDialogueHandler
{
    bool IsHandling { get; }
    bool TryHandle(RuleEntryObject ruleEntryObject);
    void StopHandling();
}
