using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableElasticThing : MonoBehaviour {
    // Start is called before the first frame update

    public Transform parent;
    private Vector2 originalPos;
    public float force=1f;
    private Rigidbody2D rb;

    private LineRenderer lr;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        rb =GetComponent<Rigidbody2D>();
        lr = GetComponent<LineRenderer>();
    }
    void Start () {
        parent = transform.parent;
        originalPos = parent.position;
    }

    // Update is called once per frame
    void Update () {
        if ((Vector2) transform.position != originalPos) {
            lr.enabled = true;
            lr.SetPosition(0,originalPos);
            lr.SetPosition(1,transform.position);

            rb.velocity += (Vector2)(originalPos-(Vector2)transform.position).normalized * force;
        }
    }
}