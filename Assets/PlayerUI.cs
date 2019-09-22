using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerUI : MonoBehaviour
{
    // Start is called before the first frame update

    public bool hasHpUI = false;
    public Text playerHealthText;
    public PlayerControl1 pc;

    private void Awake()
    {
        pc = GetComponent<PlayerControl1>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(hasHpUI)  playerHealthText.text = pc.hp.ToString() + "/" + pc.maxhp;
    }

    
}
