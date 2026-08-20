// Copyright (c) 2026 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Linq;
using AccelByte.Api;
using AccelByte.Core;
using AccelByte.Models;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Xsolla.AccelByte;

public class XsollaAuthWrapper : MonoBehaviour
{
    private const string XsollaButtonLabel = "Login with Xsolla";
    private const string XsollaSteamButtonLabel = "Login with Steam";

    private User user;
    private UserProfiles userProfiles;
    private Lobby lobby;
    private bool isXsollaAuthenticated;

    private void Awake()
    {
        user = AccelByteSDK.GetClientRegistry().GetApi().GetUser();
        userProfiles = AccelByteSDK.GetClientRegistry().GetApi().GetUserProfiles();
        lobby = AccelByteSDK.GetClientRegistry().GetApi().GetLobby();

        MainMenu.OnQuitPressed += OnQuitPressed;

        AssignXsollaAuthButtonsCallback();
    }

    private void OnDestroy()
    {
        MainMenu.OnQuitPressed -= OnQuitPressed;
    }

    private async void AssignXsollaAuthButtonsCallback()
    {
        while (!(MenuManager.Instance?.IsInitiated ?? false) ||
            MenuManager.Instance?.GetCurrentMenu()?.GetAssetEnum() != AssetEnum.LoginMenu)
        {
            await UniTask.Yield();
        }

        LoginMenu loginMenu = MenuManager.Instance.GetCurrentMenu() as LoginMenu;
        if (!loginMenu)
        {
            return;
        }

        SinglePlatformAuthWrapper singlePlatformAuthWrapper =
            TutorialModuleManager.Instance.GetModuleClass<SinglePlatformAuthWrapper>();
        if (singlePlatformAuthWrapper)
        {
            await singlePlatformAuthWrapper.WaitUntilButtonSetupComplete();
        }
        else
        {
            BytewarsLogger.LogWarning(
                "XsollaAuthWrapper could not find an active SinglePlatformAuthWrapper (it may be running in starter mode). " +
                "The Login with Steam via Xsolla button will be hidden.");
        }

        loginMenu.GetLoginButton(AuthEssentialsModels.LoginType.DeviceId)?.gameObject.SetActive(false);
        loginMenu.GetLoginButton(AuthEssentialsModels.LoginType.SinglePlatformAuth)?.gameObject.SetActive(false);

        Button xsollaButton = loginMenu.GetComponentsInChildren<Button>(true)
            .FirstOrDefault(button => button.name == XsollaButtonLabel);
        if (xsollaButton)
        {
            xsollaButton.onClick.AddListener(() => OnLoginWithXsollaAccountButtonClicked(loginMenu));
        }

        Button xsollaSteamButton = loginMenu.GetComponentsInChildren<Button>(true)
            .FirstOrDefault(button => button.name == XsollaSteamButtonLabel);
#if !UNITY_WEBGL
        bool isSteamAvailable = singlePlatformAuthWrapper && singlePlatformAuthWrapper.IsSteamAvailable;
        xsollaSteamButton?.gameObject.SetActive(isSteamAvailable);
        if (xsollaSteamButton && isSteamAvailable)
        {
            xsollaSteamButton.onClick.AddListener(() =>
                OnLoginWithXsollaSteamButtonClicked(loginMenu, singlePlatformAuthWrapper));
        }
#else
        xsollaSteamButton?.gameObject.SetActive(false);
#endif
    }

    private void OnLoginWithXsollaAccountButtonClicked(LoginMenu loginMenu)
    {
        loginMenu.WidgetSwitcher.SetWidgetState(AccelByteWarsWidgetSwitcher.WidgetState.Loading);
        loginMenu.OnRetryLoginClicked = () => OnLoginWithXsollaAccountButtonClicked(loginMenu);

        AccelByteSDK.GetClientRegistry()
            .GetApi()
            .GetXsollaAuth()
            .LoginWithXsollaAccount(result => OnLoginCompleted(loginMenu, result));
    }

#if !UNITY_WEBGL
    private void OnLoginWithXsollaSteamButtonClicked(LoginMenu loginMenu, SinglePlatformAuthWrapper singlePlatformAuthWrapper)
    {
        loginMenu.WidgetSwitcher.SetWidgetState(AccelByteWarsWidgetSwitcher.WidgetState.Loading);
        loginMenu.OnRetryLoginClicked = () => OnLoginWithXsollaSteamButtonClicked(loginMenu, singlePlatformAuthWrapper);

        const string byteWarsIdentity = "ByteWars";
        singlePlatformAuthWrapper.GetSteamAuthTicket(byteWarsIdentity, steamAuthResult =>
        {
            if (steamAuthResult.IsError)
            {
                OnLoginCompleted(loginMenu, Result<TokenData, OAuthError>.CreateError(new OAuthError()
                {
                    error = steamAuthResult.Error.Code.ToString(),
                    error_description = steamAuthResult.Error.Message
                }));
                return;
            }

            const string providerName = "Steam";
            string gameAppId = GConfig.GetSteamAppId();
            AccelByteSDK.GetClientRegistry()
                .GetApi()
                .GetXsollaAuth()
                .LoginWithXsollaSilentAuth(
                    providerName,
                    gameAppId,
                    steamAuthResult.Value,
                    result => OnLoginCompleted(loginMenu, result));
        });
    }
#endif

    private void CreateOrGetUserProfile(ResultCallback<UserProfile> resultCallback)
    {
        userProfiles.GetUserProfile((Result<UserProfile> getUserProfileResult) =>
        {
            if (getUserProfileResult.IsError &&
                getUserProfileResult.Error.Code == ErrorCode.UserProfileNotFoundException)
            {
                CreateUserProfileRequest request = new CreateUserProfileRequest()
                {
                    language = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName,
                    timeZone = TutorialModuleUtil.GetLocalTimeOffsetFromUTC(),
                };
                userProfiles.CreateUserProfile(request, resultCallback);
                return;
            }

            resultCallback.Invoke(getUserProfileResult);
        });
    }

    private void GetUserPublicData(string userId, ResultCallback<AccountUserPlatformInfosResponse> resultCallback)
    {
        user.GetUserOtherPlatformBasicPublicInfo("ACCELBYTE", new string[] { userId }, resultCallback);
    }

    private void OnLoginCompleted(LoginMenu loginMenu, Result<TokenData, OAuthError> loginResult)
    {
        if (loginResult.IsError)
        {
            BytewarsLogger.Log($"Failed to login with Xsolla. Error: {loginResult.Error.error}");
            loginMenu.OnLoginCompleted(loginResult);
            return;
        }

        isXsollaAuthenticated = true;

        if (!lobby.IsConnected)
        {
            lobby.Connect();
        }

        TokenData tokenData = loginResult.Value;
        BytewarsLogger.Log("Success to login with Xsolla. Querying the user profile and user info.");
        CreateOrGetUserProfile((Result<UserProfile> userProfileResult) =>
        {
            if (userProfileResult.IsError)
            {
                BytewarsLogger.LogWarning($"Failed to create or get user profile. Error: {userProfileResult.Error.Message}");
                loginMenu.OnLoginCompleted(Result<TokenData, OAuthError>.CreateError(new OAuthError() { error = userProfileResult.Error.Message }));
                return;
            }

            GetUserPublicData(tokenData.user_id, (Result<AccountUserPlatformInfosResponse> userInfoResult) =>
            {
                if (userInfoResult.IsError || userInfoResult.Value.Data.Length <= 0)
                {
                    BytewarsLogger.LogWarning($"Failed to get user info. Error: {userInfoResult.Error.Message}");
                    loginMenu.OnLoginCompleted(Result<TokenData, OAuthError>.CreateError(new OAuthError() { error = userInfoResult.Error.Message }));
                    return;
                }

                AccountUserPlatformData publicUserData = userInfoResult.Value.Data[0];
                GameData.CachedPlayerState.PlayerId = publicUserData.UserId;
                GameData.CachedPlayerState.AvatarUrl = publicUserData.AvatarUrl;
                GameData.CachedPlayerState.PlayerName = AccelByteWarsOnlineUtility.GetDisplayName(publicUserData);
                GameData.CachedPlayerState.PlatformId =
                    string.IsNullOrEmpty(GameData.CachedPlayerState.PlatformId) ?
                    tokenData.platform_id : GameData.CachedPlayerState.PlatformId;

                loginMenu.OnLoginCompleted(loginResult);
            });
        });
    }

    private void OnQuitPressed(Action callback)
    {
        if (isXsollaAuthenticated)
        {
            AccelByteSDK.GetClientRegistry().GetApi().GetXsollaAuth().Logout(result =>
            {
                isXsollaAuthenticated = false;
                if (result.IsError)
                {
                    BytewarsLogger.LogWarning($"Failed to logout from Xsolla. Error message: {result.Error.Message}");
                }
                else
                {
                    BytewarsLogger.Log("Xsolla logout successful");
                }
            });
        }
    }
}
