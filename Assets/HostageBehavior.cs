using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostageBehavior : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject projectile;
    public float bulletSpeed;
    Swap _swap;
    Thing _thing;
    int count=0;


    private void Awake()
    {
        
    }
    void Start()
    {
        _swap = PlayerControl1.Instance.GetComponent<Swap>();
        _thing = GetComponent<Thing>();
        count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReSwap(float time) {
        
            StartCoroutine(StartReSwap(time));
            _thing.SetShield(true);
        
        
       
    }

    public void Shoot() {

        bool faceRight =!_swap.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX;

        GameObject newBullet = Instantiate(projectile, faceRight ? (transform.position + 60f * Vector3.right) : (transform.position + 60f * Vector3.left), Quaternion.identity);
        Rigidbody2D bulletBody = newBullet.GetComponent<Rigidbody2D>();
        bulletBody.velocity = new Vector2(faceRight ? bulletSpeed * 3 : -bulletSpeed * 3, 0f);

    }

    IEnumerator StartReSwap(float _time) {
        yield return new WaitForSeconds(_time);
        _thing.SetShield(false);
        _swap.col = transform.GetComponent<BoxCollider2D>();
        _swap.DoSwap();
        


    }
}
