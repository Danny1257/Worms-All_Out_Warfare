using UnityEngine;
using System.Collections;

public class CameraControls : MonoBehaviour {

	public GameObject plane;

	public float minSwipeDistY;	
	public float minSwipeDistX;	
	private Vector2 prevPos;
	private Vector2 currentPos;
	private float dragSpeed = 3;
	private Vector2 dragOrigin;
	private Touch touch;
	private Vector2 pos;
	private Vector3 move;
	public bool CamReady;
	private BuildingPlacement buildingPlacement;

	void Start()
	{
		CamReady = true;
	}


	void Update()
	{
		if (CamReady == true) {
			if (Input.touchCount > 0) { 	// if user is touching the screen
					touch = Input.touches [0];

					switch (touch.phase) {
					case TouchPhase.Began:
							dragOrigin = touch.position;
							break;
					case TouchPhase.Moved:
							pos = Camera.main.ScreenToViewportPoint (dragOrigin - touch.position);
							move = new Vector3 (pos.x * dragSpeed, 0, pos.y * dragSpeed);				
							transform.Translate (move, Space.World);
							break;
					case TouchPhase.Stationary:
							dragOrigin = touch.position;
							break;
					}
			}
		}
	




		/*if (Input.touchCount > 0)  // Screen has been touched
		{
			Touch touch = Input.touches[0];	

			switch (touch.phase) 				
			{				
				case TouchPhase.Began:				
					startPos = touch.position;				
					break;			
				
				case TouchPhase.Ended:				
					float swipeDistVertical = (new Vector3(0, touch.position.y, 0) - new Vector3(0, startPos.y, 0)).magnitude;				
					if (swipeDistVertical > minSwipeDistY)				
					{					
						float swipeValue = Mathf.Sign(touch.position.y - startPos.y);					
						if (swipeValue > 0)//up swipe
						{
						Debug.Log("UP SWIPE");
						}					
						//Jump ();						
						else if (swipeValue < 0)//down swipe
						{
						Debug.Log("Down SWIPE");
						}						
							//Shrink ();						
					}				
					float swipeDistHorizontal = (new Vector3(touch.position.x,0, 0) - new Vector3(startPos.x, 0, 0)).magnitude;
				
					if (swipeDistHorizontal > minSwipeDistX)					
					{					
						float swipeValue = Mathf.Sign(touch.position.x - startPos.x);					
						if (swipeValue > 0)//right swipe
						{
						//MoveRight ();
						Debug.Log("Right SWIPE");
						}
						else if (swipeValue < 0)//left swipe
						{	
							//MoveLeft ();
						Debug.Log("Left SWIPE");
						}	
					}
					break;
			}
		}*/
	}

	public void SetCameraState(bool s)
	{
		CamReady = s;
	}
}
