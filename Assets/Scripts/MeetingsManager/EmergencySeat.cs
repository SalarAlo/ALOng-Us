using UnityEngine;

public class EmergencySeat : MonoBehaviour
{
    private Player occupiedBy;
    private Vector3 posBefore;
    public void FillSeat(Player player){
        posBefore = player.GetPos();
        occupiedBy = player;

        player.GetComponent<PlayerController>().SetCanMove(false);
        player.transform.position = transform.position;
        player.transform.localScale = transform.lossyScale;
    }

    public bool IsOccupied() => occupiedBy != null;

    public void ClearSeat() {
        occupiedBy.transform.localScale = Vector3.one;
        occupiedBy.transform.position = posBefore;
        occupiedBy.GetComponent<PlayerController>().SetCanMove(true);
        occupiedBy = null;
    }
}
