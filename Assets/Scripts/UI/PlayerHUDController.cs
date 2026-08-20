using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHUDController : MonoBehaviour
{
    [System.Serializable]
    public class Column
    {
        public TextMeshProUGUI m_title;
        public TextMeshProUGUI m_value;
    }

    public Column[] m_columns;

    enum ColumnEnum
    {
        Lives,
        Score,
        Kills
    }

    public void SetEnabled(bool enabled)
    {
        GetComponent<Canvas>().enabled = enabled;
    }

    public void SetColour(Color colour)
    {
        foreach(Column column in m_columns)
        {
            column.m_title.color = colour;
            column.m_value.color = colour;
        }
    }

    public void SetLivesValue(int livesValue)
    {
        m_columns[(int)ColumnEnum.Lives].m_value.text = livesValue.ToString();
    }

    public void SetScoreValue(int scoreValue)
    {
        m_columns[(int)ColumnEnum.Score].m_value.text = scoreValue.ToString();
    }

    public void SetKillsValue(int killsValue)
    {
        m_columns[(int)ColumnEnum.Kills].m_value.text = killsValue.ToString();
    }
}
