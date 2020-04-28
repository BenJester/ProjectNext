using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostageBehavior : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject projectile;
    public float bulletSpeed;
    public float dashSpeed;
    Swap _swap;
    Thing _thing;
    int count=0;
    public AudioClip sound1;
    public AudioClip sound2;
    AudioSource _asr;
    Rigidbody2D _rb;

    private void Awake()
    {
        
    }
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _swap = PlayerControl1.Instance.GetComponent<Swap>();
        _thing = GetComponent<Thing>();
        _asr = GetComponent<AudioSource>();
        count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReSwap(float time) {
        _asr.clip = sound1;
        _asr.Play();


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
        _rb.gravityScale = 50;
        _asr.clip = sound2;
        _asr.Play();
        _thing.SetShield(false);
        _swap.col = transform.GetComponent<BoxCollider2D>();
        _swap.DoSwap();
        


    }


    public void Dash() {

        _rb.gravityScale = 0;
        bool faceRight = !_swap.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX;
        _rb.velocity = new Vector2(faceRight ? dashSpeed : -dashSpeed,0);


    }
}
