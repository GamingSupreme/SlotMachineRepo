using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * This class was used when i was testing all the values for the slots and didnt wanna randomly roll through the slots every time
 * Please disregard this class
 * */
public class TestButton : MonoBehaviour
{
    public Transform Row;

    public void MoveSlot()
    {

            Row.transform.position = new Vector2(Row.transform.position.x, Row.transform.position.y + 0.85f);
    }
    void Update()
    {
        if (Row.transform.position.y >= 11.5f)
        {
            Row.transform.position = new Vector2(Row.transform.position.x, -6.5f);
        }
    }
}
