using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationSwap : MonoBehaviour
{
    private SpriteRenderer m_render;
    private void Start()
    {
        m_render = GetComponent<SpriteRenderer>();
    }
    public void DisableRender()
    {
        m_render.enabled = false;
    }
}
