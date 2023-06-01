using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogueEventsListener : MonoBehaviour, IDialogueEventsListener
{
    [RequireInterface(typeof(IDialogueHandler))]
    [SerializeField] private Object _dialogueHandlerObject;
    [SerializeField] private List<EventEntryObject> _eventsEntries = new List<EventEntryObject>();
    [SerializeField] private bool _overrideHandlingOnEventOverflow;
    [SerializeField] private bool _performInitialisations = true;

    public IDialogueHandler DialogueHandler => _dialogueHandlerObject as IDialogueHandler;
    public IEnumerable<EventEntryObject> EventsEntries => _eventsEntries;
    public bool OverrideHandlingOnEventOverflow => _overrideHandlingOnEventOverflow;
    public bool Raisable => _eventsEntries.Any(e => e.GetSuccessfullyDispatchingRules(e.ListenerRules).Count() > 0);

    private void OnEnable()
    {
        if (!_performInitialisations) return;
        InitialiseEventListening();
    }

    private void OnDisable()
    {
        if (!_performInitialisations) return;
        FinaliseEventListening();
    }

    public void InitialiseEventListening()
    {
        FilterEventEntries();

        foreach (var eventEntry in _eventsEntries)
        {
            eventEntry.OnDispatched.RemoveListener(OnDialogueEventDispatched);
            eventEntry.OnDispatched.AddListener(OnDialogueEventDispatched);
        }
    }

    public void FinaliseEventListening()
    {
        foreach (var eventEntry in _eventsEntries)
            eventEntry.OnDispatched.RemoveListener(OnDialogueEventDispatched);
    }

    private void OnDialogueEventDispatched(RuleEntryObject ruleEntryObject) => OnDialogueEventRaised(ruleEntryObject);

    public bool OnDialogueEventRaised(RuleEntryObject eventEntry)
    {
        if (OverrideHandlingOnEventOverflow)
        {
            DialogueHandler.StopHandling();
            return DialogueHandler.TryHandle(eventEntry);
        }

        return !DialogueHandler.IsHandling && DialogueHandler.TryHandle(eventEntry);
    }

    private void FilterEventEntries() => _eventsEntries = _eventsEntries.Distinct().Where(e => e != null).ToList();
}
