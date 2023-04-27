using System;
using System.Collections;
using System.Collections.Generic;
using Chronos;
using Sirenix.OdinInspector;
using UnityEngine;

namespace JengaGame
{
    public class GameManager : Singleton<GameManager>
    {
        [ShowInInspector, ReadOnly] private Dictionary<string, List<BlockData>> _blocksData;

        [SerializeField] private List<Stack> stacks;

        public event Action<IStack> OnStackSelected;
        public event Action<IBlock> OnBlockSelected;
        public event Action<IBlock> OnBlockHovered;
        public event Action OnStackTestStarted, OnStackTestFinished;

        private IStack _selectedStack;
        private IBlock _selectedBlock;
        private IBlock _hoveredBlock;

        private Clock _clock;
        private Clock[] _stackClocks;
        private Coroutine _stackTestCoroutine;

        protected override void Awake()
        {
            base.Awake();
            DataManager.Instance.GetStackData(OnStackDataLoaded);
            _clock = Timekeeper.instance.Clock("Game");
            _stackClocks = new Clock[]
            {
                Timekeeper.instance.Clock("Stack1"),
                Timekeeper.instance.Clock("Stack2"),
                Timekeeper.instance.Clock("Stack3")
            };
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

                stacks[stackId].InitializeStack(_blocksData[blockDataKey], _stackClocks[stackId] as GlobalClock);
                stackId++;
            }
        }

        public void SelectStack(IStack stack)
        {
            _selectedStack = stack;
            OnStackSelected?.Invoke(stack);
        }

        public void SelectBlock(IBlock block)
        {
            _selectedBlock = block;
            OnBlockSelected?.Invoke(block);
        }

        public void HoverBlock(IBlock block)
        {
            if (_hoveredBlock == block) return;
            _hoveredBlock = block;
            OnBlockHovered?.Invoke(block);
        }

        [Button]
        public void TestStack()
        {
            if (_selectedStack == null) return;
            _stackTestCoroutine = StartCoroutine(TestStackCo());
        }

        private IEnumerator TestStackCo()
        {
            OnStackTestStarted?.Invoke();
            _selectedStack.PrepareForTest();
            var clock = _selectedStack.Clock;
            clock.paused = false;
            yield return new WaitForSeconds(20);
            clock.paused = true;
            _stackTestCoroutine = null;
        }

        [Button]
        public void ResetStack()
        {
            if (_selectedStack == null) return;
            StartCoroutine(ResetStackCo());
        }

        private IEnumerator ResetStackCo()
        {
            if (_stackTestCoroutine != null) StopCoroutine(_stackTestCoroutine);
            var clock = _selectedStack.Clock;
            clock.LerpTimeScale(-3.5f, 0.5f);
            clock.paused = false;

            yield return new WaitUntil(() => clock.time <= 0.01f);
            clock.LerpTimeScale(1, 0);
            clock.paused = true;
            _selectedStack.Reset();
            OnStackTestFinished?.Invoke();
        }
    }
}