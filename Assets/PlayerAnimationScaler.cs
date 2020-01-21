using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerAnimationScaler : MonoBehaviour {

    private Rigidbody2D myRb;
    public float threshold;
    private bool touchGround;
    public bool isDoing = false;

    void Awake () {
        myRb = GetComponent<Rigidbody2D> ();
    }
    // Start is called before the first frame update
    void Start () {

    }

    // Update is called once per frame
    void Update () {
       


        if(PlayerControl1.Instance.isTouchingGround!=touchGround && touchGround==false){
            touchGround = true;
            touchGround=PlayerControl1.Instance.isTouchingGround;
            //transform.DOKill();
            //StopAllCoroutines();
            StartCoroutine(Stop());
            return;
        }
        touchGround=PlayerControl1.Instance.isTouchingGround;
       

        if (!isDoing && Mathf.Abs (myRb.velocity.y) > threshold) {
            transform.DOScale (new Vector3 (0.9f, 1.1f, 1), 0.15f);
            isDoing = true;
        } else if (isDoing && Mathf.Abs (myRb.velocity.y) <= threshold) {
            transform.DOScale (new Vector3 (1f, 1f, 1), 0.15f);
            isDoing=false;
        }
    }

    IEnumerator Stop () {
        transform.DOScale (new Vector3 (1.2f, 0.75f, 1), 0.16f);
        yield return new WaitForSeconds(0.16f);
        transform.DOScale (new Vector3 (1, 1, 1), 0.05f);
        
    }
}