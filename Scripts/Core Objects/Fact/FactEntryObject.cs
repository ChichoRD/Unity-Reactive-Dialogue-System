using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = NEW_WORD + OBJECT_NAME, menuName = PATH + OBJECT_NAME)]
public class FactEntryObject : DialogueEntryObject
{
    public const string OBJECT_NAME = "Fact Entry";

    [SerializeField] private int _value;
    public int Value 
    { 
        get => _value;
        set
        {
            _value = value;
            OnValueSet?.Invoke(value);
        }
    }

    [field: SerializeField] public UnityEvent<int> OnValueSet { get; private set; }

    public void Add(int amount) => Value += amount;
    public void Muliply(int amount) => Value *= amount;

    public void Not() => Value = ~Value;
    public void And(int other) => Value &= other;
    public void Or(int other) => Value |= other;
    public void Xor(int other) => Value ^= other;
    public void Nand(int other) => Value = ~(Value & other);
    public void Nor(int other) => Value = ~(Value | other);
    public void Xnor(int other) => Value = ~(Value ^ other);
}
