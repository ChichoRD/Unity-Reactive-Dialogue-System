using UnityEngine;
using UnityEngine.UI;

public class ImageComponentSettableImage : MonoBehaviour, ISettableImage
{
    [SerializeField] private Image _image;
    public void SetImage(Sprite sprite) => _image.sprite = sprite;
}
