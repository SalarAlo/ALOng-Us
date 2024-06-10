using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EmoteWheelUI : BaseUISingleton<EmoteWheelUI>
{
    private const int OPTIONS_COUNT = 8;
    [SerializeField] private List<SingleWheelOption> singleWheelOptions;
    [SerializeField] private SingleOptionWheelUI singleOptionWheelUIPrefab;

    private void OnValidate() {
        if (singleWheelOptions.Count < OPTIONS_COUNT){
            int missing = OPTIONS_COUNT - singleWheelOptions.Count;

            for(int i = 0; i < missing; i++){
                singleWheelOptions.Add(new SingleWheelOption());
            }
        } else if (singleWheelOptions.Count > OPTIONS_COUNT){
            int over = singleWheelOptions.Count - OPTIONS_COUNT;
            for(int i = 0; i < over; i++){
                singleWheelOptions.RemoveAt(singleWheelOptions.Count-1);
            }
        }
    }

    public bool GetIsOpen() => isOpen;

    public override void Awake() {
        Player.OnLocalInstanceInitialised += Player_OnLocalInstanceInitialised;
    }

    private void Start() {
        GameInput.Instance.SubscribeToEmote(GameInput_SubscribeToEmote);
    }

    private void GameInput_SubscribeToEmote() {
        if(isOpen) Hide();
        else Show();
    }

    private void Player_OnLocalInstanceInitialised(){
        Initialize();
    }

    private void Initialize(){
        foreach(Transform t in ownWindow.transform) Destroy(t.gameObject);
        for(int i = 0; i < OPTIONS_COUNT; i++){
            var wheelOption = Instantiate(singleOptionWheelUIPrefab, ownWindow.transform);
            wheelOption.SetOptionSprite(singleWheelOptions[i].optionSprite);
        }
        base.Awake();
    }

    public override void Show() {
        base.Show();

        if(Player.LocalInstance != null){
            Player.LocalInstance.GetComponent<PlayerController>().SetCanMove(false);
            PlayerCam.Instance.SetCanLookAround(false);
        }

        foreach(Transform child in ownWindow.transform) child.localEulerAngles = Vector3.zero;

        for(int i = 0; i < ownWindow.transform.childCount; i++) {
            ownWindow.transform.GetChild(i).GetComponent<SingleOptionWheelUI>().Place(i, false);
        }
    }

    public override void Hide() {
        for(int i = 0; i < ownWindow.transform.childCount; i++) {
            SingleOptionWheelUI wheelOptionUI = ownWindow.transform.GetChild(i).GetComponent<SingleOptionWheelUI>();

            void WheelOptionUI_OnTransitionBackFinished(){
                base.Hide();
                wheelOptionUI.OnTransitionBackFinished -= WheelOptionUI_OnTransitionBackFinished;
                if(Player.LocalInstance != null){
                    Player.LocalInstance.GetComponent<PlayerController>().SetCanMove(true);
                    PlayerCam.Instance.SetCanLookAround(true);
                }
            }

            if(i == 0) wheelOptionUI.OnTransitionBackFinished += WheelOptionUI_OnTransitionBackFinished;
            wheelOptionUI.Place(0, true);
        }
    }

        

    private void Update() {
        if(isOpen){
            for(int i = 1; i <= OPTIONS_COUNT; i++){
                if(Input.GetKeyDown((i+1).ToString())){
                    Player.LocalInstance.GetComponent<PlayerVisuals>().Emote(i);
                    Hide();
                }
            }
        }
    }

    public Sprite GetEmoteSpriteByIndex(int index){
        return singleWheelOptions[index].optionSprite;
    }
}

[Serializable]
public class SingleWheelOption {
    public Sprite optionSprite;
}