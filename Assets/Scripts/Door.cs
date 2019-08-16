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


	public Vector3 origin;
	public Vector3 target;

	public float speed;

	bool won;

	void Awake()
	{
		origin = transform.position;
		buttonList = new List<PhysicalButton>();
	}
	void Start()
	{

	}

	void Update()
	{
		if (checkEnemies() && checkButtons() && checkHostages())
		{
			active = true;
			//anim.SetBool("Active", true);
			Open ();
		}
		else
		{
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
		if (transform.position.y < origin.y)
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

}
