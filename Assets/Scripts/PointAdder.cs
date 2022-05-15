using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
public class PointAdder : MonoBehaviour
{
    [Header("References")]
    public ParticleSystem m_Particles;

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
            GameController.AddPoint();
            Debug.Log("Added point!");
            TurnOff();
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
        GetComponent<BoxCollider>().enabled = true;
    }

    private void TurnOff()
    {
        m_IsActive = false;
        m_Particles.Stop();
        GetComponent<BoxCollider>().enabled = false;
    }

}
