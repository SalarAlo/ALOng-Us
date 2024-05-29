using System;
using UnityEngine;

public abstract class BaseUI : MonoBehaviour
{
    public static Action OnAnyWindowOpened;
    [SerializeField] private GameObject ownWindow;

    protected virtual void Awake() {
        Hide();
    }

    public virtual void Show() {
        ownWindow.SetActive(true);
        OnAnyWindowOpened?.Invoke();
    }

    public virtual void Hide() {
        ownWindow.SetActive(false);
    }
}


public abstract class BaseUISingletonPersistent<T> : SingletonPersistent<T> where T : Component
{
    [SerializeField] private GameObject ownWindow;

    public override void Awake() {
        base.Awake();
        Hide();
    }

    public virtual void Show() {
        ownWindow.SetActive(true);
        BaseUI.OnAnyWindowOpened?.Invoke();
    }

    public virtual void Hide() {
        ownWindow.SetActive(false);
    }
}
