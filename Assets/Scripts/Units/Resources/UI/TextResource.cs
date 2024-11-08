using System;
using Manager;
using TMPro;
using UnityEngine;

namespace Units.Resources.UI
{
    [Serializable]
    public class TextResource
    {
        [HideInInspector] public string name;
        [SerializeField] private ResourcesManager.ResourceType _resourceType;
        [SerializeField] private TextMeshProUGUI[] _resourceTexts;
        [SerializeField] private string _preFix;

        public ResourcesManager.ResourceType ResourceType => _resourceType;

        public void SetText(int amount)
        {
            foreach (TextMeshProUGUI text in _resourceTexts)
            {
                text.text = $"{_preFix} {amount}";
            }
        }
    }
}