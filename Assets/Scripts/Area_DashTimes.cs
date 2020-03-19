using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area_DashTimes : MonoBehaviour
{
    // Start is called before the first frame update

    public Dash dash;

    private void Awake()
    {
        dash = GameObject.FindGameObjectWithTag("player").GetComponent<Dash>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "player")
        {
            dash.charge = dash.maxCharge;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "player")
        {
            dash.charge = dash.maxCharge;
        }
    }
}
