using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

public class Typewriter : MonoBehaviour, ITypewriter
{
    private const float AVERAGE_CHARACTERS_PER_SECOND = 8.888f;
    private readonly Dictionary<char, float> _charactersWaitTimeMultipliers = new Dictionary<char, float>()
    {
        { '.', 4.0f },
        { '!', 4.0f },
        { '?', 4.0f },

        { ',', 2.0f },
        { ':', 2.0f },
        { ';', 2.0f },
        { '(', 2.0f },
        { ')', 2.0f },

        { ' ', 0.5f },
        { '\n', 0.5f },
        { '\t', 0.5f },
    };

    private readonly StringBuilder _stringBuilder = new StringBuilder();
    private bool _skipToken;
    private bool _abortToken;
    [field: SerializeField][field: Min(0.0f)] public float CharactersPerSecond { get; set; } = AVERAGE_CHARACTERS_PER_SECOND;
    [field: SerializeField] public UnityEvent<StringBuilder> OnTyped { get; private set; }

    public void ClearTypedText()
    {
        _stringBuilder.Clear();
        OnTyped?.Invoke(_stringBuilder);
    }

    public void AbortTyping() => _abortToken = true;

    public void SkiptTypingToCompletion() => _skipToken = true;

    public string GetTypedText() => _stringBuilder.ToString();

    public IEnumerator TypeCoroutine(string text)
    {
        ClearTypedText();

        yield return TypingWaitCoroutine('\0');

        foreach (char c in text)
        {
            _stringBuilder.Append(c);
            OnTyped?.Invoke(_stringBuilder);

            yield return TypingWaitCoroutine(c);
        }

        IEnumerator TypingWaitCoroutine(char c)
        {
            float waitTime = GetCharTypingTime(c);
            for (float t = 0; t < waitTime; t += Time.deltaTime)
            {
                if (_skipToken)
                {
                    _skipToken = false;

                    _stringBuilder.Append(_stringBuilder.Length..^0);
                    OnTyped?.Invoke(_stringBuilder);

                    yield break;
                }

                if (_abortToken)
                {
                    _abortToken = false;
                    yield break;
                }

                yield return null;
            }
        }
    }

    private float GetCharTypingTime(char currentCharacter) => (1.0f / CharactersPerSecond) *
                                                              (_charactersWaitTimeMultipliers.ContainsKey(currentCharacter) ?
                                                                _charactersWaitTimeMultipliers[currentCharacter] :
                                                                1.0f);
}
