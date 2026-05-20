using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.Rendering;

public class Timer : MonoBehaviour
{
    [SerializeField] private PlayerRoomHandler roomHandler;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float remainingTime;

    public bool hasWon;

    void Update()
    {
        remainingTime -= Time.deltaTime;
        //timerText.text = elapsedTime.ToString();
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{00:00}:{1:00}", minutes, seconds);


        if(remainingTime <= 0 && roomHandler.IsPlayerInsideRoom())
        {
            hasWon = true;
        }
        else if(remainingTime <= 0 && !roomHandler.IsPlayerInsideRoom())
        {
            hasWon = false;
        }
    }
}
