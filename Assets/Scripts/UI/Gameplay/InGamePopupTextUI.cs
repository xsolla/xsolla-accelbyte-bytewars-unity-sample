using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGamePopupTextUI : FxEntity
{
    [SerializeField]
    private RectTransform parentPanel;
    [SerializeField]
    private TMPro.TextMeshProUGUI textUI;
    [SerializeField] 
    private RectTransform rectTransform;
    [SerializeField]
    private float Speed = 2.5f;
    [SerializeField]
    private float LifeTime = 2.0f;

    [SerializeField] private AudioSource sfx;

    float m_timeAlive = 0.0f;

    Vector3 m_worldPosition;
    private Camera _camera;

    public void Init(Vector3 worldPosition, Color colour, string text)
    {
        if (!_camera)
        {
            _camera = GameManager.Instance.MainCamera;
        }
        m_worldPosition = worldPosition;
        Vector2 screenPoint = WorldToCanvas(worldPosition + new Vector3(1.5f, 0.5f, 0.0f),_camera);
        parentPanel.anchoredPosition = screenPoint;

        textUI.text = text;
        textUI.color = colour;
        float volume = AudioManager.Instance.GetCurrentVolume(AudioManager.AudioType.SfxAudio);
        if (volume > 0.01f)
        {
            sfx.volume = volume;
            sfx.Play();
        }
    }

    void Update()
    {
        m_timeAlive += Time.deltaTime;

        float timeFraction = Mathf.InverseLerp(LifeTime, 0.0f, m_timeAlive);

        float alpha = timeFraction * 0.8f;

        Color textColour = textUI.color;
        textColour.a = alpha;
        textUI.color = textColour;

        m_worldPosition.y += Speed * Time.deltaTime;

        SetPosition(m_worldPosition);

        if( m_timeAlive > LifeTime )
        {
            Reset();
        }
    }

    Vector2 WorldToCanvas( Vector3 world_position, Camera camera)
     {
         
         var viewport_position = camera.WorldToViewportPoint(world_position);
         var sizeDelta = rectTransform.sizeDelta;
         return new Vector2((viewport_position.x * sizeDelta.x) - (sizeDelta.x * 0.5f),
                            (viewport_position.y * sizeDelta.y) - (sizeDelta.y * 0.5f));
     }

    public void SetPosition(Vector3 worldPosition)
    {
        m_worldPosition = worldPosition;
        Vector2 screenPoint = WorldToCanvas(worldPosition + new Vector3(1.5f, 0.5f, 0.0f),_camera);
        parentPanel.anchoredPosition = screenPoint;
    }

    public override void Reset()
    {
        m_timeAlive = 0;
        gameObject.SetActive(false);
    }
}
