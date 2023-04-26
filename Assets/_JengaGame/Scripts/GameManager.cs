using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace JengaGame
{
    public class GameManager : Singleton<GameManager>
    {
        [ShowInInspector, ReadOnly] private Dictionary<string, List<BlockData>> _blocksData;

        [SerializeField] private List<Stack> stacks;

        public event Action<IClickableStack> OnStackSelected;
        public event Action<IClickableBlock> OnBlockSelected;
        public event Action<IClickableBlock> OnBlockHovered;

        private IClickableStack _selectedStack;
        private IClickableBlock _selectedBlock;
        private IClickableBlock _hoveredBlock;

        protected override void Awake()
        {
            base.Awake();
            DataManager.Instance.GetStackData(OnStackDataLoaded);
        }

        private void OnStackDataLoaded(Dictionary<string, List<BlockData>> blocksData)
        {
            _blocksData = blocksData;
            BuildStacks();
        }

        private void BuildStacks()
        {
            var stackId = 0;
            foreach (var blockDataKey in _blocksData.Keys)
            {
                if (stackId >= stacks.Count) break;

                stacks[stackId].InitializeStack(_blocksData[blockDataKey]);
                stackId++;
            }
        }

        public void SelectStack(IClickableStack stack)
        {
            _selectedStack = stack;
            OnStackSelected?.Invoke(stack);
        }

        public void SelectBlock(IClickableBlock block)
        {
            _selectedBlock = block;
            OnBlockSelected?.Invoke(block);
        }
        
        public void HoverBlock(IClickableBlock block)
        {
            if(_hoveredBlock == block) return;
            _hoveredBlock = block;
            OnBlockHovered?.Invoke(block);
        }
    }
}