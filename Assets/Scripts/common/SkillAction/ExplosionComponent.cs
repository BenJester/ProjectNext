using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionComponent : MonoBehaviour
{
    public float StartRadius;
    public float EndRadius;
    public float TimeOfChagngingRadius;
    private CircleCollider2D m_collider;
    private bool m_bRunning;
    private float m_fCurrentTime;
    // Start is called before the first frame update
    void Start()
    {
        m_collider = GetComponent<CircleCollider2D>();
        m_collider.radius = StartRadius;
        m_bRunning = true;
    }
    private void FixedUpdate()
    {
        if(m_bRunning == true)
        {
            m_fCurrentTime += Time.fixedDeltaTime;
            float fRate = m_fCurrentTime / TimeOfChagngingRadius;
            float fCurRadius = StartRadius + (EndRadius - StartRadius) * fRate;
            m_collider.radius = fCurRadius;
            if (m_fCurrentTime >= TimeOfChagngingRadius)
            {
                m_bRunning = false;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if( collision.gameObject.CompareTag(GlobalTagDefine.TagName_player))
        {
            PlayerControl1 _player = collision.gameObject.GetComponent<PlayerControl1>();
            if(_player != null)
            {
                _player.Die();
            }
        }
    }
}
