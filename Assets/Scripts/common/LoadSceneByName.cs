using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadSceneByName : MonoBehaviour
{
    public string SceneName;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("player"))
        {
            GameObject worldManager = GameObject.FindGameObjectWithTag("WorldManager");
            GameObject levelTest = GameObject.FindGameObjectWithTag("LevelManager");
            Destroy(worldManager);
            Destroy(levelTest);
            SceneManager.LoadScene(SceneName);
        }
    }
}
