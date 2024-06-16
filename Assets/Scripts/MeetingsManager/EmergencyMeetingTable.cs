using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyMeetingTable : MonoBehaviour, IUseable
{
    public void Use(){
        EmergencyMeetingUI.Instance.Show();
    }
}
