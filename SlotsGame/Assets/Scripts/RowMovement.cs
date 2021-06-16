using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using GameSparks.Core;
using System.Collections;
using UnityEngine;

public class RowMovement : MonoBehaviour
{
    private int randomValue;
    private float timeInterval;
    public bool rowStopped;
    public string stoppedSlot;
    
    void Start()
    {
        rowStopped = true;
        stoppedSlot = string.Empty;
        GameControl.HandlePulled += StartRotating;
    }

    private void StartRotating()
    {
        stoppedSlot = string.Empty;
        StartCoroutine("Rotate");
    }

    private IEnumerator Rotate()
    {
        rowStopped = false;
        timeInterval = 0.025f;

        for (int i = 0; i < 30; i++)
        {
            if (transform.position.y <= -2f)
                transform.position = new Vector2(transform.position.x, 1f);

            transform.position = new Vector2(transform.position.x, transform.position.y - 0.25f);

            yield return new WaitForSeconds(timeInterval);
        }

        randomValue = Random.Range(60, 100);

        switch (randomValue % 3)
        {
            case 1:
                randomValue += 2;
                break;
            case 2:
                randomValue += 1;
                break;
        }

        for (int i = 0; i < randomValue; i++)
        {
            if (transform.position.y <= -2f)
                transform.position = new Vector2(transform.position.x, 1f);

            transform.position = new Vector2(transform.position.x, transform.position.y - 0.25f);

            if (i > Mathf.RoundToInt(randomValue * 0.25f))
                timeInterval = 0.05f;
            if (i > Mathf.RoundToInt(randomValue * 0.5f))
                timeInterval = 0.1f;
            if (i > Mathf.RoundToInt(randomValue * 0.75f))
                timeInterval = 0.15f;
            if (i > Mathf.RoundToInt(randomValue * 0.95f))
                timeInterval = 0.2f;

            yield return new WaitForSeconds(timeInterval);
        }

        GetStoppedSlot();
        rowStopped = true;
    }

    private void GetStoppedSlot()
    {
        // Reaching Limit for Number of API Request per second in Preview 
        // https://docs.gamesparks.com/documentation/key-concepts/system-limits.html
        //LogEventRequest request = new LogEventRequest();
        //request.SetEventKey("SLOT");
        //request.SetEventAttribute("Y", (long)transform.position.y);
        //request.Send(OnGetSlotSuccess, OnGetSlotError);

        if (transform.position.y == -2f)
            stoppedSlot = "Diamond";
        else if (transform.position.y == -1.25f)
            stoppedSlot = "Crown";
        else if (transform.position.y == -0.5f)
            stoppedSlot = "Melon";
        else if (transform.position.y == 0.25f)
            stoppedSlot = "Bar";
        else if (transform.position.y == 1f)
            stoppedSlot = "Seven";
        else if (transform.position.y == 1.75f)
            stoppedSlot = "Cherry";
        else if (transform.position.y == 2.5f)
            stoppedSlot = "Lemon";
        else if (transform.position.y == 3.25f)
            stoppedSlot = "Diamond";
    }

    private void OnGetSlotSuccess(LogEventResponse response)
    {
        rowStopped = true;
        GSData scriptData = response.ScriptData;
        stoppedSlot = scriptData.GetString("stoppedSlot");
        print(stoppedSlot);
    }

    private void OnGetSlotError(LogEventResponse response)
    {
        rowStopped = true;
        print(response.Errors.JSON.ToString());
    }

    private void OnDestroy()
    {
        GameControl.HandlePulled -= StartRotating;
    }
}
