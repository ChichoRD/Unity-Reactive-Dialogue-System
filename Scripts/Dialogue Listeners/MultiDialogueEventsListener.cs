using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MultiDialogueEventsListener : MonoBehaviour, IDialogueEventsListener
{
    [SerializeField] private List<DialogueEventsListener> _dialogueEventsListeners = new List<DialogueEventsListener>();
    [SerializeField] private bool _overrideHandlingOnEventOverflow;
    [SerializeField] private bool _performInitialisations = true;

    public IEnumerable<EventEntryObject> EventsEntries => _dialogueEventsListeners.SelectMany(listener => listener.EventsEntries);
    public bool OverrideHandlingOnEventOverflow => _overrideHandlingOnEventOverflow;
    public bool Raisable => _dialogueEventsListeners.Any(listener => listener.Raisable);

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
        foreach (var dialogueEventsListener in _dialogueEventsListeners)
            dialogueEventsListener.InitialiseEventListening();
    }

    public void FinaliseEventListening()
    {
        foreach (var dialogueEventsListener in _dialogueEventsListeners)
            dialogueEventsListener.FinaliseEventListening();
    }

    public bool OnDialogueEventRaised(RuleEntryObject eventEntry)
    {
        if (OverrideHandlingOnEventOverflow)
        {
            foreach (var dialogueEventsListener in _dialogueEventsListeners)
                dialogueEventsListener.DialogueHandler.StopHandling();
            return _dialogueEventsListeners.Any(listener => listener.DialogueHandler.TryHandle(eventEntry));
        }

        return !_dialogueEventsListeners.Any(listener => listener.DialogueHandler.IsHandling) && _dialogueEventsListeners.Any(listener => listener.DialogueHandler.TryHandle(eventEntry));
    }
}