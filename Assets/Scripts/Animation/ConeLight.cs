using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ConeLight : MonoBehaviour
{
    // Start is called before the first frame update
    public float angleOffset = 0f;
    Transform player;

    [SerializeField]
    UnityEngine.Experimental.Rendering.LWRP.Light2D m_Light2D;

    void Start()
    {
        player = transform.parent;
        transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.position;
        Vector3 vecMouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log(vecMouseWorldPos);
        float angleToCursor = Mathf.Rad2Deg * PlayerControl1.AngleBetween(player.position, vecMouseWorldPos);
        if (PlayerControl1.Instance.player.GetAxis("AimHorizontal") != 0 || PlayerControl1.Instance.player.GetAxis("AimVertical") != 0)
        {
            m_Light2D.enabled = true;
            Vector2 dir = new Vector2(PlayerControl1.Instance.player.GetAxis("AimHorizontal"), PlayerControl1.Instance.player.GetAxis("AimVertical")).normalized;
            angleToCursor = Mathf.Rad2Deg * PlayerControl1.AngleBetween(Vector2.zero, dir);
        }
        else
        {
            m_Light2D.enabled = false;
            angleToCursor = Mathf.Rad2Deg * PlayerControl1.Instance.aimAngle;
        }
        transform.eulerAngles = new Vector3(0f, 0f, angleToCursor + angleOffset);
    }
}
