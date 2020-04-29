using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactToPlayer : MonoBehaviour
{

    Transform player;
    Enemy my_enemy;
    SpriteRenderer spr;
    bool isTurning=false;
    
    // Start is called before the first frame update
    void Start()
    {
        
        my_enemy = GetComponent<Enemy>();
        player = PlayerControl1.Instance.GetComponent<Transform>();
        spr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.position.x < transform.position.x && !isTurning)
        {
            isTurning = true;
            StopAllCoroutines();
            StartCoroutine(Turn(false));

        }
        else if(player.position.x > transform.position.x && !isTurning) {
            isTurning = true;
            StopAllCoroutines();
            StartCoroutine(Turn(true));
        }
    }

    IEnumerator Turn(bool isRight) {
        yield return new WaitForSeconds(0.2f);
        my_enemy.faceRight = isRight;
        spr.flipX = !isRight;
        isTurning = false;



    }
}
