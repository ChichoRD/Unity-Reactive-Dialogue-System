using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MultiEventEntryRaiser : MonoBehaviour, IEventEntryRaiser
{
    [SerializeField] private Object[] _eventEntryRaiserObjects;
    private IEnumerable<IEventEntryRaiser> EventEntryRaisers => _eventEntryRaiserObjects.Cast<IEventEntryRaiser>();

    public void Raise()
    {
        foreach (var eventEntryObject in EventEntryRaisers)
            eventEntryObject.Raise();
    }
}