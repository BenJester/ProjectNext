using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagSystem : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isCollect = false;
    public float collectRadius;
    public GameObject bagFirstThing;
    public GameObject indicator;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!isCollect)
            {
                StartCoroutine(StartCollection());
            }
            else
            {
                Set();
            }
        }
    }



    IEnumerator StartCollection(){
        GameObject area = Instantiate(indicator, transform.position, Quaternion.identity);
        area.transform.parent = null;
        area.GetComponent<SpriteRenderer>().size = new Vector2(collectRadius * 2, collectRadius * 2);
        area.transform.parent = transform;
        yield return new WaitForSeconds(0.5f);
        area.GetComponent<SpriteRenderer>().color=Color.green;
        Destroy(area,0.1f);
        Collect();
    }
        

    
    public void Collect()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, collectRadius, 1<<12);
       

        foreach (var item in cols)
        {
            if (item.tag == "thing")
            {
                bagFirstThing = item.gameObject;
                bagFirstThing.transform.position = (Vector2)transform.position + Vector2.up * 100000;
                isCollect = true;
                return;
            }
        }
    }

    public void Set()
    {
        if (bagFirstThing != null && isCollect)
        {
            bagFirstThing.transform.position = (Vector2)transform.position + Vector2.up * 50;
            isCollect = false;
        }
    }

}
