using TMPro;
using UnityEngine;

namespace JengaGame
{
    public class BlockInfoPanelBubble : MonoBehaviour
    {
        public TMP_Text contentText;
        
        public void SetText(string text)
        {
            contentText.text = text;
        }
    }
}