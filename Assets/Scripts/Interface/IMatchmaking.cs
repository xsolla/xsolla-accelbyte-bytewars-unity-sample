

using System;

public interface IMatchmaking
{
    public void StartMatchmaking(InGameMode inGameMode, Action<MatchmakingResult> onMatchmakingFinished);
    public void CancelMatchmaking();
}
