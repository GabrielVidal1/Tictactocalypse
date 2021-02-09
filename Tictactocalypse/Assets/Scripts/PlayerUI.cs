using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    public enum CooldownType
    {
        MainAction,
        Event
    }

    [SerializeField] private ButtonEvent mainButton;
    public float mainButtonDelayMultiplier = 1f;

    [SerializeField] private ButtonEvent[] eventButtons; 

    [SerializeField] private TextMeshProUGUI scoreText;

    private int playerIndex;

    private InputManager im;

    public void Initialize(int playerIndex)
    {
        im = GameManager.gm.GetComponent<InputManager>();
        this.playerIndex = playerIndex;

        mainButton.Initialize(im.playersKeyBinding[playerIndex].upKeyString);
        mainButton.SetTexture(GameManager.gm.playersData[playerIndex].playerIcon);

        eventButtons[0].Initialize(im.playersKeyBinding[playerIndex].leftKeyString);
        eventButtons[1].Initialize(im.playersKeyBinding[playerIndex].rightKeyString);

    }

    public void UpdatePlayerEventsUI()
    {
        UpdatePlayerEventsUI(0);
        UpdatePlayerEventsUI(1);
    }

    public void UpdatePlayerEventsUI(int buttonIndex)
    {
        if (Player.players[playerIndex].events[buttonIndex] != null)
        {
            eventButtons[buttonIndex].SetTexture(Player.players[playerIndex].events[buttonIndex].icon);
        }
        else
        {
            eventButtons[buttonIndex].SetTexture(GameManager.gm.emptyTexture);
        }
    }

    public void TriggerPlaceMode(bool enbale, int index)
    {
        if (enbale)
        {
            int nindex = (playerIndex % 2 + index + 1) % 2; 
            eventButtons[0].SetTexture(GameManager.gm.arrowsTextures[2 * nindex + 1]);
            eventButtons[1].SetTexture(GameManager.gm.arrowsTextures[2 * nindex]);
        }
        else
        {
            UpdatePlayerEventsUI(0);
            UpdatePlayerEventsUI(1);
        }
    }

    public void PressKey(int index)
    {
        if (index == 0)
            mainButton.PressButton();
        else
            eventButtons[index].PressButton();
    }

    public void PressMainButton()
    {
        StartCoroutine(ButtonCooldown(
            CooldownType.MainAction, 0,
            mainButton,
            ConstantManager.mainButtonCooldown * mainButtonDelayMultiplier));
    }

    public void StartCooldownOnEvent(int eventIndex)
    {
        StartCoroutine(ButtonCooldown(
            CooldownType.Event, eventIndex,
            eventButtons[eventIndex],
            Player.players[playerIndex].events[eventIndex].cooldown * mainButtonDelayMultiplier));
    }

    IEnumerator ButtonCooldown(CooldownType type, int index, ButtonEvent buttonEvent, float duration)
    {
        if (type == CooldownType.MainAction)
            Player.players[playerIndex].canPlayMain = false;
        else if (type == CooldownType.Event)
            Player.players[playerIndex].canPlayEvent[index] = false;

        yield return StartCoroutine(buttonEvent.StartCooldown(duration));

        if (type == CooldownType.MainAction)
            Player.players[playerIndex].canPlayMain = true;
    }

    public void TriggerDisabledButton(int index)
    {
        if (index == -1)
            mainButton.TriggerDisabled();
        else
            eventButtons[index].TriggerDisabled();
    }

    public void TriggerPressButton(int index)
    {
        if (index == -1)
            mainButton.TriggerPress();
        else
            eventButtons[index].TriggerPress();
    }

    public void SetScore(int score)
    {
        scoreText.text = score.ToString();
    }
}

