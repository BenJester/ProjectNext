using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Fly_Boss : MonoBehaviour
{
    public List<Danmaku> danmakuList;
    public List<Danmaku> buffList;
    public float atkInterval;
    public float danmakuInterval;
    public float buffInterval;
    Enemy enemy;
    private void Start()
    {
        enemy = GetComponent<Enemy>();
        StartCoroutine(Attack());
        StartCoroutine(Power());
    }

    IEnumerator Attack()
    {
        while (enemy.health > 0)
        {
            int rand = Random.Range(0, danmakuList.Count);
            danmakuList[rand].Shoot();
            danmakuInterval = atkInterval * Mathf.Clamp(((GetComponent<Enemy>().health - 75f) / ((float) GetComponent<Enemy>().maxHealth - 75f)), 0.5f, 1f);
            yield return new WaitForSeconds(danmakuInterval);
        }
        
    }
    IEnumerator Power()
    {
        int i = 0;
        while (enemy.health > 0)
        {
            
            buffList[i % 2].Shoot();
            i += 1;

            yield return new WaitForSeconds(buffInterval);
        }

    }
}
