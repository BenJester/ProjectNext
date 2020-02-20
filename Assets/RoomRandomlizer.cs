using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomRandomlizer : MonoBehaviour
{
    // Start is called before the first frame update

    public int OpenningRoomTimes=10;
    public int roomIndex=0;
    public int normalLevelIndex=0;
    public int upgradeLevelIndex=0;
    public GameObject[] normalLevels;
    public GameObject[] upgradeLevels;
    private GameObject tempGO;
    public Transform roomAchor;
    public Vector2 roomOffset;

    [Range(0f,0.5f)]
    public float posibilityUpgradeRoom;

    void Start()
    {
        Shuffle(normalLevels);
        Shuffle(upgradeLevels);
        for (int i = 0; i < OpenningRoomTimes; i++)
        {
            RandomRoom();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

        
     
     public void Shuffle(GameObject[] decklist ) {
         for (int i = 0; i < decklist.Length; i++) {
             int rnd = Random.Range(0, decklist.Length);
             tempGO = decklist[rnd];
             decklist[rnd] = decklist[i];
             decklist[i] = tempGO;
         }
    }

    public void RandomRoom(){
        if(RandomUpgrade()){
            SetRoom(upgradeLevels[upgradeLevelIndex]);
            upgradeLevelIndex+=1;
        }else{
            SetRoom(normalLevels[normalLevelIndex]);
            normalLevelIndex+=1;
        }
    }

    bool RandomUpgrade(){
        float temp = Random.Range(0F,1F);
        if(temp>posibilityUpgradeRoom){
            return false;
        }else return true;
    }

    public void SetRoom(GameObject room){
        room.transform.position=(Vector2)roomAchor.transform.position+roomOffset*roomIndex;
        roomIndex+=1;

    }
 }

