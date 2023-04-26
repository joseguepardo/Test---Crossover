using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace JengaGame
{
    public class Stack : MonoBehaviour, IClickableStack
    {
        public string StackId { get; private set; }
        [SerializeField, BoxGroup("Configuration")]
        private GameObject blockPrefab;
        [SerializeField, BoxGroup("Configuration")]
        private Vector3 blockSize;
        [SerializeField, BoxGroup("Configuration")]
        private float blockSpacing;
        [SerializeField, BoxGroup("References")]
        private BoxCollider stackCollider;
        [SerializeField, BoxGroup("References")]
        private TMP_Text gradeText;
        private List<Block> _blocks;

        [SerializeField, BoxGroup("Selection")]
        private GameObject baseGo;
        [SerializeField, BoxGroup("Selection")]
        private Outline outlineCube;
        [SerializeField, BoxGroup("Selection")]
        private Color hoveredColor;

        private bool _isSelected;

        public void InitializeStack(List<BlockData> blocksData)
        {
            BuildStack(blocksData);
        }

        private void BuildStack(List<BlockData> blocksData)
        {
            if (blocksData == null || blocksData.Count == 0) return;

            StackId = blocksData[0].Grade;
            gradeText.text = blocksData[0].Grade;
            _blocks = new List<Block>();

            var numberOfStacks = blocksData.Count;
            var reorderedBlocksData = GetBlocksDataReordered(blocksData);

            var leftPosition = -blockSize.x - blockSpacing;
            var positionForward = false;
            var floorId = 0;
            var floorHeight = 0f;
            var floorBlockId = 0;
            for (var i = 0; i < numberOfStacks; i++)
            {
                if (i % 3 == 0)
                {
                    positionForward = !positionForward;
                    floorId++;
                    floorHeight = (blockSize.y + blockSpacing) * floorId;
                    floorBlockId = 0;
                }

                var blockT = Instantiate(blockPrefab, transform).transform;
                var position = positionForward
                    ? new Vector3(leftPosition + (blockSize.x * floorBlockId) + (blockSpacing * floorBlockId), floorHeight,
                        0)
                    : new Vector3(0, floorHeight,
                        leftPosition + (blockSize.x * floorBlockId) + (blockSpacing * floorBlockId));
                blockT.localPosition = position;
                blockT.localEulerAngles = positionForward ? Vector3.zero : new Vector3(0, 90, 0);

                var block = blockT.GetComponent<Block>();
                block.Initialize(StackId, i, reorderedBlocksData[i]);
                _blocks.Add(block);
                floorBlockId++;
            }

            InitializeCollider(floorHeight);
        }

        private List<BlockData> GetBlocksDataReordered(List<BlockData> blocksData)
        {
            return blocksData
                .OrderBy(blockData => blockData.Domain)
                .ThenBy(blockData => blockData.Cluster)
                .ThenBy(blockData => blockData.StandardId)
                .ToList();
        }

        private void InitializeCollider(float height)
        {
            height += blockSize.y / 2;
            stackCollider.size = new Vector3(blockSize.z, height, blockSize.z);
            stackCollider.center = new Vector3(0, height / 2, 0);

            var outlineCubeT = outlineCube.transform;
            outlineCubeT.localScale = new Vector3(blockSize.z, height, blockSize.z);
            outlineCubeT.localPosition = new Vector3(0, height / 2, 0);
            outlineCubeT.gameObject.SetActive(false);
        }

        public void OnClick(bool isClicked)
        {
            if (_isSelected == isClicked) return;

            _isSelected = isClicked;

            stackCollider.enabled = !_isSelected;
            outlineCube.gameObject.SetActive(false);
            baseGo.SetActive(_isSelected);

            if (_isSelected)
            {
                GameManager.Instance.SelectStack(this);
            }

            foreach (var block in _blocks)
            {
                block.EnableCollider(isClicked);
                block.OnHover(false);
            }
        }

        public void OnHover(bool isHovered)
        {
            if (_isSelected) return;

            outlineCube.gameObject.SetActive(isHovered);
        }
    }
}