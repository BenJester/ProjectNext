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

    public Sprite buttonSprite;
    public Sprite clickSprite;
    public GameObject[] objectToActives;
    public GameObject[] objectToDisactives;

    
    public Vector3 offset;
    private Vector3 targetPosition;
    private Vector3 originalPosition;
    private SpriteRenderer spr;

	Goal goal;

    public ClickState state;
    [HideInInspector]


    // Use this for initialization
    void Start () {
        originalPosition = transform.position;
        targetPosition = transform.position + offset;
		goal = GameObject.FindGameObjectWithTag ("goal").GetComponent<Goal>();
		goal.buttonList.Add(GetComponent<PhysicalButton>());
        spr= GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {

   //     switch (state)
   //     {
   //         case ClickState.NoClick:
			//	transform.position = Vector3.Lerp(transform.position, originalPosition, 0.2f);
   //             break;
			//case ClickState.IsClick:
			//	transform.position = Vector3.Lerp(transform.position, targetPosition, 0.6f);
   //             break;
   //         default:
   //             break;
   //     }


           
        
	}



    void OnTriggerStay2D(Collider2D col)
    {


		if (!(col.CompareTag("floor") || col.CompareTag("CameraTrigger") || col.CompareTag("Untagged")))
        {
			if(clickSprite!=null && state!=ClickState.IsClick){
                spr.sprite = clickSprite;
            }
            state = ClickState.IsClick;
            
            

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
            if(buttonSprite!=null && state!=ClickState.NoClick){
                spr.sprite = buttonSprite;
            }

			state = ClickState.NoClick;

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
