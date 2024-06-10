using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUI : BaseUISingleton<MainUI>
{
    public override void Awake()
    {
        base.Awake();
        Show();
    }
}
