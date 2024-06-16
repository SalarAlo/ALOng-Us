using UnityEngine.UI;

public class SabotageUI : BaseUISingleton<SabotageUI>
{
    public Button colorBlindSabotage;
    public Button closeSabotage;
    
    private void Start() {
        Player.OnLocalInstanceInitialised += Player_OnLocalInstanceInitialised;
        closeSabotage.onClick.AddListener(Hide);
    }

    private void Player_OnLocalInstanceInitialised() {
        PlayerRole role = Player.LocalInstance.GetComponent<PlayerRoleManager>().GetRole();
        if (role != PlayerRole.Imposter) return;
        colorBlindSabotage.onClick.AddListener(ColorBlindSabotage_OnClick);
    }

    private void ColorBlindSabotage_OnClick() {
        PlayerData playerDataColorblind = new() {
            colorIndex = 0,
            playerName = "X"
        };

        var networkedPlayerDataList = AlongUsMultiplayer.Instance.networkedPlayerDataList;

        for(int i = 0; i < networkedPlayerDataList.Count; i++){
            var originalPlayerData = networkedPlayerDataList[i];
            if(i != networkedPlayerDataList.Count-1){
                AlongUsMultiplayer.Instance.ChangePlayerAppearanceTo(
                    originalPlayerData.clientId,
                    playerDataColorblind,
                    10, 
                    originalPlayerData
                );
                continue;
            }

            //TODO: NOTIFY GAMEMANAGER OF BACK TO REGULAR (RPCS!)

            // Only invoke this on the last playerData to make sure that if the player is back to normal 
            // we notify the gamestate that its back to regular instead of sabotage
            AlongUsMultiplayer.Instance.ChangePlayerAppearanceTo(
                originalPlayerData.clientId,
                playerDataColorblind,
                10, 
                originalPlayerData
            );
        }

        GameManager.Instance.SetGameState(GameState.Sabotage);
        Hide();
    }

    public override void Show() {
        base.Show();
        Player.LocalInstance.GetComponent<PlayerController>().DisableControls();
    }

    public override void Hide() {
        base.Hide();
        if(Player.LocalInstance != null)
            Player.LocalInstance.GetComponent<PlayerController>().EnableControls();
    }
}
