using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public struct AudioDialogueUnit
{
    [field: SerializeField] public AudioClip Audio { get; private set; }
    [field: SerializeField] [field: Range(0.0f, 1.0f)] public float Volume { get; private set; }
    [field: SerializeField] [field: Range(0.0f, 1.0f)] public float VolumeVariance { get; private set; }
    [field: SerializeField] [field: Range(-3.0f, 3.0f)] public float Pitch { get; private set; }
    [field: SerializeField] [field: Range(-3.0f, 3.0f)] public float PitchVariance { get; private set; }

    public float GetVolumeWithVariance() => Volume + Random.Range(-VolumeVariance, VolumeVariance);
    public float GetPitchWithVariance() => Pitch + Random.Range(-PitchVariance, PitchVariance);
}