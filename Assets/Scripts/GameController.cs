using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Config")]
    public float m_MaxTime;
    public float m_AddingTimeAmount;


    private bool  m_IsRunning;
    private float m_TimeLeft;
    private int   m_Points;

    private static GameController s_Instance;

    public delegate void CallOnStart();
    private List<CallOnStart> m_CallOnStartFunctions = new List<CallOnStart>();
    private List<CallOnStart> m_CallOnEndFunctions = new List<CallOnStart>();


    private void Awake()
    {
        s_Instance = this;
    }

    private void Start()
    {
        StartGame();
    }

    private void Update()
    {
        if (!m_IsRunning) return;

        if (m_TimeLeft < 0) EndGame();
        m_TimeLeft -= Time.deltaTime;
    }



    //////////////////////////
    ///   STATIC FUNCTIONS
    //////////////////////////

    public static void AddFuncOnStart(CallOnStart func)
    { s_Instance.m_CallOnStartFunctions.Add(func); }

    public static void AddFuncOnEnds(CallOnStart func)
    { s_Instance.m_CallOnEndFunctions.Add(func); }

    public static void AddTime()
    { s_Instance.m_TimeLeft += s_Instance.m_AddingTimeAmount; Debug.Log("Added " + s_Instance.m_AddingTimeAmount + " sec!"); }

    public static float GetTimeLeft()
    { return s_Instance.m_TimeLeft; }

    public static void AddPoint()
    { s_Instance.m_Points++; }

    public static int GetPoints()
    { return s_Instance.m_Points; }
    
    
    //////////////////////////
    ///      BACKEND
    //////////////////////////

    private void StartGame()
    {
        m_TimeLeft = m_MaxTime;
        m_Points = 0;

        foreach (var func in m_CallOnStartFunctions)
            func();

        m_IsRunning = true;
        Debug.Log("Game started!");
    }
    
    private void EndGame()
    {
        foreach (var func in m_CallOnEndFunctions)
            func();

        m_IsRunning = false;
        Debug.Log("Game ends!");
    }
}
