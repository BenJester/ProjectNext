using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    protected Scene defaultScene;

    public void Update()
    {
      /*  if (Input.GetKeyDown(KeyCode.F1))
        {
            SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }*/
        if (Input.GetKeyDown(KeyCode.F1))
        {
            GameObject worldManager = GameObject.FindGameObjectWithTag("WorldManager");
            GameObject levelTest = GameObject.FindGameObjectWithTag("LevelManager");
            Destroy(worldManager);
            Destroy(levelTest);
            SceneManager.LoadScene(0);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            GameObject worldManager = GameObject.FindGameObjectWithTag("WorldManager");
            GameObject levelTest = GameObject.FindGameObjectWithTag("LevelManager");
            Destroy(worldManager);
            Destroy(levelTest);
            SceneManager.LoadScene(1);
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            GameObject worldManager = GameObject.FindGameObjectWithTag("WorldManager");
            GameObject levelTest = GameObject.FindGameObjectWithTag("LevelManager");
            Destroy(worldManager);
            Destroy(levelTest);
            SceneManager.LoadScene(2);
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            GameObject worldManager = GameObject.FindGameObjectWithTag("WorldManager");
            GameObject levelTest = GameObject.FindGameObjectWithTag("LevelManager");
            Destroy(worldManager);
            Destroy(levelTest);
            SceneManager.LoadScene(3);
        }
    }
}
