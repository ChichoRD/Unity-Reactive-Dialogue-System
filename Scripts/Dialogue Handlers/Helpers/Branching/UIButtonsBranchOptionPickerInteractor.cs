using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIButtonsBranchOptionPickerInteractor : MonoBehaviour, IBranchOptionPickerInteractor
{
    [SerializeField] private bool _hideOptionsOnPicked = true;
    [SerializeField] private bool _useOptionsTimeout = false;
    [SerializeField] private float _optionsTimeout = 5f;

    [SerializeField] private Transform _buttonsParent;
    [SerializeField] private EventTrigger _buttonPrefab;

    private readonly Dictionary<BranchingDialogueUnit, EventTrigger> _branchButtonPairs = new Dictionary<BranchingDialogueUnit, EventTrigger>();
    public bool IsShowingOptions { get; private set; }
    public bool HasPicked { get; private set; }
    [field: SerializeField] public UnityEvent OnPickedBranch { get;  private set; }

    public void HideOptions()
    {
        foreach (var branchButtonPair in _branchButtonPairs)
            branchButtonPair.Value.gameObject.SetActive(false);

        IsShowingOptions = false;
    }

    public void SetOptions(IDialogueBranchContent branchContent)
    {
        foreach (var branchButtonPair in _branchButtonPairs)
        {
            if (branchContent.Branches.Contains(branchButtonPair.Key)) continue;

            _branchButtonPairs.Remove(branchButtonPair.Key, out var buttonTrigger);
            if (buttonTrigger == null) continue;

            Destroy(buttonTrigger.gameObject);
        }

        foreach (var branch in branchContent.Branches)
        {
            if (_branchButtonPairs.TryGetValue(branch, out var buttonTrigger) && buttonTrigger != null) continue;

            buttonTrigger = Instantiate(_buttonPrefab, _buttonsParent);
            buttonTrigger.gameObject.SetActive(false);
            buttonTrigger.GetComponentInChildren<TMP_Text>().text = branch.BranchOptionText;

            buttonTrigger.triggers.Add(new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerClick,
                callback = new EventTrigger.TriggerEvent()
            });
            buttonTrigger.triggers[^1].callback.RemoveListener(OnPointerClick);
            buttonTrigger.triggers[^1].callback.AddListener(OnPointerClick);

            _branchButtonPairs[branch] = buttonTrigger;

            void OnPointerClick(BaseEventData data)
            {
                if (_hideOptionsOnPicked)
                    HideOptions();
                HasPicked = true;
                branch.OnPickedBranch?.Invoke();
                OnPickedBranch?.Invoke();
            }
        }

        HasPicked = false;
    }

    public IEnumerator SetOptionsAndShowCoroutine(IDialogueBranchContent branchContent)
    {
        SetOptions(branchContent);
        yield return ShowOptionsCoroutine(branchContent);
    }

    public IEnumerator ShowOptionsCoroutine(IDialogueBranchContent branchContent)
    {
        foreach (var branch in branchContent.Branches)
        {
            if (!_branchButtonPairs.TryGetValue(branch, out var buttonTrigger) || buttonTrigger == null) continue;
            buttonTrigger.gameObject.SetActive(true);
        }

        IsShowingOptions = true;

        yield return new WaitUntil(() => HasPicked);
    }

    public bool CanInteract(IDialogueContent content) => content is IDialogueBranchContent;
}
