using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JengaGame
{
    public class BlockInfoPanel : MonoBehaviour
    {
        [System.Serializable]
        public class InfoTierReferences
        {
            public Block.BlockType BlockType;
            public Sprite panelSprite, iconSprite;
            public GameObject bubblePrefab;
        }

        [SerializeField] private InfoTierReferences[] infoTierReferences;
        [SerializeField] private Image panelImage, iconImage;
        [SerializeField] private TMP_Text gradeText;
        [SerializeField] private RectTransform bubbleParent;

        public void Initialize(IBlock block)
        {
            DestroyBubbles();

            foreach (var infoTierReference in infoTierReferences)
            {
                if (infoTierReference.BlockType != block.BlockTypeValue) continue;

                gradeText.text = block.BlockData.Grade;
                panelImage.sprite = infoTierReference.panelSprite;
                iconImage.sprite = infoTierReference.iconSprite;
                InstantiateBubbles(infoTierReference.bubblePrefab, block.BlockData);
                break;
            }
        }

        private void DestroyBubbles()
        {
            var bubbleCount = bubbleParent.childCount;
            for (var i = 0; i < bubbleCount; i++)
            {
                Destroy(bubbleParent.GetChild(i).gameObject);
            }
        }

        private void InstantiateBubbles(GameObject bubblePrefab, BlockData blockData)
        {
            var bubbleContent = new string[]
            {
                blockData.Domain,
                blockData.Cluster,
                $"{blockData.StandardId} : {blockData.StandardDescription}"
            };

            foreach (var content in bubbleContent)
            {
                var bubble = Instantiate(bubblePrefab, bubbleParent).GetComponent<BlockInfoPanelBubble>();
                bubble.SetText(content);
            }
        }
    }
}