using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BagSystem : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isCollect = false;
    public List<Image> itemImages;
    public float collectRadius;
    public List<GameObject> items;
    public int index;
    public GameObject indicator;
    PlayerControl1 playerControl;
    public float throwSpeed;
    public float throwOffset;
    public LayerMask colletLayer;
    public Image border;
    public Sprite empty;
    public bool mouseSetPosition = true;
    public bool RotateThing = false;
    void Start()
    {
        playerControl = PlayerControl1.Instance;
        for (int i = 0; i < itemImages.Count; i++)
        {
            if (items[i] != null)
                itemImages[i].sprite = items[i].GetComponent<SpriteRenderer>().sprite;
            else
                itemImages[i].sprite = empty;

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(1))
        {
            Throw();
            Collect();
            //if (items[index] == null)
            //{
            //    Collect();
            //}
            //else
            //{
            //    Throw();
            //}
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            index = 0;


        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            index = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            index = 2;
        }
        border.rectTransform.position = itemImages[index].rectTransform.position;
    }



    IEnumerator StartCollection()
    {
        GameObject area = Instantiate(indicator, transform.position, Quaternion.identity);
        area.transform.parent = null;
        area.GetComponent<SpriteRenderer>().size = new Vector2(collectRadius * 2, collectRadius * 2);
        area.transform.parent = transform;
        yield return new WaitForSeconds(0.1f);
        area.GetComponent<SpriteRenderer>().color = Color.green;
        Destroy(area, 0.1f);
        Collect();
    }



    public void Collect()
    {
        if (items[index] != null) return;
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, collectRadius, colletLayer);


        foreach (var item in cols)
        {
            items[index] = item.gameObject;
            items[index].transform.position = (Vector2)transform.position + Vector2.up * 100000;
            itemImages[index].sprite = item.GetComponent<SpriteRenderer>().sprite;
            isCollect = true;
            return;
        }
    }

    public void Throw()
    {
        if (items[index] == null) return;
        StartCoroutine(DoThrow(items[index].GetComponent<Rigidbody2D>()));
    }

    IEnumerator DoThrow(Rigidbody2D rb)
    {
        Vector2 dir ;
        if (rb.GetComponent<Thing>().hasShield) yield break;

        int layer = rb.gameObject.layer;
        rb.gameObject.layer = 19;

        items[index] = null;
        itemImages[index].sprite = empty;


        if (mouseSetPosition)
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dir= (mouseWorldPos - (Vector2)playerControl.transform.position).normalized;
        }else{
            dir = Vector2.up;
        }
        


        //TODO: 旋转

        //rb.transform.localRotation = Quaternion.Euler(0, 0, AngleBetween(Vector2.up, dir.normalized));

        if(RotateThing){
            rb.transform.rotation = Quaternion.identity;
            rb.DORotate(360f,1f);
        }
        rb.transform.position = playerControl.transform.position + (Vector3)dir * throwOffset;
        rb.gameObject.GetComponent<Rigidbody2D>().velocity = dir * throwSpeed;


        yield return new WaitForSeconds(0.15f);
        rb.gameObject.layer = layer;

    }
    //public void Set()
    //{
    //    if (bagFirstThing != null && isCollect)
    //    {
    //        bagFirstThing.transform.position = (Vector2)transform.position + Vector2.up * 50;
    //        itemImage.sprite=null;
    //        isCollect = false;
    //    }
    //}

    public static float AngleBetween(Vector2 vectorA, Vector2 vectorB)
    {
        float angle = Vector2.Angle(vectorA, vectorB);
        Vector3 cross = Vector3.Cross(vectorA, vectorB);

        if (cross.z > 0)
        {
            angle = 360 - angle;
        }

        return angle;
    }

}
