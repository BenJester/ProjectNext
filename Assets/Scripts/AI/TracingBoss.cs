using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracingBoss : MonoBehaviour
{
    public float DistanceDashPlayer;
    public float DistanceDelta;
    public float PlayerBreakAwayDistance;

    public Color BeginColorStartMin;
    public Color BeginColorStartMax;

    public Color ChangeStateColorStart;
    public Color ChangeStateColorEnd;

    public bool OnlyTracing;

    private ParticleSystem m_particleSys;
    private MovingObjectByWayPoint m_movingObj;
    private bool m_bDash;

    private ParticleSystem.MainModule m_mainParticle;

    // Start is called before the first frame update
    void Start()
    {
        m_movingObj = GetComponent<MovingObjectByWayPoint>();
        m_particleSys = GetComponent<ParticleSystem>();
        m_mainParticle = m_particleSys.main;
        m_mainParticle.startColor = new ParticleSystem.MinMaxGradient(BeginColorStartMax, BeginColorStartMin);
    }

    private void FixedUpdate()
    {
        if(OnlyTracing == true)
        {
            transform.position = Vector2.MoveTowards(transform.position, GlobalVariable.GetPlayer().transform.position, DistanceDelta * Time.fixedDeltaTime);
        }
        else
        {
            if (m_bDash == false)
            {
                float fDistancePlayer = Vector2.Distance(transform.position, GlobalVariable.GetPlayer().transform.position);
                if (fDistancePlayer <= DistanceDashPlayer)
                {
                    m_bDash = true;
                    m_movingObj.enabled = false;
                    m_mainParticle.startColor = new ParticleSystem.MinMaxGradient(ChangeStateColorEnd, ChangeStateColorStart);
                }
            }
            else
            {
                float fDistanceBreakAway = Vector2.Distance(transform.position, GlobalVariable.GetPlayer().transform.position);
                if (fDistanceBreakAway >= PlayerBreakAwayDistance)
                {
                    m_bDash = false;
                    m_movingObj.enabled = true;
                    m_mainParticle.startColor = new ParticleSystem.MinMaxGradient(BeginColorStartMax, BeginColorStartMin);
                }
                else
                {
                    transform.position = Vector2.MoveTowards(transform.position, GlobalVariable.GetPlayer().transform.position, DistanceDelta * Time.fixedDeltaTime);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, DistanceDashPlayer );
    }
}
