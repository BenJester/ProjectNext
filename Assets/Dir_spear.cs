using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Dir_spear : MonoBehaviour
{
    // Start is called before the first frame update

    //public bool AimingPlayer = false;
    //public bool isTrigger=false;
    //public Transform target;

    //public float aimingSpeed;
    //public float speedAcc;
    //public float maxSpeed;

    [Space]
    Rigidbody2D rb;
    public float rotateSpeed;
    public float velocityThreshold;
    public float bouncingSpeed;
    public float floorBouncingThreshold;
    public GameObject DamageBox;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
       
        
        //Vector2 dir = target.transform.position - transform.position;
        //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.AngleAxis(angle - 90, new Vector3(0, 0, 1));
        ////yield return new WaitForSeconds(0.3f);
        //transform.DOMove(target.position, 0.5f, true).SetEase(Ease.InSine);
        ////target = PlayerControl1.Instance.gameObject.transform;
    }

    // Update is called once per frame
    void Update() { 

        CheckSpeed();

       
        
        


        //MoveToPlayer();
    }

    void CheckSpeed() {
        if (rb.velocity.magnitude > velocityThreshold)
        {
            DamageBox.SetActive(true);
            DoRotate();
        }
        else DamageBox.SetActive(false);


    }


    public void addSpeed() {
       
        StopAllCoroutines();
        rb.velocity *= 3;
    }

    public void MoveToPlayer() {
        Vector2 dir = PlayerControl1.Instance.gameObject.transform.position - transform.position;
        //rb.AddForce(dir* aimingSpeed);

    }

    public void DoRotate() {
        Vector2 dir = rb.velocity.normalized;
    

        float rotateAmount = Vector3.Cross(dir, transform.up).z;
        rb.angularVelocity = -rotateAmount * rotateSpeed;

    }


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


    public void JumpOffFloor() {
        Vector2 dir = PlayerControl1.Instance.gameObject.transform.position - transform.position;
        rb.velocity=(-rb.velocity.normalized + dir.normalized) * Random.Range(0.5f, 1f)* bouncingSpeed* rb.velocity.magnitude;
    
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "floor") {
            //JumpOffFloor();
            rb.velocity = Vector2.zero;
            rb.freezeRotation = true;
            rb.constraints=RigidbodyConstraints2D.FreezePosition;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "floor")
        {
            rb.freezeRotation = false;
            rb.constraints = RigidbodyConstraints2D.None;
        }
    }
    public void Unfreeze()
    {
        rb.freezeRotation = false;
        rb.constraints = RigidbodyConstraints2D.None;
    }



}
