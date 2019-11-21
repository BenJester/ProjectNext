using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayManager : MonoBehaviour
{
    private Dictionary<string, AnimationPlayComponent> m_dic;
    // Start is called before the first frame update
    void Start()
    {
    }
    public void RegisteSkill(string strAnimationName, AnimationPlayComponent _singleAnimation)
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
    public void CastSkill(string strNameOfSkill)
    {
        AnimationPlayComponent _skill;
        if (m_dic.TryGetValue(strNameOfSkill, out _skill))
        {
            //_skill.CastSkill();
        }
    }
}
