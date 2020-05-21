using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ti_BurstBox : MonoBehaviour
{
    // Start is called before the first frame update

    public Vector2 dir;
    public Vector2 playerSpriteOffset;
    Thing thing;
    Vector2 originalPos;
    public float speed;
    public GameObject burstParticle;
    public GameObject Cursor;
    public bool hasCursor=false;
    public bool withPlayer=false;
    public GameObject dashEffect;

    
    void Start()
    {
        Cursor.transform.SetParent(null);
        thing = GetComponent<Thing>();
        originalPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasCursor)
        {
            Cursor.SetActive(true);
            if (withPlayer)
            {
                dir = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)PlayerControl1.Instance.transform.position+ playerSpriteOffset;
                Cursor.transform.position = (Vector2)PlayerControl1.Instance.transform.position+ playerSpriteOffset + dir.normalized * 70f;
                Cursor.transform.localRotation = Quaternion.Euler(0, 0, -Dash.AngleBetween(Vector2.up, dir));
            }
            else
            {
                dir = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - originalPos;
                Cursor.transform.position = (Vector2)originalPos + dir.normalized * 70f;
                Cursor.transform.localRotation = Quaternion.Euler(0, 0, -Dash.AngleBetween(Vector2.up, dir));

            }

        }
        else {
            Cursor.SetActive(false);
        }
        


    }

    public void Burst() {


        thing.hasShield = true;
        StartCoroutine(StartBurst());
        thing.Die();
        Destroy(gameObject, 1f);
        



    }


    IEnumerator StartBurst() {

        hasCursor = true;
        if (withPlayer) {
            Cursor.transform.SetParent(PlayerControl1.Instance.transform);
        } 
        else PlayerControl1.Instance.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        
        
        yield return new WaitForSeconds(0.5f);

        if (withPlayer)
        {
            //GameObject part2 = Instantiate(burstParticle, (Vector2)PlayerControl1.Instance.transform.position+ playerSpriteOffset, Quaternion.identity);
            //Destroy(part2, 0.5f);

            if (dashEffect != null) {
                GameObject part3 = Instantiate(dashEffect,(Vector2)PlayerControl1.Instance.transform.position+ playerSpriteOffset, Quaternion.identity);
                part3.transform.localRotation = Quaternion.Euler(0, 0, -Dash.AngleBetween(Vector2.up, dir));
            }

        }
        else {
            GameObject part1 = Instantiate(burstParticle, originalPos, Quaternion.identity);
            Destroy(part1, 0.5f);
        }
        

        PlayerControl1.Instance.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;

        PlayerControl1.Instance.GetComponent<AirJump>().charge = PlayerControl1.Instance.GetComponent<AirJump>().maxCharge;

        if (withPlayer) {
            dir = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)PlayerControl1.Instance.GetComponent<Transform>().position + playerSpriteOffset;
        }else dir = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - originalPos;
        PlayerControl1.Instance.GetComponent<Rigidbody2D>().velocity = dir.normalized * speed;
        StartCoroutine(DisableAirControl());
        hasCursor = false;

    }

    IEnumerator DisableAirControl()
    {
        PlayerControl1.Instance.GetComponent<AirJump>().active = false;
        PlayerControl1.Instance.disableAirControl = true;
        yield return new WaitForSeconds(0.2f);
        
        PlayerControl1.Instance.GetComponent<AirJump>().active = true;
        yield return new WaitForSeconds(0.5f);
            PlayerControl1.Instance.disableAirControl = false;
    }



}
