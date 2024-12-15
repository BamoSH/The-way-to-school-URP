using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
public class HoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite hoverImage;
    public Sprite defaultImage;
    public AudioClip hoverSound;
    public AudioSource audioSource;
    private Image image;
    

    private void Awake()
    {
        image = GetComponent<Image>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.sprite = hoverImage;
        audioSource.PlayOneShot(hoverSound);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.sprite = defaultImage;
    }
}
