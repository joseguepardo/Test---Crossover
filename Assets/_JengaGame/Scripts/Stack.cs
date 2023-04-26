using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace JengaGame
{
    public class Stack : MonoBehaviour
    {
        public string StackId;
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

        public void InitializeStack(List<BlockData> blocksData)
        {
            BuildStack(blocksData);
            InitializeListeners();
        }

        private void BuildStack(List<BlockData> blocksData)
        {
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

                blockT.GetComponent<Block>().Initialize(reorderedBlocksData[i]);

                floorBlockId++;
            }

            StackId = blocksData[0].Grade;
            gradeText.text = blocksData[0].Grade;

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
        }

        private void InitializeListeners()
        {
            GameManager.Instance.OnStackSelected += OnStackSelected;
        }

        private void OnStackSelected(Stack stack)
        {
            stackCollider.enabled = stack.StackId != StackId;
        }

        private void OnDestroy()
        {
            GameManager.Instance.OnStackSelected -= OnStackSelected;
        }
    }
}