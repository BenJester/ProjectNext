using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box_Nearby : MonoBehaviour
{

    Thing thing;
    public bool canSwap=false;
    public GameObject areaIndicator;
    public float distance;
    
    Transform player;
    Color tempColor;
    SpriteRenderer spr;
    AudioSource asr;
    Animator anim;
    GameObject area;
    SpriteRenderer areaSpr;

    public GameObject destroyParticle;

    // Start is called before the first frame update
    void Start()
    {

        anim = GetComponent<Animator>();
        spr = GetComponent<SpriteRenderer>();
        tempColor = spr.color;
        player = PlayerControl1.Instance.transform;
        thing = GetComponent<Thing>();
        asr = GetComponent<AudioSource>();

         area = Instantiate(areaIndicator, transform.position, Quaternion.identity);
        area.transform.parent = null;
        areaSpr = area.GetComponent<SpriteRenderer>();
        areaSpr.size = new Vector2(distance * 2, distance * 2);
        areaSpr.color = new Color(areaSpr.color.r, areaSpr.color.g, areaSpr.color.b, 0.05f);
        area.transform.parent = transform;
        


        canSwap = false;
        thing.hasShield = true;
        spr.color = tempColor;

        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, player.position) <= distance && player.position.x<=transform.position.x)
        {
            Open();

        }
        else {
            Close();
        
        }
    }



    void Open() {
        if (canSwap) return;
        canSwap = true;
        thing.hasShield = false;
        areaSpr.color = new Color(areaSpr.color.r, areaSpr.color.g, areaSpr.color.b, 0.5f);
        anim.CrossFade("near_box",0.01f);

    }

    void Close() {
        if (!canSwap) return;
        canSwap = false;
        areaSpr.color = new Color(areaSpr.color.r, areaSpr.color.g, areaSpr.color.b, 0.05f);
        thing.hasShield = true;
        anim.CrossFade("near_box_close", 0.01f);

    }

    public void Hide() {
        GameObject part1 = Instantiate(destroyParticle, transform.position, Quaternion.identity);
        Destroy(part1, 1f);
        Destroy(area, 0.2f);
        thing.Die();
        
    
    }


}
