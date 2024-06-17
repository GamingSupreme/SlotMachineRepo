using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
    public static event Action SpinButtonPressed = delegate { };

    public int currencyValue = 100;
    public int bet = 0;
    public bool stringValued = false;
    private bool firstloop = true;

    [SerializeField]
    private SlotRow[] slotRows;

    [SerializeField]
    private Button tMPButton;
    [SerializeField]
    private TextMeshProUGUI multiplierText;
    [SerializeField]
    private TextMeshProUGUI currencyValText;
    [SerializeField]
    private TMP_InputField bettingField;
    [SerializeField]
    private TextMeshProUGUI betOutcome;

    private float multiplierVal;

    [SerializeField]
    private bool resultsChecked = false;

    void Update(){
        currencyValText.text = "You have: " + currencyValue + "$";
        

        if (!slotRows[0].rowStopped || !slotRows[1].rowStopped || !slotRows[2].rowStopped){
            multiplierVal = 0;
            betOutcome.enabled = false;
            multiplierText.enabled = false;
            resultsChecked = false;
        }

        if (slotRows[0].rowStopped && slotRows[1].rowStopped && slotRows[2].rowStopped && !resultsChecked)
        {
            CheckResults();
            CashOut();
            if (!firstloop)
            {
                betOutcome.enabled = true;
                multiplierText.enabled = true;
                tMPButton.interactable = true;
            }
            multiplierText.text = "Your multiplier is: " + multiplierVal;
        }
    }

    public void ClickedButton(){
        if (CheckBal()){
            if (slotRows[0].rowStopped && slotRows[1].rowStopped && slotRows[2].rowStopped && (currencyValue >= bet)){
                multiplierText.fontSize = 25;
                SpinButtonPressed();
                firstloop = false;
                tMPButton.interactable = false;
            }
        }
        else if (stringValued){
            multiplierText.fontSize = 20;
            multiplierText.enabled = true;
            multiplierText.text = "Please enter a valid number!";
            stringValued = false;
        }
        else{
            multiplierText.fontSize = 20;
            multiplierText.enabled = true;
            multiplierText.text = "You don't have enough money! setting max bet.";
        }
    }

    public bool CheckBal()
    {
        if (!int.TryParse(bettingField.text, out int fill))
        {
            stringValued = true;
            return false;
        }
        bet = int.Parse(bettingField.text);
        if (bet > currencyValue){
            bettingField.text = currencyValue.ToString();
            return false;
        }
        return true;
    }

    public void CashOut()
    {
        currencyValue -= bet;
        bet = (int)(bet * multiplierVal);
        currencyValue += bet;
    }

    private void CheckResults()
    {
        // Define a dictionary to map slot combinations to multiplier values
        Dictionary<string[], float> exactMatches = new Dictionary<string[], float>()
        {
            { new string[] { "YellowDuck", "YellowDuck", "YellowDuck" }, 2.5f },
            { new string[] { "GreyDuck", "GreyDuck", "GreyDuck" }, 3.5f },
            { new string[] { "GreenDuck", "GreenDuck", "GreenDuck" }, 4.5f },
            { new string[] { "YoungYellowDuck", "YoungYellowDuck", "YoungYellowDuck" }, 2f },
            { new string[] { "YoungGreyDuck", "YoungGreyDuck", "YoungGreyDuck" }, 3f },
            { new string[] { "YoungGreenDuck", "YoungGreenDuck", "YoungGreenDuck" }, 4f },
            { new string[] { "Goose", "Goose", "Goose" }, 10f }
        };

        Dictionary<string, float> partialMatches = new Dictionary<string, float>()
        {
            { "YellowDuck", 1.5f },
            { "GreyDuck", 2.5f },
            { "GreenDuck", 3.5f },
            { "YoungYellowDuck", 1.5f },
            { "YoungGreyDuck", 2.5f },
            { "YoungGreenDuck", 3.5f },
            { "Goose", 5f }
        };

        // Check exact matches first
        foreach (var kvp in exactMatches)
        {
            string[] slotCombination = kvp.Key;
            float multiplierValue = kvp.Value;

            if (slotRows[0].stoppedSlot == slotCombination[0] &&
                slotRows[1].stoppedSlot == slotCombination[1] &&
                slotRows[2].stoppedSlot == slotCombination[2])
            {
                multiplierVal = multiplierValue;
                betOutcome.text = "You won! " + (bet * multiplierVal) + "$";
                resultsChecked = true;
                return;
            }
        }

        // Check partial matches (two out of three)
        foreach (var kvp in partialMatches)
        {
            string slotValue = kvp.Key;
            float multiplierValue = kvp.Value;

            if ((slotRows[0].stoppedSlot == slotValue && slotRows[1].stoppedSlot == slotValue) ||
                (slotRows[0].stoppedSlot == slotValue && slotRows[2].stoppedSlot == slotValue) ||
                (slotRows[1].stoppedSlot == slotValue && slotRows[2].stoppedSlot == slotValue))
            {
                multiplierVal = multiplierValue;
                betOutcome.text = "You won! " + (bet * multiplierVal) + "$";
                resultsChecked = true;
                return;
            }
        }
        // Default case if no matches found
        multiplierVal = 0f; // Or any default value you want
        betOutcome.text = "You lost! " + bet + "$";
        resultsChecked = true;
    }

    public void exitGame()
    {
        Application.Quit();
    }
}
