// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MainMenu : MenuCanvas
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button helpAndOptionsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private ScrollRect buttonsScrollRect;
    
    public static event Action<Action> OnQuitPressed;

    private void OnEnable()
    {
        StartCoroutine(ScrollButtonsToTop());
    }

    private void Start()
    {
        playButton.onClick.AddListener(OnPlayButtonPressed);
        helpAndOptionsButton.onClick.AddListener(OnHelpAndOptionsButtonPressed);
        quitButton.onClick.AddListener(OnQuitButtonPressed);
    }

    private static void OnPlayButtonPressed()
    {
        MenuManager.Instance.ChangeToMenu(AssetEnum.PlayMenuCanvas);
    }
    
    private static void OnHelpAndOptionsButtonPressed()
    {
        MenuManager.Instance.ChangeToMenu(AssetEnum.HelpAndOptionsMenuCanvas);
    }
    
    private static void OnQuitButtonPressed()
    {
        MenuManager.Instance.PromptMenu.ShowPromptMenu(
            header: "Quit Game", 
            message: "Are you sure you want to quit the game?", 
            cancelText: "No", 
            cancelAction: null,
            confirmText: "Yes", 
            confirmAction: () =>
            {
                bool authEssentialsActive = TutorialModuleManager.Instance.IsModuleActive(TutorialType.AuthEssentials);
                if (authEssentialsActive)
                {
                    OnQuitPressed?.Invoke(QuitGame);
                }
                else
                {
                    QuitGame();
                }
            });
    }

    private static void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    public override GameObject GetFirstButton()
    {
        return playButton.gameObject;
    }

    public override AssetEnum GetAssetEnum()
    {
        return AssetEnum.MainMenuCanvas;
    }

    private IEnumerator ScrollButtonsToTop()
    {
        yield return new WaitForEndOfFrame();

        if (buttonsScrollRect == null)
        {
            yield break;
        }

        Canvas.ForceUpdateCanvases();
        if (buttonsScrollRect.content != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(buttonsScrollRect.content);
        }
        Canvas.ForceUpdateCanvases();

        buttonsScrollRect.verticalNormalizedPosition = 1f;
    }
}
