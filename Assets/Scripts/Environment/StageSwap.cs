using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSwap : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> Stages;
    public List<GameObject> Triggers;
    public int index;
    public Transform pos;
    public float speed;
    public float maxSpeed;
    public float acc;
    public float riseDelay;
    public float snapThreshold;
    public void Next()
    {
        StartCoroutine(MoveStage());
    }
    IEnumerator MoveStage()
    {
        float timer = 0f;
        speed = 0f;
        while (timer < riseDelay)
        {
            
            Stages[index].transform.position += Vector3.down * speed * Time.deltaTime;
            timer += Time.deltaTime;
            speed = Mathf.Clamp(speed + acc, 0f, maxSpeed);
            yield return new WaitForEndOfFrame();
        }
        index += 1;
        speed = maxSpeed;
        while (Vector3.Distance(pos.position, Stages[index].transform.position) > snapThreshold)
        {
            
            Vector3 diff = pos.position - Stages[index].transform.position;
            Stages[index].transform.position += diff.normalized * speed * Time.deltaTime;
            speed = Mathf.Clamp(speed - acc, 100f, maxSpeed);
            yield return new WaitForEndOfFrame();
        }
        Stages[index].transform.position = pos.position;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
