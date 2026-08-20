// Copyright (c) 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Models;

public static class AccelByteWarsOnlineUtility
{
    public static string GetDisplayName(AccountUserPlatformData userData) =>
        !string.IsNullOrEmpty(userData.DisplayName) ? userData.DisplayName :
        !string.IsNullOrEmpty(userData.UniqueDisplayName) ? userData.UniqueDisplayName :
        AccelByteWarsUtility.GetDefaultDisplayNameByUserId(userData.UserId);

    public static string GetDisplayName(PublicUserInfo publicUserInfo) =>
        !string.IsNullOrEmpty(publicUserInfo.displayName) ? publicUserInfo.displayName :
        !string.IsNullOrEmpty(publicUserInfo.UniqueDisplayName) ? publicUserInfo.UniqueDisplayName :
        AccelByteWarsUtility.GetDefaultDisplayNameByUserId(publicUserInfo.userId);

    public static string GetDisplayName(BaseUserInfo userInfo) =>
        !string.IsNullOrEmpty(userInfo.displayName) ? userInfo.displayName :
        !string.IsNullOrEmpty(userInfo.UniqueDisplayName) ? userInfo.UniqueDisplayName :
        AccelByteWarsUtility.GetDefaultDisplayNameByUserId(userInfo.userId);

    /* Xsolla access token of the signed in player, used to authenticate them on the Xsolla web shop.
     * This is the Xsolla token, not the AGS one. The Xsolla SDK owns it, so it is read on demand rather
     * than cached at login: every login path the game uses (the Xsolla login widget and Steam silent
     * auth) builds its result from this same value. Null or empty until an Xsolla login succeeds. */
    public static string GetXsollaAccessToken() => Xsolla.Core.XsollaToken.AccessToken;
}
