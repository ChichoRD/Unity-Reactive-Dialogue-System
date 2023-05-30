using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ImageDialogueHandler : MonoBehaviour, IDialogueHandler
{
    [RequireInterface(typeof(ISettableImage))]
    [SerializeField] private Object _settableImageObject;
    private ISettableImage SettableImage => _settableImageObject as ISettableImage;
    private Coroutine _imageShowCoroutine;

    public bool IsHandling => _imageShowCoroutine != null;
    [field: SerializeField] public UnityEvent<RuleEntryObject> OnHandlingStarted { get; private set; }
    [field: SerializeField] public UnityEvent OnHandlingStopped { get; private set; }

    public bool TryHandle(RuleEntryObject ruleEntryObject)
    {
        if (ruleEntryObject.GetContent() is not IDialogueImageContent content) return false;
        _imageShowCoroutine = StartCoroutine(ShowImageCoroutine(ruleEntryObject, content));
        OnHandlingStarted?.Invoke(ruleEntryObject);
        return true;
    }

    public void StopHandling()
    {
        if (!IsHandling) return;

        StopCoroutine(_imageShowCoroutine);
        _imageShowCoroutine = null;
        OnHandlingStopped?.Invoke();
    }

    private IEnumerator ShowImageCoroutine(RuleEntryObject ruleEntryObject, IDialogueImageContent content)
    {
        SettableImage.SetImage(content.ImageUnit.Image);
        yield return content.ImageUnit.UseShowTime ? new WaitForSeconds(content.ImageUnit.ShowTime) : null;

        StopHandling();
        ruleEntryObject.RaiseCascadingEvents();
    }
}
