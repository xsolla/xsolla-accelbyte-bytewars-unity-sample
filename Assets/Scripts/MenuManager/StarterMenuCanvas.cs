using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarterMenuCanvas : MonoBehaviour
{
    [SerializeField] private ButtonAnimation buttonPrefab;

    [SerializeField] private RectTransform buttonPanel;
    [SerializeField] private TMPro.TMP_Text additionalInfo;
    private Dictionary<string, ButtonAnimation> instantiatedButtons= new Dictionary<string, ButtonAnimation>();
    private string infoStr;
    private void OnEnable()
    {
        additionalInfo.gameObject.SetActive(!String.IsNullOrEmpty(infoStr));
    }

    public void InstantiateButtons(MenuButtonData[] buttonsData, string message=null)
    {
        if (instantiatedButtons.Count == buttonsData.Length)
            return;
        if (!String.IsNullOrEmpty(message))
        {
            SetAdditionalInfo(message);
        }
        foreach (var buttonData in buttonsData)
        {
            var instantiatedButton = Instantiate(buttonPrefab, Vector3.zero, Quaternion.identity, buttonPanel);
            instantiatedButton.name = buttonData.name;
            if(buttonData.callback!=null)
            instantiatedButton.button.onClick.AddListener(buttonData.callback);
            instantiatedButton.text.text = buttonData.label;
            if (!instantiatedButtons.TryGetValue(buttonData.name, out var btn))
            {
                instantiatedButtons[buttonData.name] = instantiatedButton;
            }
        }
    }

    public void SetButtonsCallback(MenuButtonData[] buttonsData)
    {
        foreach (var buttonData in buttonsData)
        {
            ButtonAnimation buttonAnimation;
            if (instantiatedButtons.TryGetValue(buttonData.name, out buttonAnimation))
            {
                if(buttonData.callback!=null)
                buttonAnimation.button.onClick.AddListener(buttonData.callback);
            }
        }
    }

    public void SetAdditionalInfo(string info)
    {
        if (!String.IsNullOrEmpty(info))
        {
            infoStr = info;
            additionalInfo.text = infoStr;
            additionalInfo.gameObject.SetActive(true);
        }
    }
}
