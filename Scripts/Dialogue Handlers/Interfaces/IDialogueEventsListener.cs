using System.Collections.Generic;

public interface IDialogueEventsListener
{
    bool Raisable { get; }
    bool OverrideHandlingOnEventOverflow { get; }
    IEnumerable<EventEntryObject> EventsEntries { get; }
    bool OnDialogueEventRaised(RuleEntryObject eventEntry);
    void InitialiseEventListening();
    void FinaliseEventListening();
}