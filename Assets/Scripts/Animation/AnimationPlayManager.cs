using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationPlayManager : MonoBehaviour
{
    private Dictionary<string, AnimationPlayComponent> m_dic;
    // Start is called before the first frame update
    void Start()
    {
    }
    public void RegisteAnimation(string strAnimationName, AnimationPlayComponent _singleAnimation)
    {
        if (m_dic == null)
        {
            m_dic = new Dictionary<string, AnimationPlayComponent>();
        }
        if (m_dic.ContainsKey(strAnimationName) == false)
        {
            m_dic.Add(strAnimationName, _singleAnimation);
        }
        else
        {
            Debug.Assert(false, string.Format("动画已经被注册过了"));
        }
    }
    public void RegisteAnimationOverEvent(string strAnimationName, UnityAction acAnimationFinished)
    {
        AnimationPlayComponent _aniCom;
        if (m_dic.TryGetValue(strAnimationName, out _aniCom))
        {
            _aniCom.RegisteFinishedEvent(acAnimationFinished);
        }
        else
        {
            Debug.Assert(false);
        }
    }
    public void PlayAnimation(string strNameOfAnimation)
    {
        AnimationPlayComponent _aniCom;
        if (m_dic.TryGetValue(strNameOfAnimation, out _aniCom))
        {
            _aniCom.PlayAnimation();
        }
        else
        {
            Debug.Assert(false);
        }
    }
}
