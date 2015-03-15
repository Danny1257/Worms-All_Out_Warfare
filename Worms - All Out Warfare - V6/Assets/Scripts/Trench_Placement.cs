using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Trench_Placement : MonoBehaviour {

	public GameObject Tick, Cross, Moving_Building_Text;
	public LayerMask GroundMask;
	private List<GameObject> trenches = new List<GameObject>();
	//private GameObject[] trenches;
	private GameObject current_Trench;
	private GameObject Tick_Clone, Cross_Clone, Moving_Building_Text_Clone;
	private bool SettingTrenches;
	private Touch touch;
	private Grid grid;
	private GameObject Trench;
	private CameraControls CamControls;

	// Use this for initialization
	void Start () {
		SettingTrenches = false;
		grid = GetComponent<Grid> ();
		CamControls = GetComponent<CameraControls>();
	}
	
	// Update is called once per frame
	void Update () {

		//Debug.Log ("Trench Count = " + trenches.Count);

		Vector3 currentGrid = grid.ActiveGrid.position;

		if (SettingTrenches)
		{
			CamControls.SetCameraState(false);
			if ( Input.touchCount > 0)	// if player has touched the screen
			{
				//Debug.Log("Touch registered");
				touch = Input.touches[0];
				RaycastHit hit = new RaycastHit();
				Ray ray = Camera.main.ScreenPointToRay(touch.position);
				currentGrid = grid.ActiveGrid.position;
				Vector3 pos = new Vector3(currentGrid.x, currentGrid.y + 0.5f, currentGrid.z);

				if (Physics.Raycast(ray,out hit, Mathf.Infinity, GroundMask))	// if the player has touched the ground
				{
					//Debug.Log("Hit Ground");

					for (int n = 0; n < trenches.Count; n++)
					{
						Debug.Log("Trench number: " + n + " pos: " + trenches[n].transform.position);
						//Debug.Log("for loop");
						if (pos == trenches[n].transform.position)	// check if the current grid has a building placed on it??
						{
							//Debug.Log("CANNOT PLACE HERE");
						}
						else
						{
							if (n == trenches.Count - 1)
							{
							//Debug.Log("Trench has been placed");
							pos = new Vector3(currentGrid.x, currentGrid.y + 0.5f, currentGrid.z);
							Trench = new GameObject (); 
							Trench = current_Trench;
							Trench.transform.position = pos;
							Instantiate(Trench);
							trenches.Add(Trench);
							
							}
						}
					}


					if (trenches.Count == 0)
					{
						trenches.Add(current_Trench);
					}
				}			
			}
		}
		else
		{
			CamControls.SetCameraState(true);
		}

	}

	public void SetTrench(GameObject b)
	{
		SettingTrenches = true;
		current_Trench = (b);
		Vector3 currentGrid = grid.ActiveGrid.position;
		Vector3 pos2 = new Vector3(currentGrid.x, currentGrid.y + 0.5f, currentGrid.z);
		current_Trench.transform.position = pos2;
		Tick_Clone = (GameObject)Instantiate (Tick);
		Cross_Clone = (GameObject)Instantiate (Cross);
		Moving_Building_Text_Clone = (GameObject)Instantiate (Moving_Building_Text);
	}


}
