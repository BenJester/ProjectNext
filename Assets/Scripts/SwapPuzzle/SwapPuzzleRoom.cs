using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapPuzzleRoom : MonoBehaviour
{
    public List<Enemy> enemyList;
    bool triggered;
    private void Update()
    {
        if (triggered) return;
        if (enemyList.Count == 0) return;
        foreach (var enemy in enemyList)
        {
            if (enemy == null)
                return;
            if (enemy != null && !enemy.GetComponent<Thing>().dead)
                return;
        }
        StartCoroutine(SwapPuzzleManager.Instance.Teleport());
        triggered = true;
    }

    
}
