using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCursorIndicator : MonoBehaviour
{
    public float distance = 5f;
    private SpriteRenderer sr;

    // Start is called before the first frame update
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerControl1.Instance.IsKeyBoard() == false)
        {
            if (PlayerControl1.Instance.player != null && (PlayerControl1.Instance.player.GetAxis("MoveHorizontal") != 0 || PlayerControl1.Instance.player.GetAxis("MoveVertical") != 0))
            {
                sr.enabled = true;
                transform.position = (Vector2)transform.parent.position + new Vector2(PlayerControl1.Instance.player.GetAxis("MoveHorizontal"), PlayerControl1.Instance.player.GetAxis("MoveVertical")).normalized * distance;
            }
            else
            {
                sr.enabled = false;

            }
        }
    }
}
