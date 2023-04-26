namespace JengaGame
{
    public interface IClickable
    {
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
    }
}