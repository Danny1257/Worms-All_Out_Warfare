using UnityEngine;
using System.Collections;

public class Leader_Selection : MonoBehaviour {

	public GUITexture[] Leader_Buttons;
	private GUITexture Leader_1_Clone, Leader_2_Clone, Leader_3_Clone;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown (0)) 
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
		}

	}
}
