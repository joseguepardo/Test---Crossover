using UnityEngine;

namespace JengaGame
{
    public interface IClickable
    {
        Transform Transform { get; }
        void OnClick(bool isClicked);
        void OnHover(bool isHovered);
    }
}