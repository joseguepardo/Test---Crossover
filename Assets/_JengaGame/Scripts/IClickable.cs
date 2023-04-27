using UnityEngine;

namespace JengaGame
{
    public interface IClickable
    {
        Transform Transform { get; }
        void OnClick(bool isClicked);
        void OnHover(bool isHovered);
    }
    
    public interface IClickableStack : IClickable
    {
        string StackId { get; }
    }
    
    public interface IClickableBlock : IClickable
    {
        string StackId { get; }
        int BlockId { get; }
        Block.BlockType BlockTypeValue { get; }
        BlockData BlockData { get; }
    }
}