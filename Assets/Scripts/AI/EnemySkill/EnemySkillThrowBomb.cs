using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillThrowBomb : MonoBehaviour
{
    public GameObject ObjShoot;
    public float objInstanceDistance;
    public float objSpeed;

    private Transform m_transPlayer;
    // Start is called before the first frame update
    void Start()
    {
        m_transPlayer = GlobalVariable.GetPlayer().transform;
        if (m_transPlayer == null)
        {
            Debug.Assert(false);
        }
    }

    public void ShootSomething()
    {
        Vector3 direction = (Vector3.up * 2f + (m_transPlayer.position - transform.position).normalized).normalized;
        GameObject newBullet = Instantiate(ObjShoot, transform.position + objInstanceDistance * (Vector3)direction, Quaternion.identity);
        Rigidbody2D bulletBody = newBullet.GetComponent<Rigidbody2D>();
        bulletBody.velocity = direction * objSpeed;
    }
}
