using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Attack_Shoot : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject bullet;
    public float bulletSpeed;

    public float bulletRespawnDistance;
    public float bulletRespawnMaxHeight;
    public float bulletRespawnYOffset;
    public int bulletCount;
    int randomTemp;
    int dir;
    public List<GameObject> bulletList;
    public void Attack()
    {
        dir = transform.localScale.z > 0 ? 1 : -1;
        bulletList.Clear();
        randomTemp = Random.Range(0, bulletCount);
        for (int i = 0; i < bulletCount; i++)
        {
            if (i != randomTemp)
            {
                Vector3 pos = new Vector3(transform.position.x + transform.right.x*dir * bulletRespawnDistance, transform.position.y - bulletRespawnYOffset + (i + 1) * (bulletRespawnMaxHeight / bulletCount), 0);
                GameObject bulletIns = Instantiate(bullet, pos, Quaternion.identity);
                bulletList.Add(bulletIns);

            }

        }

        StartCoroutine(FireBullet());

    }

    IEnumerator FireBullet()
    {
        yield return new WaitForSeconds(0.5f);
        foreach (var item in bulletList)
        {
            item.GetComponent<Rigidbody2D>().velocity = transform.right * bulletSpeed*dir;
        }
    }

    void Start()
    {
        bulletList = new List<GameObject>();
        Attack();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
