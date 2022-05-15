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
        

    private void Start()
    {
        s_Instance = this;
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

    public static void AddTime()
    { s_Instance.m_TimeLeft += s_Instance.m_AddingTimeAmount; }

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

        m_IsRunning = true;
        Debug.Log("Game started!");
    }
    
    private void EndGame()
    {
        m_IsRunning = false;
        Debug.Log("Game ends!");
    }
}
