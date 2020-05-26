using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class Door_Lion : MonoBehaviour {
    public bool active;
    public bool isChecking = true;
    public bool isOpen = false;
    public List<Thing> enemyList;
    public List<PhysicalButton> buttonList;
    public List<Thing> hostageList;
    public List<Mech_EnemySpawner> spawnerList;
    public List<Key> keyList;
    public FlameGroup flameList;
    public List<MovingLaser> laserToTurnOff;

    public bool isHostageCountdownDoor;
    public float hostageCountdown;
    float currHostageTimer;
    public MechTriggerArea triggerArea;

    private Animator animator;
    public bool hasUIIndicator = false;
    public Vector3 origin;
    public Vector3 target;


    public UnityEvent openEvent;
    public UnityEvent closeEvent;

    public float speed;

    bool won;
    public bool doesNotCloseOnceOpened = false;
    bool opened;

    void Awake () {
        origin = transform.position;
        animator = GetComponent<Animator> ();

        //buttonList = new List<PhysicalButton>();
    }
    void Start () {

        if (isOpen) {
            Open();
        }
    }


    void Update () {
        if (triggerArea != null && triggerArea.activated)
            currHostageTimer += Time.deltaTime;

        if (isChecking) {
            if ((isHostageCountdownDoor && currHostageTimer > hostageCountdown && checkHostages()) || (!isHostageCountdownDoor && checkEnemies() && checkButtons() && checkHostages() && checkSpawner() && checkKey() && checkFlame()))
            {
                active = true;
                //anim.SetBool("Active", true);
                Open();
                opened = true;
            }
            else
            {

                active = false;
                //anim.SetBool("Active", false);
                if (opened && doesNotCloseOnceOpened)
                    return;
                Close();

            }


        }
        
    }

    public void Open () {

        animator.CrossFade("Door", 0.01f);
        foreach (var laser in laserToTurnOff)
        {
            laser.active = false;
        }
        GetComponent<BoxCollider2D> ().enabled = false;
        openEvent.Invoke();



    }

    public void Close () {

        GetComponent<BoxCollider2D> ().enabled = true;

        animator.CrossFade ("DoorClose", 0.01f);
        closeEvent.Invoke();

    }

    bool checkButtons () {

        for (int i = 0; i < buttonList.Count; i++) {
            if (buttonList[i].state != ClickState.IsClick)
                return false;
        }
        return true;
    }

    public bool checkSpawner()
    {
        foreach (var spawner in spawnerList)
        {
            if (!spawner.lastSpawnedDead) return false;
        }
        foreach (var spawner in spawnerList)
        {
            spawner.end = true;
        }
        return true;
    }
    bool checkKey()
    {
        for (int i = 0; i < keyList.Count; i++)
        {
            if (!keyList[i].activated)
            {
                return false;
            }
        }
        return true;
    }
    bool checkFlame()
    {
        if (flameList == null) return true;
        for (int i = 0; i < flameList.flames.Length; i++)
        {
            if (!flameList.flames[i].active)
            {
                return false;
            }
        }
        return true;
    }
    bool checkHostages () {
        for (int i = 0; i < hostageList.Count; i++) {
            if (hostageList[i].dead) {
                return false;

            }
        }
        return true;
    }

    bool checkEnemies () {
        for (int i = 0; i < enemyList.Count; i++) {
            if (enemyList[i] == null)
                continue;
            if (!enemyList[i].dead) {
                return false;

            }
        }
        return true;
    }

    void ClearUI () {

    }

    void SetUIIndicator () {

        int index = 0;

        if (!checkEnemies ()) {
            for (int i = 0; i < enemyList.Count; i++) {
                if (enemyList[i] != null) {
                    index += 2;
                }
            }
        } else if (!checkButtons ()) {
            for (int i = 0; i < buttonList.Count; i++) {

                index += 2;
            }
        } else if (!checkHostages ()) {
            for (int i = 0; i < hostageList.Count; i++) {
                if (hostageList[i] != null) {
                    index += 2;
                }
            }

        }
    }
}