using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public static Action OnSystemsInitialized;

    private IEnumerator Start() {
        yield return null;
        OnSystemsInitialized?.Invoke();
    }
}
