public interface IAbortableTypewriter : ITypewriter
{
    void AbortTyping();
    void SkipTypingToCompletion();
}
