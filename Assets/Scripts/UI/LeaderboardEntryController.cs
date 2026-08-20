using UnityEngine;
using TMPro;

public class LeaderboardEntryController : MonoBehaviour
{
    public TextMeshProUGUI m_nameTextUI;
    public TextMeshProUGUI m_killCountTextUI;
    public TextMeshProUGUI m_scoreTextUI;
    public void SetDetails(string playerName, Color playerColour, int killCount, int score )
    {
        m_nameTextUI.text = playerName;
        m_killCountTextUI.text = killCount.ToString();
        m_scoreTextUI.text = score.ToString();

        m_nameTextUI.color = playerColour;
        m_killCountTextUI.color = playerColour;
        m_scoreTextUI.color = playerColour;
    }
}
