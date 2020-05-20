using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject bullet;
    public float speed;
    AudioSource asr;



    void Start()
    {
        asr = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot(Vector2 dir) {
        GameObject bulletIns =  Instantiate(bullet, (Vector2)transform.position+dir*85f, Quaternion.identity);
        bulletIns.transform.localScale = new Vector3(1.5f, 1.5f,1.5f);
        bulletIns.GetComponent<SpriteRenderer>().color = Color.red;
        bulletIns.GetComponent<Rigidbody2D>().velocity = 0.25f*dir * speed;
        StartCoroutine(Acc(bulletIns));
    }

    IEnumerator Acc(GameObject bullet) {
        yield return new WaitForSeconds(0.23f);
              bullet.GetComponent<Rigidbody2D>().velocity *=11f;
    }
}
