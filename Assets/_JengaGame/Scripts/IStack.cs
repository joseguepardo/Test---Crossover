using Chronos;

namespace JengaGame
{
    public interface IStack : IClickable
    {
        string StackId { get; }
        GlobalClock Clock { get; }
        void PrepareForTest();
        void Reset();
    }
}