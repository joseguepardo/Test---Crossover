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

                stacks[stackId].BuildStack(_blocksData[blockDataKey]);
                stackId++;
            }
        }
    }
}