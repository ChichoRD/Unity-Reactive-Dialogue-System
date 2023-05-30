using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public struct BranchingDialogueUnit
{
    [field: SerializeField] public string BranchOptionText { get; private set; }
    [field: SerializeField] public UnityEvent OnPickedBranch { get; private set; }
}