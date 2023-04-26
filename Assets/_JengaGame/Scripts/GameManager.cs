using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace JengaGame
{
    public class GameManager : Singleton<GameManager>
    {
        [ShowInInspector, ReadOnly] private List<StackData> _stacksData;

        protected override void Awake()
        {
            base.Awake();
            DataManager.Instance.GetStackData(OnStackDataLoaded);
        }

        private void OnStackDataLoaded(List<StackData> stacksData)
        {
            _stacksData = stacksData;
        }
    }
}