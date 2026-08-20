using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoMenuCanvas : MenuCanvas
{
    [SerializeField] private Button okBtn;
    [SerializeField] private TextMeshProUGUI infoLabel;
    [SerializeField] private TextMeshProUGUI titleLabel;
    
    void Start()
    {
        okBtn.onClick.AddListener(MenuManager.Instance.HideInfo);
    }

    public void Show(string info, string title)
    {
        infoLabel.text = info;
        titleLabel.text = title;
        gameObject.SetActive(true);
    }

    public override GameObject GetFirstButton()
    {
        return okBtn.gameObject;
    }

    public override AssetEnum GetAssetEnum()
    {
        return AssetEnum.InfoMenuCanvas;
    }
}
