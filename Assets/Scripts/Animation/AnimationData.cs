using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
[System.Serializable]
public class AnimationParamData 
{
    public enum AnimationParamTyp
    {
        AnimationParamTyp_None,
        AnimationParamTyp_Int,
        AnimationParamTyp_String,
    }
    public string AnimationParam;
    public AnimationParamTyp ParamTyp;
    public string Paramvalue;

    private int m_nHashParam;
    public void ParamGenerate()
    {
        m_nHashParam = Animator.StringToHash(AnimationParam);
    }
    public int GetHashParam()
    {
        return m_nHashParam;
    }
    public bool GetIntValue(out int nVal)
    {
        return int.TryParse(Paramvalue, out nVal);
    }
}
[System.Serializable]
public class AnimationData
{
    public List<AnimationParamData> LstParam;
    public void Generate()
    {
        foreach(AnimationParamData _data in LstParam)
        {
            _data.ParamGenerate();
        }
    }

    public void PlayAnimation(Animator _animator)
    {
        foreach (AnimationParamData _data in LstParam)
        {
            //if(_data.ParamTyp == AnimationParamData.AnimationParamTyp.AnimationParamTyp_Int)
            //{
            //    _animator.SetInteger(_data.GetHashParam(),_data)
            //}
            //else if (_data.ParamTyp == AnimationParamData.AnimationParamTyp.AnimationParamTyp_String)
            //{

            //}
        }
    }
}