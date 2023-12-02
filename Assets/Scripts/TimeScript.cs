using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeScript : MonoBehaviour//////////////////Ovidio////////////////
{
    private RubyController rubyTime;
    public bool TimerActive ;// timer bool switch
    [SerializeField] TextMeshProUGUI timerText;/////////text UI///////
    [SerializeField] float remainingTime;/////// adding the timer value in inspector/////////
    
    
    void Start()
    {
        TimerActive = false;/// toggle is set to off//////
        timerText.text = "00:00";//// format display
    
        GameObject RubyTimerObject = GameObject.FindWithTag("RubyPlayer");/// finding the rubyscript////
        if(RubyTimerObject!=null)
        {
          rubyTime = RubyTimerObject.GetComponent<RubyController>();/// making a new variable for the ruby script to use in this script/////
        }
    }

    void Update()
    {
        if(TimerActive)///////while its active and on
       { 
        if( remainingTime>0)///////////timer countdown/////////
        {
            remainingTime -= Time.deltaTime;
        }
        else if(remainingTime<0)//////time runs out lower ruby's health to zero and end a 0///////
        {   
            rubyTime.ChangeHealth(-20);/// calling the ruby script///////
            Debug.Log("ruby took damage");
            remainingTime=0;
        }
        int minutes = Mathf.FloorToInt(remainingTime / 60);//////timer format minute////////
        int seconds = Mathf.FloorToInt(remainingTime % 60);//////timer format seconds/////
        timerText.text = string.Format("{0:00}:{1:00}",minutes, seconds);///////displayer ui ///////
       }
    }
}
