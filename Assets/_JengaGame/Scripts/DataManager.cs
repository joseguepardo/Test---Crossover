using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace JengaGame
{
    public class DataManager : Singleton<DataManager>
    {
        private readonly string DataURL = "https://ga1vqcu3o1.execute-api.us-east-1.amazonaws.com/Assessment/stack";

        public List<StackData> StacksData { get; private set; }

        public void GetStackData(Action<List<StackData>> onStackDataLoaded)
        {
            StartCoroutine(GetStackDataCo(onStackDataLoaded));
        }

        private IEnumerator GetStackDataCo(Action<List<StackData>> onStackDataLoaded)
        {
            var webRequest = UnityEngine.Networking.UnityWebRequest.Get(DataURL);
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
            {
                InitializeStacksData(webRequest.downloadHandler.text);
            }
            else
            {
                Debug.LogError(webRequest.error);
                Debug.Log($"Loading data from backup file...");
                InitializeStacksData(Resources.Load<TextAsset>("stack").text);
            }

            onStackDataLoaded?.Invoke(StacksData);
        }

        private void InitializeStacksData(string json)
        {
            StacksData = new List<StackData>();
            var jsonNode = SimpleJSON.JSON.Parse(json);
            for (var i = 0; i < jsonNode.Count; i++)
            {
                var stackData = new StackData(
                    jsonNode[i]["id"],
                    jsonNode[i]["subject"],
                    jsonNode[i]["grade"],
                    jsonNode[i]["mastery"].AsInt,
                    jsonNode[i]["domainid"],
                    jsonNode[i]["domain"],
                    jsonNode[i]["cluster"],
                    jsonNode[i]["standardid"],
                    jsonNode[i]["standarddescription"]);
                StacksData.Add(stackData);
            }
        }
    }
}