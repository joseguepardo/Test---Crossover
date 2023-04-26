using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace JengaGame
{
    public class Block : MonoBehaviour, IClickableBlock
    {
        public string StackId { get; private set; }
        public int BlockId { get; private set; }
        [ShowInInspector, ReadOnly] public BlockData BlockData;

        [SerializeField] private Collider blockCollider;
        [SerializeField, BoxGroup("Outline")] private Outline outline;
        [SerializeField, BoxGroup("Outline")] private Color selectedColor, hoveredColor;

        [ShowInInspector, ReadOnly] private bool _isSelected;

        public enum BlockType
        {
            Glass = 0,
            Wood = 1,
            Stone = 2
        }

        [ReadOnly] public BlockType BlockTypeValue;

        public void Initialize(string stackId, int blockId, BlockData blockData)
        {
            StackId = stackId;
            BlockId = blockId;
            BlockData = blockData;
            BlockTypeValue = (BlockType)blockData.Mastery;
            outline.OutlineColor = hoveredColor;
        }

        public void EnableCollider(bool enable)
        {
            blockCollider.enabled = enable;
        }

        public void OnClick(bool isClicked)
        {
            _isSelected = isClicked;
            blockCollider.enabled = !_isSelected;
            outline.enabled = _isSelected;
            outline.OutlineColor = _isSelected ? selectedColor : hoveredColor;
            if (_isSelected) GameManager.Instance.SelectBlock(this);
        }

        public void OnHover(bool isHovered)
        {
            if (_isSelected) return;

            outline.enabled = isHovered;
        }
    }
}