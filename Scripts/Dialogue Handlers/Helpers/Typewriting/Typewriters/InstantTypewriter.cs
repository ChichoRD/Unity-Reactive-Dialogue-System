using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

public class InstantTypewriter : MonoBehaviour, ITypewriter
{
    private readonly StringBuilder _stringBuilder = new StringBuilder();
    public float CharactersPerSecond { get => float.PositiveInfinity; set { } }
    [field: SerializeField] public UnityEvent<StringBuilder> OnTyped { get; private set; }

    public void ClearTypedText()
    {
        _stringBuilder.Clear();
        OnTyped?.Invoke(_stringBuilder);
    }

    public string GetTypedText()
    {
        return _stringBuilder.ToString();
    }

    public IEnumerator TypeCoroutine(string text)
    {
        ClearTypedText();

        yield return null;
        _stringBuilder.Append(text);

        OnTyped?.Invoke(_stringBuilder);
    }
}