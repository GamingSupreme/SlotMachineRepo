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
        rowStopped = false;
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

        // Define a dictionary to map y position ranges to stoppedSlot values
        Dictionary<(float, float), string> slotMapping = new Dictionary<(float, float), string>(){
            { (-Mathf.Infinity, -6.5f), "YellowDuck" },
            { (-6.5f, -4f), "GreyDuck" },
            { (-4f, -1.5f), "GreenDuck" },
            { (-1.5f, 1f), "YoungYellowDuck" },
            { (1f, 3.6f), "YoungGreyDuck" },
            { (3.6f, 6.2f), "YoungGreenDuck" },
            { (6.2f, 8.6f), "Goose" },
            { (8.6f, 9f), "Goose" }
        };

        foreach (var kvp in slotMapping)
        {
            float minY = kvp.Key.Item1;
            float maxY = kvp.Key.Item2;
            string slotType = kvp.Value;

            if (transform.position.y >= minY && transform.position.y < maxY)
            {
                stoppedSlot = slotType;
                break;
            }
        }

        rowStopped = true;
    }

    private void OnDestroy()
    {
        GameControl.SpinButtonPressed -= StartSpinning;
    }
}
