using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    public static event Action SpinButtonPressed = delegate { };

    public int moneyValue = 0;
    public int bet = 0;

    [SerializeField]
    private SlotRow[] slotRows;

    [SerializeField]
    private Transform spinButton;

    [SerializeField]
    private TextMeshProUGUI multiplierText;

    private float multiplierVal;

    [SerializeField]
    private bool resultsChecked = false;

    void Update(){
        if (!slotRows[0].rowStopped || !slotRows[1].rowStopped || !slotRows[2].rowStopped){
            multiplierVal = 0;
            multiplierText.enabled = false;
            resultsChecked = false;
        }

        if (slotRows[0].rowStopped && slotRows[1].rowStopped && slotRows[2].rowStopped && !resultsChecked)
        {
            CheckResults();
            multiplierText.enabled = true;
            multiplierText.text = "Your multiplier is: " + multiplierVal;
        }
    }

    public void ClickedButton(){
        if (slotRows[0].rowStopped && slotRows[1].rowStopped && slotRows[2].rowStopped){
            SpinButtonPressed();
        }
    }

    private void CheckResults(){
        if (slotRows[0].stoppedSlot == "YellowDuck" &&
            slotRows[1].stoppedSlot == "YellowDuck" && 
            slotRows[2].stoppedSlot == "YellowDuck"){
            multiplierVal = 2;
        }

        else if (slotRows[0].stoppedSlot == "GreyDuck" &&
            slotRows[1].stoppedSlot == "GreyDuck" &&
            slotRows[2].stoppedSlot == "GreyDuck"){
            multiplierVal = 3;
        }

        else if (slotRows[0].stoppedSlot == "GreenDuck" &&
            slotRows[1].stoppedSlot == "GreenDuck" &&
            slotRows[2].stoppedSlot == "GreenDuck"){
            multiplierVal = 4;
        }

        else if (slotRows[0].stoppedSlot == "YoungYellowDuck" &&
        slotRows[1].stoppedSlot == "YoungYellowDuck" &&
        slotRows[2].stoppedSlot == "YoungYellowDuck"){
            multiplierVal = 1.5f;
        }

        else if (slotRows[0].stoppedSlot == "YoungGreyDuck" &&
        slotRows[1].stoppedSlot == "YoungGreyDuck" &&
        slotRows[2].stoppedSlot == "YoungGreyDuck"){
            multiplierVal = 2.5f;
        }

        else if (slotRows[0].stoppedSlot == "YoungGreenDuck" &&
        slotRows[1].stoppedSlot == "YoungGreenDuck" &&
        slotRows[2].stoppedSlot == "YoungGreenDuck"){
            multiplierVal = 3.5f;
        }

        else if (slotRows[0].stoppedSlot == "Goose" &&
        slotRows[1].stoppedSlot == "Goose" &&
        slotRows[2].stoppedSlot == "Goose"){
            multiplierVal = 10f;
        }

        resultsChecked = true;
    }


}
