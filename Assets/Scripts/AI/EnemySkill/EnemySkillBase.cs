using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillBase : MonoBehaviour
{

    protected Transform m_transPlayer;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        m_transPlayer = GlobalVariable.GetPlayer().transform;
        if (m_transPlayer == null)
        {
            Debug.Assert(false);
        }
    }

    public virtual void CastSkill()
    {

    }
}
