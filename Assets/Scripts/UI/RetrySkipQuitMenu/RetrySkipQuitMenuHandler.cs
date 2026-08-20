using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RetrySkipQuitMenuHandler : MonoBehaviour
{
    [SerializeField] private Button retryBtn;

    [SerializeField] private Button skipBtn;

    [SerializeField] private Button quitBtn;
    [SerializeField] private TMPro.TMP_Text infoText;

    public void SetData(UnityAction retryCallback, UnityAction skipCallback, string message=null)
    {
        retryBtn.onClick.AddListener(retryCallback);
        skipBtn.onClick.AddListener(skipCallback);
        quitBtn.onClick.AddListener(Application.Quit);
        if (String.IsNullOrEmpty(message))
        {
            infoText.gameObject.SetActive(false);
        }
        else
        {
            infoText.text = message;
            infoText.gameObject.SetActive(true);
        }
    }
}
