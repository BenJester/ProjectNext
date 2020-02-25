using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapPuzzlePlayerPosSet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerControl1.Instance.transform.position = SwapPuzzleManager.Instance.roomList[SwapPuzzleManager.Instance.currIndex].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
