using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbility : MonoBehaviour
{
    private TimeManager timemanager;
    private _controls PControls;

    [SerializeField] private bool TimeStopped;
    [SerializeField] private float CoolDownTime; // add 1 more
    public float StopTime;


    void Awake()
    {
        PControls = new _controls();
        timemanager = GameObject.FindGameObjectWithTag("TimeManager").GetComponent<TimeManager>();
    }

    void OnEnable()
    {
        PControls.Enable();
    }

    void OnDisable()
    {
        PControls.Disable();
    }

    void Start()
    {
        PControls.Player.TimeStop.performed += x => TimeStopOn();
    }

    void Update()
    {
        if (TimeStopped & CoolDownTime > 0)
        {
            CoolDown();
        }
    }

    void CoolDown()
    {
        if (TimeStopped == true)
        {
            if (CoolDownTime >= 1)
            {
                CoolDownTime = CoolDownTime - Time.fixedDeltaTime;
                return;
            }
            else if (CoolDownTime < 1)
            {
                TimeStopOff();
            }
        }
    }

    void TimeStopOn()
    {
        timemanager.StopTime();
        TimeStopped = true;
        CoolDownTime = StopTime;
    }

    void TimeStopOff()
    {
        timemanager.ContinueTime();
        TimeStopped = false;
    }
}
