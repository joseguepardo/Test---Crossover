using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace JengaGame
{
    public class Stack : MonoBehaviour
    {
        [SerializeField, BoxGroup("Configuration")]
        private GameObject blockPrefab;
        [SerializeField, BoxGroup("Configuration")]
        private Vector3 blockSize;
        [SerializeField, BoxGroup("Configuration")]
        private float blockSpacing;

        public void BuildStack(List<BlockData> blocksData)
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
        }

        private List<BlockData> GetBlocksDataReordered(List<BlockData> blocksData)
        {
            return blocksData
                .OrderBy(blockData => blockData.Domain)
                .ThenBy(blockData => blockData.Cluster)
                .ThenBy(blockData => blockData.StandardId)
                .ToList();
        }
    }
}