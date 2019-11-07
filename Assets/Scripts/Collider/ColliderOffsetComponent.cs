using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ColliderOffsetComponent : MonoBehaviour
{
    public Vector2 DestOfMovement;
    public float SpeedOfOffset;

    private Collider2D m_collider;
    // Start is called before the first frame update
    void Start()
    {
        m_collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        m_collider.offset = Vector2.MoveTowards(m_collider.offset, DestOfMovement, SpeedOfOffset);
    }
}
