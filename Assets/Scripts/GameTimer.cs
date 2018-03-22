using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameTimer : MonoBehaviour
{
    [SerializeField] float totalGametime;
    [SerializeField] Text timerText;

    // Use this for initialization
    void Start()
    {
        // maximum value omdat deze timer geen uren weergeeft is 59:59 (m:s)
        if(totalGametime >= 60 * 60)
        {
            totalGametime = 59 * 60;
        }
    }

    // Update is called once per frame
    void Update()
    {
        totalGametime -= Time.deltaTime;
        int minutes = Mathf.FloorToInt(totalGametime / 60);
        int seconds = Mathf.FloorToInt(totalGametime - (minutes * 60));
        timerText.text = minutes.ToString() + " : " + seconds.ToString();
        if(totalGametime < 0)
        {
            GoGameOver();
        }
    }

    private void GoGameOver()
    {
        // zet eind game scherm hier neer
    }
}
