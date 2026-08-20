// Copyright (c) 2026 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;

/// <summary>
/// Keeps the in-game browser on the web shop's own domain, so it cannot be used as a general web browser.
/// </summary>
/// <remarks>
/// Unreal detects an off-domain URL only after navigating and reacts by tearing the browser down. The Xsolla
/// SDK lets us refuse the navigation up front, so the browser stays open on the storefront instead.
/// </remarks>
public class XsollaWebshopDomainGuard : Xsolla.Core.IInAppBrowserNavigationInterceptor
{
    public string AllowedHost { get; }

    public bool HasBlockedNavigation { get; private set; }

    public XsollaWebshopDomainGuard(string allowedUrl)
    {
        AllowedHost = ExtractHost(allowedUrl);
    }

    public bool ShouldAbortNavigation(string url)
    {
        string host = ExtractHost(url);

        /* Exact host or a real subdomain of it. Unreal uses a substring test, which also accepts a host
         * such as "bytewars.xsolla.site.example.net". */
        bool isAllowed = host.Equals(AllowedHost, StringComparison.Ordinal)
            || host.EndsWith($".{AllowedHost}", StringComparison.Ordinal);

        if (isAllowed)
        {
            return false;
        }

        HasBlockedNavigation = true;

        // Hosts only. The web shop URL carries the access token in its query string.
        BytewarsLogger.LogWarning($"Blocked in-game browser navigation to {host}. Expected {AllowedHost}.");

        return true;
    }

    private static string ExtractHost(string url)
    {
        string host = Uri.TryCreate(url, UriKind.Absolute, out Uri uri) ? uri.Host : url;
        host = host.ToLowerInvariant();

        return host.StartsWith("www.", StringComparison.Ordinal) ? host.Substring("www.".Length) : host;
    }
}
