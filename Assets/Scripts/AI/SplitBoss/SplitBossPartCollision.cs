using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitBossPartCollision : MonoBehaviour
{
    private Rigidbody2D m_rigidbody;
    private SplitBossPart m_part;
    private bool m_bCollide;
    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_part = GetComponent<SplitBossPart>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if( collision.gameObject.tag == "floor")
        {
            m_part.SetGravity();
            m_bCollide = true;
        }
    }
    public bool IsCollision()
    {
        return m_bCollide;
    }
}
