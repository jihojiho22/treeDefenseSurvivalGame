using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI invasionText;
    [SerializeField] float remainingTime;
   
    void Update()
    {
        if (GameManager.instance.playerCharacter)
        {
            if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
        else if (remainingTime < 0)
        {
            remainingTime = 0;
        }
        int minutes = Mathf.FloorToInt(remainingTime / 60); 
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
         if (remainingTime <= 730 && remainingTime > 720)
        {
            int countdownSeconds = Mathf.CeilToInt(remainingTime - 720);
            invasionText.text = $"Invasion starts in {countdownSeconds}s!";
            invasionText.gameObject.SetActive(true);
        }
       
       if(remainingTime <= 720 && 719 <= remainingTime ) {
        invasionText.gameObject.SetActive(false);
        Debug.Log("remaingtime 720");
        SceneManager.LoadScene("Invasion1Scene");
       }
    }
}
