using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapPuzzleManager : MonoBehaviour
{
    public static SwapPuzzleManager Instance { get; private set; }
    public int currIndex;
    public SwapPuzzleRoom[] roomList;
    public float delay;

    void Start()
    {
        transform.parent = null;
        DontDestroyOnLoad(gameObject);

        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
        //roomList = GetComponentsInChildren<SwapPuzzleRoom>();
        
    }

    
    public IEnumerator Teleport()
    {
        yield return new WaitForSeconds(delay);
        if (currIndex + 1 >= roomList.Length) yield break;
        currIndex += 1;
        PlayerControl1.Instance.transform.position = roomList[currIndex].transform.position;
    }
}
