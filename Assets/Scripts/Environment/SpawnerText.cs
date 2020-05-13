using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnerText : MonoBehaviour
{
    public Text text;
    public List<Mech_EnemySpawner> spawnerList;
    public int currWave;
    public int totalWave;
    public Image progress;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void UpdateSpawnerLeft()
    {
        totalWave = 0;
        currWave = 0;
        foreach (Mech_EnemySpawner spawner in spawnerList)
        {
            totalWave += spawner.maxNum;
            currWave += spawner.count;
        }
        text.text = "第 " + currWave.ToString() + "/" + totalWave.ToString() + " 波";
        progress.fillAmount = (float) currWave / (float) totalWave;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSpawnerLeft();
    }
}
