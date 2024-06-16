using UnityEngine;

public class EmergencySeat : MonoBehaviour
{
    private Player occupiedBy;
    private Vector3 posBefore;
    public void FillSeat(Player player){
        posBefore = player.GetPos();
        occupiedBy = player;
        player.transform.parent = transform;
        player.transform.localScale = Vector3.one;
        player.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(0, 0, 0));
    }

    public bool IsOccupied() => occupiedBy != null;

    public void ClearSeat() => occupiedBy = null;
}
