using UnityEngine;

public class SpeechUnitKeyReplacerFactUpdater : MonoBehaviour
{
    [SerializeField] private SpeechUnitKeyReplacer _speechUnitKeyReplacer;
    [SerializeField] private FactEntryObject[] _factEntryObjects;

    private void Awake()
    {
        foreach (FactEntryObject factEntryObject in _factEntryObjects)
        {
            _speechUnitKeyReplacer.KeyReplacements[factEntryObject.Name] = factEntryObject.Value;
            factEntryObject.OnValueChanged.AddListener(OnFactValueChanged);

            void OnFactValueChanged(int value)
            {
                _speechUnitKeyReplacer.KeyReplacements[factEntryObject.Name] = value;
            }
        }
    }
}
