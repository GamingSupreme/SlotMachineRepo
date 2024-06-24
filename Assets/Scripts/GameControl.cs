using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
    // Event to be called when the spin button is pressed
    public static event Action SpinButtonPressed = delegate { };
    // This event will be subscribed to by the SlotRow class

    // Ints to store the players currency and bet
    public int currencyValue = 100;
    public int bet = 0;

    // Float to store the payout multiplier value
    private float multiplierVal;

    // bools to make sure the player has input a number
    public bool stringValued = false;
    // Abd to make sure were on the first loop so the text on the screen doesnt show up till AFTER the first spin
    private bool firstloop = true;

    //Reference to the slot rows
    [SerializeField]
    private SlotRow[] slotRows;

    //Reference to the spin button
    [SerializeField]
    private Button tMPButton;

    // Reference to the on screen UI elements
    [SerializeField]
    private TextMeshProUGUI multiplierText;
    [SerializeField]
    private TextMeshProUGUI currencyValText;
    [SerializeField]
    private TMP_InputField bettingField;
    [SerializeField]
    private TextMeshProUGUI betOutcome;

    // Booleans to check if the slot rows have stopped and if the results have been checked
    [SerializeField]
    private bool resultsChecked = false;

    void Update(){
        // Update the currency value text
        currencyValText.text = "You have: " + currencyValue + "$";
        
        // Check if the slot rows have stopped
        if (!slotRows[0].rowStopped || !slotRows[1].rowStopped || !slotRows[2].rowStopped){
            // If rows havent stopped, disable UI and reset multiplier
            multiplierVal = 0;
            betOutcome.enabled = false;
            multiplierText.enabled = false;
            resultsChecked = false;
        }

        // If rows have stopped, and we havent checked the results
        if (slotRows[0].rowStopped && slotRows[1].rowStopped && slotRows[2].rowStopped && !resultsChecked)
        {
            // Check the results
            CheckResults();
            // Cash out the bet
            CashOut();
            // if were not in the first loop, we can enable the UI
            if (!firstloop)
            {
                betOutcome.enabled = true;
                multiplierText.enabled = true;
                tMPButton.interactable = true;
            }
            // update the multiplier text
            multiplierText.text = "Your multiplier is: " + multiplierVal;
        }
    }

    // This function is called when the spin button is pressed
    public void ClickedButton(){
        // If the player has input a correct number proceed
        if (CheckBal()){
            // If the slot rows have stopped and our bet is less equal than our currency
            if (slotRows[0].rowStopped && slotRows[1].rowStopped && slotRows[2].rowStopped && (currencyValue >= bet)){
                // We increase the font size, call the spin button function, and remove the ability to press the button again
                multiplierText.fontSize = 25;
                SpinButtonPressed();
                firstloop = false;
                tMPButton.interactable = false;
            }
        }
        // If the player has input a string
        else if (stringValued){
            // We enable the UI and display an error message
            multiplierText.fontSize = 20;
            multiplierText.enabled = true;
            multiplierText.text = "Please enter a valid number!";
            stringValued = false;
        }
        // If the player has bet more than they have
        else{
            // We enable the UI and display an error message
            multiplierText.fontSize = 20;
            multiplierText.enabled = true;
            multiplierText.text = "You don't have enough money! setting max bet.";
        }
    }

    public bool CheckBal()
    {
        // If the value passed in isnt an Int
        if (!int.TryParse(bettingField.text, out int fill))
        {
            // Set the stringValued bool to true and return false
            stringValued = true;
            return false;
        }
        // If the bet isnt a string, we set the bet to whatever is in the betting field
        bet = int.Parse(bettingField.text);

        // Then we check if our bet is higher than our current currency
        if (bet > currencyValue){
            // If it is, we set the bet to our current currency and return false
            bettingField.text = currencyValue.ToString();
            return false;
        }
        // If the bet is less than our currency, we return true
        return true;
    }

    // This function is called after checking slot results
    public void CashOut()
    {
        // Subtract the bet from currency, multiply the bet by the mutiplier and add it to the currency
        currencyValue -= bet;
        bet = (int)(bet * multiplierVal);
        currencyValue += bet;
    }

    private void CheckResults()
    {
        // Define two dictionaries to map slot combinations to multiplier values

        // First we do exact matches
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

        // Then we do partial matches (two out of three)
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
        // For each key value pair in the exactMatches dictionary
        foreach (var kvp in exactMatches)
        {
            // We use a string array to store the three slot values
            string[] slotCombination = kvp.Key;
            // And a float to store the multiplier value
            float multiplierValue = kvp.Value;

            // Now we compare the kvp data to the current slot rows
            if (slotRows[0].stoppedSlot == slotCombination[0] &&
                slotRows[1].stoppedSlot == slotCombination[1] &&
                slotRows[2].stoppedSlot == slotCombination[2])
            {
                // If we have a match, we set the multiplier value and display the outcome
                multiplierVal = multiplierValue;
                betOutcome.text = "You won! " + (bet * multiplierVal) + "$";
                resultsChecked = true;
                return;
            }
        }

        // Check partial matches (two out of three)
        foreach (var kvp in partialMatches)
        {
            // We use a string to store the slot value
            string slotValue = kvp.Key;
            // And a float to store the multiplier value
            float multiplierValue = kvp.Value;

            // Now we compare the kvp data to the current slot rows
            if ((slotRows[0].stoppedSlot == slotValue && slotRows[1].stoppedSlot == slotValue) ||
                (slotRows[0].stoppedSlot == slotValue && slotRows[2].stoppedSlot == slotValue) ||
                (slotRows[1].stoppedSlot == slotValue && slotRows[2].stoppedSlot == slotValue))
            {
                // If we have a partial match, we set the multiplier value and display the outcome
                multiplierVal = multiplierValue;
                betOutcome.text = "You won! " + (bet * multiplierVal) + "$";
                resultsChecked = true;
                return;
            }
        }

        // In the case that there are no matches, we set the multiplier value to 0 and display the outcome
        multiplierVal = 0f; // Or any default value you want
        betOutcome.text = "You lost! " + bet + "$";

        // Once all checking is done, we set the resultsChecked bool to true
        resultsChecked = true;
    }

    // This function is soley used to close the application on a button press
    public void exitGame()
    {
        Application.Quit();
    }
}
