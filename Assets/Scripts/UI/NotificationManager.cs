using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotificationManager : MonoBehaviour
{
    public Button acceptButton;
    public Button rejectButton;
    public TextMeshProUGUI message;
    public bool sessionTimeRunning;
    public GameObject notificationPanel;
    [SerializeField] public float sessionTimeRemaining;
    
    // Start is called before the first frame update
    void Start()
    {
        sessionTimeRunning = true;
        acceptButton.onClick.AddListener(OnAcceptButtonPressed);
        rejectButton.onClick.AddListener(OnRejectButtonPressed);
    }

    // Update is called once per frame
    void Update()
    {
        if (sessionTimeRunning)
        {
            if (sessionTimeRemaining > 0)
            {
                sessionTimeRemaining -= Time.deltaTime;
                message.text = "User invited you to join their Game " + sessionTimeRemaining.ToString();
            }
            else
            {
                Debug.Log("Timeout");
                sessionTimeRemaining = 0;
                sessionTimeRunning = false;
                OnSessionTimeOut();
            }
        }
    }
    public void OnAcceptButtonPressed()
    {
        OnSessionTimeOut();
    }

    public void OnRejectButtonPressed()
    {
        OnSessionTimeOut();
    }
    public void OnSessionTimeOut()
    {
        notificationPanel.SetActive(false);
        //Destroy(notificationPanel);
    }
}
