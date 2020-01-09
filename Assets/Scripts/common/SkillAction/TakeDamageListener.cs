using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamageListener : MonoBehaviour
{
    private Enemy m_enemy;
    private UIBossHealth m_bossHealth;
    // Start is called before the first frame update
    void Start()
    {
        m_enemy = GetComponent<Enemy>();
        if(m_enemy != null)
        {
            m_enemy.RegisteTakeDamage(_updateDamage);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDestroy()
    {
        if (m_enemy != null)
        {
            m_enemy.RemoveTakeDamage(_updateDamage);

            //StartCoroutine(removeHealthBar());
        }
    }
    private void _updateDamage(int nDamage)
    {
        if(m_bossHealth == null)
        {
            m_bossHealth = FindObjectOfType<UIBossHealth>();
            if (m_bossHealth == null)
            {
                Debug.Assert(false);
            }
        }

        if (m_bossHealth != null)
        {
            m_bossHealth.UpdateHealth((float)m_enemy.health / (float)m_enemy.maxHealth);
        }
    }
    IEnumerator removeHealthBar()
    {
        yield return new WaitForSeconds(1f);
        m_bossHealth.OpenBossHealthBar(false);
    }
}
