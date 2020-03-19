using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ti_JumpingBox : MonoBehaviour
{
    public float force;

    public GameObject[] SetObjects;
    public float time;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddForceToPlayer() {
        PlayerControl1.Instance.GetComponent<Rigidbody2D>().AddForce(Vector2.up * force);
    }

    public void AddHorizontalForceToPlayer() {
        StartCoroutine(disAbleAir());
        PlayerControl1.Instance.GetComponent<Rigidbody2D>().AddForce(transform.up * force);


    }

    IEnumerator disAbleAir() {
        PlayerControl1.Instance.disableAirControl = true;
        yield return new WaitForSeconds(0.2f);
        PlayerControl1.Instance.disableAirControl = false;
    }
    public void AddForceToSelf()
    {
        GetComponent<Rigidbody2D>().AddForce(Vector2.up * force);
    }

    public void BeginSetObject() {
        StartCoroutine(SetObj());
    }

    public void StartSpeedUp() {
        StartCoroutine(SpeedUp());
    }

    IEnumerator SpeedUp() {
        
        PlayerControl1.Instance.speed = 1000;
        //PlayerControl1.Instance.jumpSpeed = 1000;
        yield return new WaitForSeconds(1f);
        PlayerControl1.Instance.speed = 500;
        //PlayerControl1.Instance.jumpSpeed = 650;
    }

    IEnumerator SetObj() {
        foreach (var item in SetObjects)
        {
            item.SetActive(true);
        }
        yield return new WaitForSeconds(time);
        foreach (var item in SetObjects)
        {
            item.SetActive(false);
        }
    }
}
