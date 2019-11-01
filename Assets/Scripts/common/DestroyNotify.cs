using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DestroyNotify : MonoBehaviour
{
    private UnityAction m_acNotify;
    // Start is called before the first frame update
    private void OnDestroy()
    {
        if(m_acNotify != null)
        {
            m_acNotify.Invoke();
        }
    }

    public void RegisteNotifyTarget(UnityAction ac)
    {
        m_acNotify += ac;
    }
}
