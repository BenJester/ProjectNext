using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMarkerComponent : MonoBehaviour
{
    private Animator m_animator;
    private Transform m_lastTrans;
    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    public void CloseMarker()
    {
        gameObject.SetActive(false);
    }
    public void UpdateTarget(Transform _curTrans)
    {
        gameObject.SetActive(true);
        if (m_lastTrans != _curTrans)
        {
            m_lastTrans = _curTrans;
            m_animator.speed /= Time.timeScale;
            m_animator.CrossFade("Focus", 0.001f);
        }
        transform.position = new Vector3(_curTrans.transform.position.x, _curTrans.transform.position.y, -1f);
    }
}
