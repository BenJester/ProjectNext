using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserRotation : MonoBehaviour
{
    // Start is called before the first frame update
    public float rotationSpeed;
    public List<Laser> lasers;
    public Color warningColor;
    public Color damageColor;
    public Animator bossAnimator;
    public int stage;

    void Start()
    {
        Close();
    }
    public void Warning()
    {
        foreach (var laser in lasers)
        {
            laser.sr.color = warningColor;
            laser.sr.enabled = true;
            laser.damageOn = false;
        }
    }
    public void Close()
    {
        foreach (var laser in lasers)
        {
            laser.sr.enabled = false;
            laser.damageOn = false;
        }
    }
    public void Damage()
    {
        foreach (var laser in lasers)
        {
            laser.sr.color = damageColor;
            laser.sr.enabled = true;
            laser.damageOn = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * rotationSpeed);
        int bossStage = bossAnimator.GetInteger("Stage");
        if (bossStage != stage)
            Close();
    }
}
