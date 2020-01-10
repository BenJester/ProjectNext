using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class ShadowMove : MonoBehaviour
{
    public float TimeToGenerateShadow;
    public float DestroyTime;
    public GameObject PrefabShadow;

    private SpriteRenderer m_spriteRender;
    private float m_fCurrentTime;
    // Start is called before the first frame update
    void Start()
    {
        m_spriteRender = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        m_fCurrentTime += Time.fixedDeltaTime;
        if( m_fCurrentTime >= TimeToGenerateShadow )
        {
            m_fCurrentTime -= TimeToGenerateShadow;

            GameObject _obj = Instantiate(PrefabShadow, Vector3.zero, Quaternion.identity);
            _obj.transform.position = transform.position;
            Destroy(_obj, DestroyTime);
        }
    }
}
