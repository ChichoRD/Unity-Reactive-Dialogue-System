using UnityEngine;

public class SpriteRendererComponentSettableImage : MonoBehaviour, ISettableImage
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    public void SetImage(Sprite sprite) => _spriteRenderer.sprite = sprite;
}