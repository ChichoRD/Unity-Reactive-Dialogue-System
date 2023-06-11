using System.Collections;
using UnityEngine;

public class EventEntryRaiser : MonoBehaviour, IEventEntryRaiser
{
    [SerializeField] private EventEntryObject _eventEntryObject;

    public void Raise() => _eventEntryObject.Dispatch();
}