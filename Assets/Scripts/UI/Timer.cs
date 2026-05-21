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
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject lostScreen;

    public bool hasWon;
    private bool hasEnded;

    private void Start()
    {
        if (winScreen != null)
        {
            winScreen.SetActive(false);
        }

        if (lostScreen != null)
        {
            lostScreen.SetActive(false);
        }
    }

    void Update()
    {
        if (hasEnded)
        {
            return;
        }

        remainingTime -= Time.deltaTime;
        remainingTime = Mathf.Max(0f, remainingTime);
        //timerText.text = elapsedTime.ToString();
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{00:00}:{1:00}", minutes, seconds);


        if(remainingTime > 0)
        {
            return;
        }

        if(roomHandler != null && roomHandler.IsPlayerInsideRoom())
        {
            ShowWinScreen();
        }
        else
        {
            ShowLostScreen();
        }
    }

    public void ShowWinScreen()
    {
        if (hasEnded)
        {
            return;
        }

        hasWon = true;
        hasEnded = true;
        DisablePlayerMovement();
        if (winScreen != null)
        {
            winScreen.SetActive(true);
        }
        if (lostScreen != null)
        {
            lostScreen.SetActive(false);
        }
    }

    public void ShowLostScreen()
    {
        if (hasEnded)
        {
            return;
        }

        hasWon = false;
        hasEnded = true;
        DisablePlayerMovement();
        if (lostScreen != null)
        {
            lostScreen.SetActive(true);
        }
        if (winScreen != null)
        {
            winScreen.SetActive(false);
        }
    }

    private void DisablePlayerMovement()
    {
        PlayerController playerController = FindFirstObjectByType<PlayerController>();
        if (playerController != null)
        {
            playerController.canMove = false;
        }
    }

    public float GetRemainingTime()
    {
        return remainingTime;
    }

    public void SetRemainingTime(float time)
    {
        remainingTime = time;
    }
}
