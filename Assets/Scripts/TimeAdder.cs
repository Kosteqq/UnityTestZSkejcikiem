using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TimeAdder : MonoBehaviour
{
    [Header("References")]
    public ParticleSystem m_Particles;

    [Header("Config")]
    public int m_MaxNumberOfUses;
    private int m_NumberOfUses;

    private bool m_IsActive;

    private void Start()
    {
        TurnOff();
        GameController.AddFuncOnStart(OnGameStart);
        GameController.AddFuncOnEnds(OnGameEnd);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!m_IsActive) return;

        if (other.tag == "Player")
        {
            m_NumberOfUses++;
            if (m_NumberOfUses >= m_MaxNumberOfUses)
            {
                TurnOff();
                Debug.Log("Time adder expired");
            }
        }
    }

    private void OnGameStart()
    {
        Restart();
    }

    private void OnGameEnd()
    {
        TurnOff();
    }

    private void Restart()
    {
        m_IsActive = true;
        m_Particles.Play();
        m_NumberOfUses = 0;
        GetComponent<BoxCollider>().enabled = true;
    }

    private void TurnOff()
    {
        m_IsActive = false;
        m_Particles.Stop();
        GetComponent<BoxCollider>().enabled = false;
    }

}
