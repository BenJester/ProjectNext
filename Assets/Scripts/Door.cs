using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
	public bool active;
	public List<Thing> enemyList;
	public List<PhysicalButton> buttonList;
	public List<Thing> hostageList;

    public bool hasUIIndicator = false;
    private LineRenderer lr;
	public Vector3 origin;
	public Vector3 target;

	public float speed;

	bool won;

	void Awake()
	{
		origin = transform.position;
        lr = GetComponent<LineRenderer>();
		//buttonList = new List<PhysicalButton>();
	}
	void Start()
	{
        if (lr != null)
        {
            lr.positionCount = 3*(enemyList.Count+buttonList.Count+hostageList.Count);
            for (int i = 0; i < lr.positionCount; i++)
            {
                lr.SetPosition(i, transform.position);
            }
        }
        
    }

	void Update()
	{
		if (checkEnemies() && checkButtons() && checkHostages())
		{
			active = true;
			//anim.SetBool("Active", true);
			Open ();

            if (hasUIIndicator) ClearUI();

        }
		else
		{
            if (hasUIIndicator)
            {
                SetUIIndicator();
            }
			active = false;
			//anim.SetBool("Active", false);
			Close ();



		}
	}

	void Open()
	{
		if (transform.position.y > origin.y + target.y)
			return;
		transform.Translate (0f, speed, 0f);
	}

	void Close()
	{

		if (transform.position.y <= origin.y)
			return;
		transform.Translate (0f, -speed, 0f);
	}

	bool checkButtons()
	{
		for (int i = 0; i < buttonList.Count; i++)
		{
			if (buttonList[i].state != ClickState.IsClick)
				return false;
		}
		return true;
	}

	bool checkHostages()
	{
		for (int i = 0; i < hostageList.Count; i++)
		{
			if (hostageList[i].dead)
			{
				return false;

			}
		}
		return true;
	}

	bool checkEnemies()
	{
		for (int i = 0; i < enemyList.Count; i++)
		{
			if (!enemyList[i].dead)
			{
				return false;

			}
		}
		return true;
	}


    void ClearUI()
    {
        lr.enabled = false;
    }

    void SetUIIndicator()
    {
        lr.enabled = true;
        
        int index = 0;

        if (!checkEnemies())
        {
            for (int i = 0; i < enemyList.Count; i++)
            {
                if (enemyList[i] != null)
                {
                    lr.SetPosition(index, transform.position);
                    lr.SetPosition(index + 1, enemyList[i].gameObject.transform.position);
                    index += 1;
                }          
            }
        }else if (!checkButtons())
        {
            for (int i = 0; i < buttonList.Count; i++)
            {
                
                lr.SetPosition(index, transform.position);
                lr.SetPosition(index + 1, buttonList[i].gameObject.transform.position);
                index += 1;
            }
        }
        else if (!checkHostages())
        {
            for (int i = 0; i < hostageList.Count; i++)
            {
                
                lr.SetPosition(index, transform.position);
                lr.SetPosition(index + 1, hostageList[i].gameObject.transform.position);
                index += 1;
            }
        }
    }
}
