using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitBossShoot : MonoBehaviour
{
    public SplitBossPart TransBody;
    public SplitBossPart TransLeftHand;
    public SplitBossPart TransRightHand;

    public float PartShootingTime;

    public bool ShootPart;

    public float ShootForce;

    private Rigidbody2D m_rigShootPart;
    private float m_fCurrentShootingTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(ShootPart == true)
        {
            ShootPart = false;
            _shootPart(TransLeftHand,new Vector2());
        }
        if(m_rigShootPart != null)
        {
            //m_rigShootPart.AddRelativeForce(Vector2.up * ShootForce);
            m_rigShootPart.AddForce(Vector2.up * ShootForce);
            m_fCurrentShootingTime += Time.deltaTime;
            if (m_fCurrentShootingTime>= PartShootingTime)
            {
                m_rigShootPart = null;
            }
        }
    }
    private void _shootPart(SplitBossPart _part, Vector2 vecDir)
    {
        _part.ThingEnable(true);
        m_rigShootPart = _part.GetComponent<Rigidbody2D>();
        m_fCurrentShootingTime = 0.0f;
    }
}
