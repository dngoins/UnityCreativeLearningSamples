using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class clock : MonoBehaviour {

    private DateTime currentTime;
    const float
        degreesPerHour = 30f,
        degreesPerMinute = 6f,
        degreesPerSecond = 6f;


    public bool Analog;
    public Transform HoursTransform;
    public Transform MinutesTransform;
    public Transform SecondsTransform;

    
    // Use this for initialization
    void Start () {
		
	}
	
    void UpdateAnalog()
    {
        TimeSpan time = DateTime.Now.TimeOfDay;
        HoursTransform.localRotation =
            Quaternion.Euler(0f, (float)time.TotalHours * degreesPerHour, 0f);
        MinutesTransform.localRotation =
            Quaternion.Euler(0f, (float)time.TotalMinutes * degreesPerMinute, 0f);
        SecondsTransform.localRotation =
            Quaternion.Euler(0f, (float)time.TotalSeconds * degreesPerSecond, 0f);

    }

    void UpdateDiscreet()
    {
        DateTime time = DateTime.Now;
        HoursTransform.localRotation =
            Quaternion.Euler(0f, time.Hour * degreesPerHour, 0f);
        MinutesTransform.localRotation =
            Quaternion.Euler(0f, time.Minute * degreesPerMinute, 0f);
        SecondsTransform.localRotation =
            Quaternion.Euler(0f, time.Second * degreesPerSecond, 0f);

    }

    // Update is called once per frame
    void Update () {

        currentTime = DateTime.Now;
		if(Analog)
        {
            UpdateAnalog();
        }
        else
        {
            UpdateDiscreet();
        }
	}
}
