using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    private LevelTest test;
    private void Start()
    {
        test = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelTest>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!test)
            return;
        if (collision.gameObject. tag == "player")
        {
            test.currentLevel = name;
            for(int i = 0; i < test.datas.Count; i++)
            {
                if (test.datas[i].levelName == name)
                    return;
            }
            test.datas.Add(new LevelTest.TestData(name, 0, 0));
        }
    }
}
