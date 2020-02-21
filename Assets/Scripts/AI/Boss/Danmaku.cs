using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Danmaku : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject bullet;

    public bool shootAtOnce;
    public int shootNum;
    public bool randomStartFinish;
    public List<Transform> startList;
    public List<Transform> finishList;
    public Transform start;
    public Transform finish;
    public float xSpeed;
    public float ySpeed;
    public float shootInterval;
    public float bulletMoveDelay;

    IEnumerator MoveBullet(Rigidbody2D bulletBody, int index)
    {
        yield return new WaitForSeconds(bulletMoveDelay);
        bulletBody.velocity = new Vector2(xSpeed, ySpeed);
    }

    IEnumerator ShowBullet(Rigidbody2D bulletBody, int index)
    {
        bulletBody.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(index * 0.02f);
        bulletBody.GetComponent<SpriteRenderer>().enabled = true;
    }

    public void Shoot()
    {
        Vector3 currPos = start.position;
        Vector3 interval = (finish.position - start.position) / (float) shootNum;
        int rand = Random.Range(0, startList.Count);
        if (randomStartFinish)
        {
            currPos = startList[rand].position;
            interval = (finishList[rand].position - startList[rand].position) / (float)shootNum;
        }
            
        for (int i = 0; i < shootNum; i ++)
        {
            GameObject bulletObj = Instantiate(bullet, currPos, Quaternion.identity);
            Rigidbody2D bulletBody = bulletObj.GetComponent<Rigidbody2D>();
            StartCoroutine(MoveBullet(bulletBody, i));
            StartCoroutine(ShowBullet(bulletBody, i));

            currPos += interval;

        }
    }

    public void RandomShoot()
    {
        StartCoroutine(DoRandomShoot());
    }

    IEnumerator DoRandomShoot()
    {
        int curr = 0;
        while (curr < shootNum)
        {
            float rand = Random.Range(0f, 1f);
            Vector3 pos = start.position + rand * (finish.position - start.position);

            GameObject bulletObj = Instantiate(bullet, pos, Quaternion.identity);
            Rigidbody2D bulletBody = bulletObj.GetComponent<Rigidbody2D>();
            bulletBody.velocity = new Vector2(xSpeed, ySpeed);
            curr += 1;
            yield return new WaitForSeconds(shootInterval);
        }
    }

    void Start()
    {
        //Shoot();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
