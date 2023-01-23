using TMPro;
using UnityEngine;

namespace TitleMenus
{
    public class SavesButtonsScript : MonoBehaviour
    {
        public void OnClick()
        {
            LoadGameScript lgs = gameObject.GetComponentInParent<LoadGameScript>();

            TextMeshProUGUI textComponent = gameObject.GetComponentInChildren<TextMeshProUGUI>();

            // deselect currently active
            
            if (lgs.selectedSaveText != null)
            {
                lgs.selectedSaveText.color = Color.black;
            }
            
            if (lgs.selectedSave == textComponent.text)
            {
                lgs.selectedSave = "";
            }
            else
            {
                lgs.selectedSave = textComponent.text;
                lgs.selectedSaveText = textComponent;
                textComponent.color = Color.red;
            }
        }
    }

}