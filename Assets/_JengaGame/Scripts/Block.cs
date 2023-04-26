using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace JengaGame
{
    public class Block : MonoBehaviour
    {
        [ShowInInspector, ReadOnly] public BlockData BlockData;

        public enum BlockType
        {
            Glass = 0,
            Wood = 1,
            Stone = 2
        }

        [ReadOnly] public BlockType BlockTypeValue;

        public void Initialize(BlockData blockData)
        {
            BlockData = blockData;
            BlockTypeValue = (BlockType)blockData.Mastery;
        }
    }
}