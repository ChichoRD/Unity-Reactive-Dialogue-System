using System.Collections.Generic;

public interface IDialogueBranchContent : IDialogueContent
{
    IEnumerable<BranchingDialogueUnit> Branches { get; }
}