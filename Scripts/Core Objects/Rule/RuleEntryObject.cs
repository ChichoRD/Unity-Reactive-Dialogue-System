using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = NEW_WORD + OBJECT_NAME, menuName = PATH + OBJECT_NAME)]
public class RuleEntryObject : DialogueEntryObject
{
    public const string OBJECT_NAME = "Rule Entry";

    [SerializeReference] private IDialogueContent _content;
    [SerializeField] private List<EventEntryObject> raisableEvents;
    [field: SerializeField] public Criteria Criteria { get; private set; }
    [field: SerializeField] public UnityEvent OnDispatched { get; private set; }

    public void RaiseCascadingEvents()
    {
        foreach (var e in raisableEvents)
            e.Dispatch();
    }

    public IDialogueContent GetContent() => _content;


    [ContextMenu(nameof(SetSpeechContent))]
    private void SetSpeechContent() => _content = new DialogueSpeechContent();

    [ContextMenu(nameof(SetAudioContent))]
    private void SetAudioContent() => _content = new DialogueAudioContent();

    [ContextMenu(nameof(SetImageContent))]
    private void SetImageContent() => _content = new DialogueImageContent();

    [ContextMenu(nameof(SetCharacterisedContent))]
    private void SetCharacterisedContent() => _content = new DialogueCharacterisedContent();


    [ContextMenu(nameof(AddBooleanCondition))]
    private void AddBooleanCondition() => Criteria.Conditions.Add(new CriteriaBooleanCondition());

    [ContextMenu(nameof(AddConstantCondition))]
    private void AddConstantCondition() => Criteria.Conditions.Add(new CriteriaConstantCondition());

    [ContextMenu(nameof(AddFactCondition))]
    private void AddFactCondition() => Criteria.Conditions.Add(new CriteriaFactCondition());
}
