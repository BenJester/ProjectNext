using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateByTarget : MonoBehaviour
{
    public Transform AroundTarget;//围绕的目标
    public float AngularSpeed;//角速度
    public float AroundRadius;//半径

    public float DiffAngle;

    private float m_fAngled;

    private SplitBossPartCollision m_collision;
    // Start is called before the first frame update
    void Start()
    {
        m_collision = GetComponent<SplitBossPartCollision>();
    }

    public void StartRotate(Transform _target)
    {
        AroundTarget = _target;
    }

    public void StopRotate()
    {
        AroundTarget = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(AroundTarget != null)
        {
            if(m_collision.IsCollision() == false)
            {
                m_fAngled += (AngularSpeed * Time.deltaTime) % 360;//累加已经转过的角度
                float posX = AroundRadius * Mathf.Sin((m_fAngled + DiffAngle) * Mathf.Deg2Rad);//计算x位置
                float posZ = AroundRadius * Mathf.Cos((m_fAngled + DiffAngle) * Mathf.Deg2Rad);//计算y位置

                transform.position = new Vector3(posX, posZ, 0) + AroundTarget.position;//更新位置   
            }
        }
    }
}
