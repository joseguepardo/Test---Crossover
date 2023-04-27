namespace JengaGame
{
    public interface IBlock : IClickable
    {
        string StackId { get; }
        int BlockId { get; }
        Block.BlockType BlockTypeValue { get; }
        BlockData BlockData { get; }
    }
}