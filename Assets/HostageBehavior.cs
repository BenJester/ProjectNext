using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostageBehavior : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject projectile;
    Swap _swap;
    Thing _thing;


    private void Awake()
    {
        
    }
    void Start()
    {
        _swap = PlayerControl1.Instance.GetComponent<Swap>();
        _thing = GetComponent<Thing>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReSwap(float time) {
        
        StartCoroutine(StartReSwap(time));
        _thing.SetShield(true);
    }

    IEnumerator StartReSwap(float _time) {
        yield return new WaitForSeconds(_time);
        _swap.col = transform.GetComponent<BoxCollider2D>();
        _swap.DoSwap();
        _thing.SetShield(false);


    }
}
