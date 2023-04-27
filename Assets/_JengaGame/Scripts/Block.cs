using System;
using System.Collections;
using Chronos;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace JengaGame
{
    public class Block : MonoBehaviour, IBlock
    {
        public Transform Transform => transform;
        public string StackId { get; private set; }
        public int BlockId { get; private set; }
        [ShowInInspector, ReadOnly] public BlockData BlockData { get; private set; }
        [SerializeField, BoxGroup("References")]
        private MeshRenderer meshRenderer;
        [SerializeField, BoxGroup("References")]
        private Material[] blockMaterials;

        [SerializeField, BoxGroup("References")]
        private Collider blockCollider;
        [SerializeField, BoxGroup("References")]
        private Rigidbody blockRigidbody;
        [SerializeField, BoxGroup("References")]
        private Timeline timeline;

        [SerializeField, BoxGroup("Outline")] private Outline outline;
        [SerializeField, BoxGroup("Outline")] private Color selectedColor, hoveredColor;

        [ShowInInspector, ReadOnly] private bool _isSelected;
        [ShowInInspector, ReadOnly] private Vector3 _startingPosition;

        [SerializeField, BoxGroup("Starting animation")]
        private AnimationCurve startingAnimationCurve;
        [SerializeField, BoxGroup("Starting animation")]
        private float startingAnimationDuration;
        [SerializeField, BoxGroup("Starting animation")]
        private float startingAnimationDelay;

        public enum BlockType
        {
            Glass = 0,
            Wood = 1,
            Stone = 2
        }

        [ReadOnly] public BlockType BlockTypeValue { get; private set; }

        public void Initialize(string stackId, int blockId, BlockData blockData, GlobalClock parentClock,
            Vector3 startingPosition)
        {
            StackId = stackId;
            BlockId = blockId;
            BlockData = blockData;
            BlockTypeValue = (BlockType)blockData.Mastery;
            outline.OutlineColor = hoveredColor;

            meshRenderer.material = blockMaterials[(int)BlockTypeValue];
            blockRigidbody.mass = BlockTypeValue == BlockType.Stone ? 5 : 1;
            timeline.globalClockKey = parentClock.key;
            _startingPosition = startingPosition;

            StartCoroutine(PlayStartingAnimationCo());
        }

        private IEnumerator PlayStartingAnimationCo()
        {
            var cachedT = transform;
            cachedT.localScale = Vector3.zero;
            yield return null;
            cachedT.localPosition = new Vector3(_startingPosition.x, _startingPosition.y + 10, _startingPosition.z);
            transform.DOLocalMove(_startingPosition, startingAnimationDuration).SetEase(startingAnimationCurve)
                .SetDelay(1 + startingAnimationDelay * BlockId)
                .OnStart(() => { transform.localScale = Vector3.one; });
        }

        public void EnableCollider(bool enable)
        {
            blockCollider.enabled = enable;
        }

        public void OnClick(bool isClicked)
        {
            _isSelected = isClicked;
            outline.enabled = _isSelected;
            outline.OutlineColor = _isSelected ? selectedColor : hoveredColor;
            if (_isSelected) GameManager.Instance.SelectBlock(this);
        }

        public void OnHover(bool isHovered)
        {
            if (_isSelected) return;

            outline.enabled = isHovered;
            GameManager.Instance.HoverBlock(isHovered ? this : null);
        }

        public void Reset()
        {
            transform.localPosition = _startingPosition;
        }
    }
}