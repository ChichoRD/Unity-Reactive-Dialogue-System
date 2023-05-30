using System.Collections;
using System.Text;
using UnityEngine.Events;

public interface ITypewriter
{
    float CharactersPerSecond { get; set; }
    UnityEvent<StringBuilder> OnTyped { get; }
    IEnumerator TypeCoroutine(string text);
    void ClearTypedText();
    void AbortTyping();
    void SkiptTypingToCompletion();
    string GetTypedText();
}
