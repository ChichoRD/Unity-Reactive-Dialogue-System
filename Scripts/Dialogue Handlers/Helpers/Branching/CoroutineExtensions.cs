using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public static class CoroutineExtensions
{
    public static IEnumerator WaitForCallback(IEnumerator coroutine, Action callback)
    {
        yield return coroutine;
        callback?.Invoke();
    }

    public static IEnumerator WaitAny(this MonoBehaviour _, params IEnumerator[] coroutines)
    {
        bool[] completions = new bool[coroutines.Length];
        for (int i = 0; i < coroutines.Length; i++)
        {
            int index = i;
            coroutines[i] = WaitForCallback(coroutines[i], () => completions[index] = true);
        }

        yield return new WaitUntil(() => completions.Any(completion => completion));
    } 

    public static IEnumerator WaitAll(this MonoBehaviour _, params IEnumerator[] coroutines)
    {
        bool[] completions = new bool[coroutines.Length];
        for (int i = 0; i < coroutines.Length; i++)
        {
            int index = i;
            coroutines[i] = WaitForCallback(coroutines[i], () => completions[index] = true);
        }
        yield return new WaitUntil(() => completions.All(completion => completion));
    }
}