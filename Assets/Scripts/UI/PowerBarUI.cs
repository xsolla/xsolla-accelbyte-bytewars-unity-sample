using UnityEngine;
using UnityEngine.UI;

public class PowerBarUI : MonoBehaviour
{
 public RectTransform m_parentPanel;
    public RectTransform m_powerBarPanel;
    Vector2 m_powerBarPanelFullHeight;
    [SerializeField] private RectTransform canvasRect;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Image[] images;
    float m_displayTimer = 0.0f;
    public void Init()
    {
        m_powerBarPanelFullHeight = m_parentPanel.sizeDelta;
        
        SetVisible(false);
    }

    void Update()
    {
        if( m_displayTimer > 0.0f )
        {
            m_displayTimer = Mathf.Max( 0.0f, m_displayTimer - Time.deltaTime );

            if( m_displayTimer == 0.0f )
            {
                SetVisible(false);
            }
        }
    }

    void SetVisible(bool visible)
    {
        canvas.enabled = visible;
    }

    public void SetColour(Color colour)
    {
        foreach( Image image in images )
        {
            var alpha = image.color.a;
            image.color = new Color(colour.r,colour.g,colour.b,alpha);
        }
    }

    public void SetPercentageFraction(float percentageFraction, bool showUI = true)
    {
        Vector2 newSize = m_powerBarPanelFullHeight * new Vector2( 1.0f, percentageFraction );

        m_powerBarPanel.sizeDelta = newSize;

        if( showUI )
        {
            m_displayTimer = 0.5f;
            SetVisible(true);
        }
    }

     public Vector2 WorldToCanvas( Vector3 worldPosition)
     {
         var viewportPosition = GameManager.Instance.MainCamera.WorldToViewportPoint(worldPosition);
         var sizeDelta = canvasRect.sizeDelta;
         return new Vector2((viewportPosition.x * sizeDelta.x) - (sizeDelta.x * 0.5f),
                            (viewportPosition.y * sizeDelta.y) - (sizeDelta.y * 0.5f));
     }

    public void SetPosition(Vector3 worldPosition)
    {
        Vector2 screenPoint = WorldToCanvas(worldPosition + new Vector3(1.5f, 0.5f, 0.0f));
        m_parentPanel.anchoredPosition = screenPoint;
    }
}
