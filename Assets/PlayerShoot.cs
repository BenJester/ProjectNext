using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject bullet;
    public float speed;
    AudioSource asr;
    public Vector2 spriteOffset;

    public float homingAngle;
    public LayerMask homingScan;


    void Start()
    {
        asr = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot(Vector2 dir) {
        GameObject bulletIns =  Instantiate(bullet, (Vector2)transform.position+ spriteOffset+dir * 85f, Quaternion.identity);
        bulletIns.transform.localScale = new Vector3(1.5f, 1.5f,1.5f);
        bulletIns.GetComponent<SpriteRenderer>().color = Color.red;

        

        //Vector2 dir2 = new Vector2(Mathf.Sin(homingAngle) * dir.y + Mathf.Cos(homingAngle) * dir.x, Mathf.Cos(homingAngle) * dir.y+Mathf.Sin(homingAngle) * dir.x);

        //Vector2 dir3 = new Vector2(Mathf.Sin(-homingAngle) * dir.y + Mathf.Cos(-homingAngle) * dir.x, Mathf.Cos(-homingAngle) * dir.y + Mathf.Sin(-homingAngle) * dir.x);

        //List<Vector2> dirs = new List<Vector2>();
        //dirs.Add(dir);
        //dirs.Add(dir2);
        //dirs.Add(dir3);

        //foreach (Vector2 di in dirs) {
        //    RaycastHit2D hit = Physics2D.Raycast(transform.position, di, 100, homingScan);
        //    if (hit != null)  dir = di;
        //}
        ////x′=sinα∗y+cosα∗x,y′=cosα∗y−sinα∗x


        bulletIns.GetComponent<Rigidbody2D>().velocity = 0.25f*dir * speed;
        StartCoroutine(Acc(bulletIns));
    }

    IEnumerator Acc(GameObject bullet) {
        yield return new WaitForSeconds(0.15f);
        asr.Play();
              bullet.GetComponent<Rigidbody2D>().velocity *=11f;
    }
}
