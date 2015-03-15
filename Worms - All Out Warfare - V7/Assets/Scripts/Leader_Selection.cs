using UnityEngine;
using System.Collections;

public class Leader_Selection : MonoBehaviour {

	public GameObject[] Leader_Buttons;
	public GameObject ArrowLeft, ArrowRight, Audio1, Audio2;
	private GameObject CurrentLeader;
	private GUITexture ArrowLeftClone, ArrowRightClone;
	private int index;

	// Use this for initialization
	void Start () {
		// On start Display First Leader image as well as left and right arrows
		CurrentLeader = (GameObject)Instantiate (Leader_Buttons [0]);
		index = 0;
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown (0))
		{
			if (Input.touchCount > 0) // user is touching the screen 
			{ 
				if (ArrowLeft.guiTexture.HitTest (Input.touches [0].position))
				{
					ScrollLeft();
					Audio1.audio.Play();

				} 
				else if (ArrowRight.guiTexture.HitTest (Input.touches [0].position))
				{
					ScrollRight();
					Audio1.audio.Play();
				}
				else
				{
					for (int n = 0; n < Leader_Buttons.Length; n++)
					{
						if (CurrentLeader.guiTexture.HitTest(Input.touches[0].position))
						{
							Debug.Log("Hit Button "+ Leader_Buttons[n].name);				
							DontDestroyOnLoad(CurrentLeader);
							Application.LoadLevel("GameScreen");					
						}
					}
				}
			}
		}
		



		/*if (Input.GetMouseButtonDown (0)) 
		{
			for (int n = 0; n < Leader_Buttons.Length; n++)
			{
				if (Leader_Buttons[n].guiTexture.HitTest(Input.mousePosition))
				{
					Debug.Log("Hit Button "+ Leader_Buttons[n].name);				
					DontDestroyOnLoad(Leader_Buttons[n]);
					Application.LoadLevel("GameScreen");					
				}
			}
		}*/

	}

	void ScrollLeft()
	{
		Destroy (CurrentLeader);
		if (index == 0)
			index = 5;
		else
			index--;
		
		CurrentLeader = (GameObject)Instantiate (Leader_Buttons [index]);
	}

	void ScrollRight()
	{
		Destroy(CurrentLeader);
		if (index == 5)
			index = 0;
		else
			index++;
		
		CurrentLeader = (GameObject)Instantiate(Leader_Buttons[index]);
	}
}
