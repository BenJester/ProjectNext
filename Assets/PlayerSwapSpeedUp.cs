using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwapSpeedUp : MonoBehaviour
{
    // Start is called before the first frame update
    public int level;
    public int maxLevel;
    public GameObject levelupParticle;
    public float levelupSpeed;
    public float nowSpeed;
    public float levelDownCoolDown;
    float levelDowntemp;
    float initialSpeed;

    void Start()
    {
        initialSpeed = PlayerControl1.Instance.speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= levelDowntemp + levelDownCoolDown) {
            LevelDown();
        }
    }

    public void LevelUp() {
        levelDowntemp = Time.time;
        level += 1;
        level = Mathf.Clamp(level, 0, maxLevel);
        nowSpeed = initialSpeed + level * levelupSpeed;
        PlayerControl1.Instance.speed = nowSpeed;

        GameObject part =  Instantiate(levelupParticle, PlayerControl1.Instance.transform.position, Quaternion.identity);
        Destroy(part, 1f);
    }
    public void LevelDown() {
        level -= 1;
        level = Mathf.Clamp(level, 0, maxLevel);
        nowSpeed = initialSpeed + level * levelupSpeed;
        PlayerControl1.Instance.speed = nowSpeed;
        levelDowntemp = Time.time;



    }
}
