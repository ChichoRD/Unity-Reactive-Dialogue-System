using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TextMeshTypewriter : MonoBehaviour, ITypewriter
{
    [RequireInterface(typeof(ITypewriter))]
    [SerializeField] private Object _typewriterObject;
    private ITypewriter Typewriter => _typewriterObject as ITypewriter;
    [SerializeField] private TMP_Text _textMesh;

    public float CharactersPerSecond { get => Typewriter.CharactersPerSecond; set => Typewriter.CharactersPerSecond = value; }
    public UnityEvent<StringBuilder> OnTyped => Typewriter.OnTyped;

    private void Awake()
    {
        Typewriter.OnTyped.AddListener(UpdateVisibleText);
    }

    private void UpdateVisibleText(StringBuilder arg0)
    {
        _textMesh.maxVisibleCharacters = arg0.Length;
    }

    public IEnumerator TypeCoroutine(string text)
    {
        _textMesh.text = text;
        yield return Typewriter.TypeCoroutine(text);
    }

    public void ClearTypedText()
    {
        _textMesh.text = string.Empty;
        Typewriter.ClearTypedText();
    }

    public void AbortTyping()
    {
        Typewriter.AbortTyping();
    }

    public void SkiptTypingToCompletion()
    {
        Typewriter.SkiptTypingToCompletion();
    }

    public string GetTypedText()
    {
        return Typewriter.GetTypedText();
    }
}