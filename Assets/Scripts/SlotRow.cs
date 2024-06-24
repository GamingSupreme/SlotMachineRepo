using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotRow : MonoBehaviour
{
    // This int is used to store out random number
    private int randomVal;
    // This float is used to store the time interval between each slot tick
    private float timeInterval;

    // This bool is used to check if the row has stopped spinning
    public bool rowStopped;
    // This string is used to store the slot value that the row has stopped on
    public string stoppedSlot;

    void Start(){
        // On start, we set the rowStopped bool to true and subscribe to the SpinButtonPressed event
        rowStopped = true;
        GameControl.SpinButtonPressed += StartSpinning;
    }

    private void StartSpinning(){
        // When the spin button event is called in the Game Control class, we start the spinning coroutine
        stoppedSlot = "";
        StartCoroutine(Spin());
    }


    private IEnumerator Spin(){
        // Now that were spinning we set the rowStopped bool to false
        rowStopped = false;
        // and the time between each slot tick to 0.025f
        timeInterval = 0.025f;

        // We initially spin each slot 30 times
        for (int i = 0; i < 30; i++){
            // If the reel is in position where the last image is being shown
            if (transform.position.y >= 11.35f){
                // We reset the position to the first image (Which is a copy of the last image)
                transform.position = new Vector2(transform.position.x, -6.5f);
            }

            // We move the slot up by 0.85f, which is a third of the distance between each slot
            transform.position = new Vector2(transform.position.x, transform.position.y + 0.85f);

            yield return new WaitForSeconds(timeInterval);
        }

        // Now we generate a random int between 60 and 100
        // This will determine the amount of ticks the row will spin for
        randomVal = Random.Range(60, 100);

        // Since there is 3 ticks for every full image swap, we want to make sure the random number is divisible by 3
        switch (randomVal % 3){
            // if we see the remainder of the random after being devided by 3 is 1
            case 1:
                // We add 2 to the random value
                randomVal += 2;
                break;
            // if we see the remainder of the random after being devided by 3 is 2
            case 2:
                // We add 1 to the random value
                randomVal += 1;
                break;
        }

        // Now we spin each slot for that set random number
        for (int i = 0; i < randomVal; i++)
        {
            if (transform.position.y >= 11.35f){
                transform.position = new Vector2(transform.position.x, -6.5f);
            }

            transform.position = new Vector2(transform.position.x, transform.position.y + 0.85f);

            // the closer we get to the random value, the longer the time between ticks will be
            if (i > Mathf.RoundToInt(randomVal * 0.25f)){
                timeInterval = 0.05f;
            }
            if (i > Mathf.RoundToInt(randomVal * 0.5f)){
                timeInterval = 0.1f;
            }
            if (i > Mathf.RoundToInt(randomVal * 0.75f)){
                timeInterval = 0.15f;
            }
            if (i > Mathf.RoundToInt(randomVal * 0.95f)){
                timeInterval = 0.2f;
            }

            yield return new WaitForSeconds(timeInterval); ;
        }

        // We know exactly what the position for every slot is, so we can determine what slot the row has stopped on
        if (transform.position.y == -6.5f || transform.position.y == 11.35f){
            stoppedSlot = "YellowDuck";
        }
        else if (transform.position.y >= -4f && transform.position.y < -2f)
        {
            stoppedSlot = "GreyDuck";
        }
        else if (transform.position.y >= -1.5f && transform.position.y < -1.3f)
        {
            stoppedSlot = "GreenDuck";
        }
        else if (transform.position.y >= 1f && transform.position.y < 2)
        {
            stoppedSlot = "YoungYellowDuck";
        }
        else if (transform.position.y >= 3.6f && transform.position.y < 5){
            stoppedSlot = "YoungGreyDuck";
        }
        else if (transform.position.y >= 6.20f && transform.position.y < 7f){
            stoppedSlot = "YoungGreenDuck";
        }
        else if (transform.position.y >= 8.6f && transform.position.y < 9f){
            stoppedSlot = "Goose";
        }

        // Once we have determined the slot the row has stopped on, we set the rowStopped bool to true
        rowStopped = true;
    }

    private void OnDestroy()
    {
        // When the object is destroyed, we unsubscribe from the SpinButtonPressed event
        GameControl.SpinButtonPressed -= StartSpinning;
    }
}
