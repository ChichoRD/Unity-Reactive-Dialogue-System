using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct DialogueBranchContent : IDialogueBranchContent
{
    [SerializeField] private BranchingDialogueUnit[] _branches;
    public IEnumerable<BranchingDialogueUnit> Branches => _branches;
}
