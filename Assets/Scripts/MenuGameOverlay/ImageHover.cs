using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MenuGameOverlay
{
    public class ImageHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public Color colorOver;
        public Color colorNormal;
    
        public void OnPointerEnter(PointerEventData eventData)
        {
            gameObject.GetComponent<Image>().color = colorOver;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            gameObject.GetComponent<Image>().color = colorNormal;
        }
    }
}

