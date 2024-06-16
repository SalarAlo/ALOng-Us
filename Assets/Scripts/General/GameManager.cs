using System;

public class GameManager : Singleton<GameManager>
{
    public Action<GameState> OnGameStateChanged;
    private GameState gameState;

    public GameState GetGameState() => gameState;
    public void SetGameState(GameState newState) {
        OnGameStateChanged?.Invoke(newState);
        gameState = newState;
    }
}