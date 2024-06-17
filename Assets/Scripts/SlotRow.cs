using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotRow : MonoBehaviour
{
    [SerializeField]
    private int randomVal;
    private float timeInterval;

    public bool rowStopped;
    public string stoppedSlot;

    void Start(){
        rowStopped = true;
        GameControl.SpinButtonPressed += StartSpinning;
    }

    private void StartSpinning(){
        stoppedSlot = "";
        StartCoroutine(Spin());
    }

    private IEnumerator Spin(){
        rowStopped = true;
        timeInterval = 0.025f;
        for (int i = 0; i < 30; i++){
            if (transform.position.y >= 11.35f){
                transform.position = new Vector2(transform.position.x, -6.5f);
            }

            transform.position = new Vector2(transform.position.x, transform.position.y + 0.85f);

            yield return new WaitForSeconds(timeInterval);
        }

        randomVal = Random.Range(60, 100);

        switch (randomVal % 3){
            case 1:
                randomVal += 2;
                break;
            case 2:
                randomVal += 1;
                break;
        }

        for (int i = 0; i < randomVal; i++)
        {
            if (transform.position.y >= 11.35f){
                transform.position = new Vector2(transform.position.x, -6.5f);
            }

            transform.position = new Vector2(transform.position.x, transform.position.y + 0.85f);

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

        if (transform.position.y == -6.5f || transform.position.y == -11.35f){
            stoppedSlot = "YellowDuck";
        }
        else if (transform.position.y == -3.95f){
            stoppedSlot = "GreyDuck";
        }
        else if (transform.position.y == -1.4f){
            stoppedSlot = "GreenDuck";
        }
        else if (transform.position.y == 1.15f){
            stoppedSlot = "YoungYellowDuck";
        }
        else if (transform.position.y == 3.7f){
            stoppedSlot = "YoungGreyDuck";
        }
        else if (transform.position.y == 6.25f){
            stoppedSlot = "YoungGreenDuck";
        }
        else if (transform.position.y == 8.79f){
            stoppedSlot = "Goose";
        }

        rowStopped = true;
    }

    private void OnDestroy()
    {
        GameControl.SpinButtonPressed -= StartSpinning;
    }
}
