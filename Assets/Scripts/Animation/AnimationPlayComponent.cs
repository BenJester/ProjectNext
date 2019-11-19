using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayComponent : MonoBehaviour
{
    public AnimationData AnimationData;
    public string NameOfAnimation;
    // Start is called before the first frame update
    void Start()
    {
        AnimationData.Generate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAnimation(Animator _animator)
    {

    }
}
