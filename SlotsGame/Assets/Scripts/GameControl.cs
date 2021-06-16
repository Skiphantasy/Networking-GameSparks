using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using GameSparks.Core;

public class GameControl : MonoBehaviour
{
    public static event Action HandlePulled = delegate { };
    
    [SerializeField]
    private Text prizeText;
    [SerializeField]
    private RowMovement[] rows;
    [SerializeField]
    private Transform handle;
    [SerializeField]
    private Text playerNameText;

    private int numberOfAPICalls = 1;

    private long? prizeValue;
    private bool resultsChecked = true;

    private void Start()
    {
        resultsChecked = true;
        prizeValue = 0;
        prizeText.enabled = false;
        GetPlayerName();
    }

    void Update()
    {
        if (GS.Instance.Authenticated)
        {
            if (!rows[0].rowStopped || !rows[1].rowStopped || !rows[2].rowStopped)
            {
                prizeValue = 0;
                prizeText.enabled = false;
                resultsChecked = false;
                numberOfAPICalls = 1;
            }

            if (rows[0].rowStopped && rows[1].rowStopped && rows[2].rowStopped && !resultsChecked)
            {
                CheckResults();                        
            }
        }
    }

    private void OnMouseDown()
    {
        if (GS.Instance.Authenticated)
        {
            if (rows[0].rowStopped && rows[1].rowStopped && rows[2].rowStopped)
                StartCoroutine("PullHandle");
        }
    }

    private IEnumerator PullHandle()
    {
        for (int i = 0; i < 15; i += 5)
        {
            handle.Rotate(0, 0, i);
            yield return new WaitForSeconds(0.1f);
        }

        HandlePulled();

        for (int i = 0; i < 15; i += 5)
        {
            handle.Rotate(0, 0, -i);
            yield return new WaitForSeconds(0.1f);
        }
    }


    public void CheckResults()
    {
        if (numberOfAPICalls == 1)
        {
            LogEventRequest request = new LogEventRequest();
            request.SetEventKey("RESULT");
            request.SetEventAttribute("Row1", rows[0].stoppedSlot);
            request.SetEventAttribute("Row2", rows[1].stoppedSlot);
            request.SetEventAttribute("Row3", rows[2].stoppedSlot);
            request.Send(OnResultSuccess, OnResultError);
            numberOfAPICalls--;
        }
    }

    private void OnResultSuccess(LogEventResponse response)
    {
        resultsChecked = true;
        GSData scriptData = response.ScriptData;
        prizeValue = scriptData.GetNumber("prizeValue");
        prizeText.enabled = true;
        prizeText.text = $"Prize: {prizeValue}";
    }

    private void OnResultError(LogEventResponse response)
    {
        prizeValue = 0;
        print(response.Errors.JSON.ToString());
        resultsChecked = true;
    }

    private void GetPlayerName()
    {
        LogEventRequest request = new LogEventRequest();
        request.SetEventKey("GET_PLAYER_NAME");
        request.Send(OnGetNameSuccess, OnGetNameError);
    }

    private void OnGetNameSuccess(LogEventResponse response)
    {
        GSData scriptData = response.ScriptData;
        playerNameText.enabled = true;
        playerNameText.text = $"Hi {scriptData.GetString("displayName")}. Good Luck!";
    }

    private void OnGetNameError(LogEventResponse response)
    {
        print(response.Errors.JSON.ToString());
        playerNameText.text = string.Empty;
        playerNameText.enabled = false;
    }

    private void OnApplicationQuit()
    {
        GS.Reset();
    }
}
