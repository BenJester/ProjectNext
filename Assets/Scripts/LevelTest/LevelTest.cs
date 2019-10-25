using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelTest : MonoBehaviour
{
    public static LevelTest instance;
    public string currentLevel;
    public List<TestData> datas = new List<TestData>();
    public GameObject levelTestPanel;
    private Text dataPrint;
    public class TestData
    {
        public string levelName;
        public int deadNum;
        public int restartNum;
        public TestData(string _levelName, int _deadNum, int _restartNum)
        {
            levelName = _levelName;
            deadNum = _deadNum;
            restartNum = _restartNum;
        }
    }
    private void Awake()
    {
        if (instance)
            Destroy(gameObject);
        if (!instance) instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        dataPrint = levelTestPanel.GetComponentInChildren<Text>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            AddRestartNum(1);
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (levelTestPanel.activeSelf)
                levelTestPanel.SetActive(false);
            else
            {
                levelTestPanel.SetActive(true);
            }
        }
    }

    public void AddDeadNum(int num)
    {
        for (int i = 0; i < datas.Count; i++)
        {
            if (currentLevel == datas[i].levelName)
            {
                datas[i].deadNum += num;
                UpdateInfo();
                return;
            }
        }
        datas.Add(new TestData(currentLevel, 1, 0));
    }
    public void AddRestartNum(int num)
    {
        for (int i = 0; i < datas.Count; i++)
        {
            if (currentLevel == datas[i].levelName)
            {
                datas[i].restartNum += num;
                UpdateInfo();
                return;
            }
        }
        datas.Add(new TestData(currentLevel, 0, 1));
    }
    private void  UpdateInfo()
    {
        dataPrint.text = "";
        for (int i = 0; i < datas.Count; i++)
        {
            dataPrint.text += datas[i].levelName + ":\n 死亡=" + datas[i].deadNum + ", 重开=" + datas[i].restartNum+"\n\n";
        }
    }
}
