using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpMenu : MenuCanvas
{
    [SerializeField] private Button backButton;
    
    void Start()
    {
        backButton.onClick.AddListener(() => MenuManager.Instance.OnBackPressed());   
    }

    public override GameObject GetFirstButton()
    {
        return backButton.gameObject;
    }

    public override AssetEnum GetAssetEnum()
    {
        return AssetEnum.HelpMenuCanvas;
    }
}
