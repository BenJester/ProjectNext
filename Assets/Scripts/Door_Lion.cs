using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door_Lion : MonoBehaviour {
    public bool active;
    public bool isOpen = false;
    public List<Thing> enemyList;
    public List<PhysicalButton> buttonList;
    public List<Thing> hostageList;

    private Animator animator;
    public bool hasUIIndicator = false;
    public Vector3 origin;
    public Vector3 target;

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

    }

    void Update () {
        if (checkEnemies () && checkButtons () && checkHostages ()) {
            active = true;
            //anim.SetBool("Active", true);
            Open ();
            opened = true;
        } else {

            active = false;
            //anim.SetBool("Active", false);
            if (opened && doesNotCloseOnceOpened)
                return;
            Close ();

        }
    }

    void Open () {

        GetComponent<BoxCollider2D> ().enabled = false;

        animator.CrossFade ("Door", 0.01f);

    }

    void Close () {

        GetComponent<BoxCollider2D> ().enabled = true;

        animator.CrossFade ("DoorClose", 0.01f);

    }

    bool checkButtons () {

        for (int i = 0; i < buttonList.Count; i++) {
            if (buttonList[i].state != ClickState.IsClick)
                return false;
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