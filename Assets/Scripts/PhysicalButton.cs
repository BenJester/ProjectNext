using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ClickState
{
	NoClick,
	BeingClick,
	IsClick,
	BeingUp,

}


public class PhysicalButton : MonoBehaviour {

    
    public bool canRevert=true;
    public bool animationRevert = true;

    public GameObject[] objectToActives;
    public GameObject[] objectToDisactives;

    
    public Vector3 offset;
    private Vector3 targetPosition;
    private Vector3 originalPosition;

	Goal goal;

    public ClickState state;
    [HideInInspector]


    // Use this for initialization
    void Start () {
        originalPosition = transform.position;
        targetPosition = transform.position + offset;
		goal = GameObject.FindGameObjectWithTag ("goal").GetComponent<Goal>();
		goal.buttonList.Add(GetComponent<PhysicalButton>());
    }
	
	// Update is called once per frame
	void Update () {

        switch (state)
        {
            case ClickState.NoClick:
                break;
            case ClickState.BeingClick:
                transform.position = Vector3.Lerp(transform.position, targetPosition, 0.6f);
                if ((transform.position - targetPosition).magnitude<=0.02f)
                {
                    state = ClickState.IsClick;
                }
                break;
			case ClickState.IsClick:
                break;
            case ClickState.BeingUp:
                transform.position = Vector3.Lerp(transform.position, originalPosition, 0.2f);
                if (transform.position == originalPosition)
                {
                    state = ClickState.NoClick;
                }
                break;
            default:
                break;
        }


           
        
	}



    void OnTriggerEnter2D(Collider2D col)
    {

        state = ClickState.BeingClick;

        if (!col.CompareTag("floor"))
        {
            foreach (var objectToActive in objectToActives)
            {
                objectToActive.SetActive(true);
            }

            foreach (var objectToDisable in objectToDisactives)
            {
                objectToDisable.SetActive(false);
            }
           
        }

    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (!col.CompareTag("floor"))
        {
            if (animationRevert)
            {
                state = ClickState.BeingUp;
            }
            

            if (canRevert)
            {
                foreach (var objectToActive in objectToActives)
                {
                    objectToActive.SetActive(false);
                }

                foreach (var objectToDisable in objectToDisactives)
                {
                    objectToDisable.SetActive(true);
                }
            }
        }
    }
}
