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
    public string ParamValue;

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
        return int.TryParse(ParamValue, out nVal);
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
            if (_data.ParamTyp == AnimationParamData.AnimationParamTyp.AnimationParamTyp_Int)
            {
                int nVal = 0;
                if( _data.GetIntValue(out nVal) == true)
                {
                    _animator.SetInteger(_data.GetHashParam(), nVal);
                }
                else
                {
                    Debug.Assert(false);
                }
            }
            else if (_data.ParamTyp == AnimationParamData.AnimationParamTyp.AnimationParamTyp_String)
            {

            }
        }
    }
}