using UnityEngine;

public class DialogueEntryObject : ScriptableObject
{
    public const string PATH = "Dialogue System/";
    public const string NEW_WORD = "New ";

    [field: SerializeField] public string Name { get; private set; }
}
