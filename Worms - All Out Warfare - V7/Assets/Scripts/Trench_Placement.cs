using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Trench_Placement : MonoBehaviour {

	public GameObject Tick, Cross, Moving_Building_Text;
	public LayerMask GroundMask;
	public GameObject[] TrenchObjects;
	private Transform current_Trench;
	private GameObject Tick_Clone, Cross_Clone, Moving_Building_Text_Clone;
	private bool SettingTrenches, hasPlaced;
	private Touch touch;
	private Grid grid;
	private CameraControls CamControls;
	private CheckForCollisions checkForCollisions;
	private Vector3 currentGridPos;
	private bool firstTouchLifted;
	private GameObject TempTrench;
	private bool TrenchOne;
	private Vector3 TrenchPos;
	private int UsedGridCounter;
	private bool RemoveGroundList;
	private List<Transform> UsedGround = new List<Transform>();

	public GameObject[] Trenches;
	// Use this for initialization
	void Start () {
		//UsedGround[] = new Grid[100];
		SettingTrenches = false;
		grid = GetComponent<Grid> ();
		CamControls = GetComponent<CameraControls>();
		checkForCollisions = GetComponentInChildren<CheckForCollisions> ();
		firstTouchLifted = true;
		TrenchOne = true;
		UsedGridCounter = 0;
	}
	
	// Update is called once per frame
	void Update () {

		Trenches = GameObject.FindGameObjectsWithTag ("Trench");

		//Debug.Log ("Trench Count = " + trenches.Count);
		Vector3 currentGrid = grid.ActiveGrid.position;

		if (SettingTrenches) {
			if (Input.touchCount == 0)
			{
				firstTouchLifted = true;
			}

			if (firstTouchLifted) {

				if (!HasTrenchPlacementFinished ())
				{
					CamControls.SetCameraState (false);
					CheckForInput ();
				}
				else
					CamControls.SetCameraState(true);					

				foreach (GameObject Trench in Trenches)
				{
					if (!IsLegalPosition(Trench))
					{
						Debug.Log("Destroy Trench");
						Destroy(Trench);
						//UsedGround.RemoveAt(UsedGround.Count);
						//UsedGridCounter--;
					}
				}
			}
		}
	}

	void CheckForInput ()
	{
		//Debug.Log ("Check for input");
		RaycastHit hit = new RaycastHit();
		Ray ray = Camera.main.ScreenPointToRay(touch.position);
		if (Input.touchCount > 0) 
		{	// if player has touched the screen					
			touch = Input.touches [0];

			if (current_Trench.name == "Textured_Tunnel_Horizontal")
				TrenchPos  = new Vector3 (grid.ActiveGrid.position.x + 3.0f, grid.ActiveGrid.position.y , grid.ActiveGrid.position.z- 2.0f);
			else
				TrenchPos  = new Vector3 (grid.ActiveGrid.position.x + 2.0f, grid.ActiveGrid.position.y , grid.ActiveGrid.position.z+ 4.0f);


			
			bool NotValid;
			bool Valid = true;
				///// check if a building is already on this grid slot.	
			RemoveGroundList = false;	/// 
			
			foreach (GameObject Trench in Trenches)
			{			
				if (Trench.transform.position == TrenchPos)
				{
					//Debug.Log("Overlapping");
					Valid = false;
				}
			}

			if (Valid)
			{
				RemoveGroundObject();
				TempTrench = current_Trench.gameObject;
				TempTrench.transform.position = TrenchPos;
				GameObject Temp2;
				Temp2 = (GameObject)Instantiate(TempTrench);
				Debug.Log ("Temp2 pos: " + Temp2.transform.position);
				Trenches = GameObject.FindGameObjectsWithTag ("Trench");

				foreach (GameObject Trench in Trenches)
				{
					if (!IsLegalPosition(Trench))
					{
						Debug.Log("Destroy Trench");
						Destroy(Trench);
						RemoveGroundList = true;
						UsedGridCounter--;
						UsedGround.RemoveAt(UsedGridCounter);
					}
				}
				//RemoveGroundList = true;
				if (!RemoveGroundList)
				{
					UsedGround.Add(grid.ActiveGrid);
					UsedGround[UsedGridCounter] = grid.ActiveGrid;
					UsedGridCounter++;
				}
				else
				{
					UsedGridCounter--;
					UsedGround.RemoveAt(UsedGridCounter);
				}
			}		
		}
	}


			
	void RemoveGroundObject()
	{
		Color col = new Color (255, 0, 0);
		grid.ActiveGrid.gameObject.renderer.enabled = false; 
	}

	public bool HasTrenchPlacementFinished()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (Tick_Clone.guiTexture.HitTest(Input.mousePosition)) 	// if user clicks the tick then place the buildin
			{				
				Debug.Log("Hit Tick");

				hasPlaced = true;
				SettingTrenches = false;
				Destroy (Tick_Clone);
				Destroy (Cross_Clone);
				Destroy (Moving_Building_Text_Clone);
				CamControls.SetCameraState (true);
				foreach (GameObject Trench in Trenches)
				{
					Trench.tag = "Building";
				}
				UsedGround.Clear();
				UsedGridCounter = 0;

				Trenches = null;
				audio.Play();
				return true;
			}
			
			if (Cross_Clone.guiTexture.HitTest(Input.mousePosition))
			{
				foreach (GameObject Trench in Trenches)
				{
					Destroy(Trench);
				}

				CreateGroundObjects();


				SettingTrenches = false;
				Destroy (Tick_Clone);
				Destroy (Cross_Clone);
				Destroy (Moving_Building_Text_Clone);
				return true;
			}
		}

		return false;
	}

	void CreateGroundObjects()
	{
		for (int n = 0; n < UsedGridCounter; n++) 
		{
			for (int x = 0; x < 10; x++)
			{
				for (int z = 0; z < 10; z++)
				{
					if (grid.grid[x,z].transform.position == UsedGround[n].transform.position)
					{
						grid.grid[x,z].transform.renderer.enabled = true;
					}
				}
			}
		}
	}

	bool IsLegalPosition(GameObject Trench)
	{
		checkForCollisions = Trench.transform.GetComponentInChildren<CheckForCollisions> ();

		checkForCollisions.Hello ();
		if (checkForCollisions.colliders.Count > 0) {
			return false;
		}
		else
			return true;
	}

	public void SetTrench(GameObject b)
	{
		touch = Input.touches [0];
		firstTouchLifted = false;
		SettingTrenches = true;
		current_Trench = b.transform;
		current_Trench.name = b.name;
		Vector3 currentGridPos = new Vector3(grid.ActiveGrid.position.x, grid.ActiveGrid.position.y - 2.5f, grid.ActiveGrid.position.x);
		current_Trench.transform.position = currentGridPos;
		Tick_Clone = (GameObject)Instantiate (Tick);
		Cross_Clone = (GameObject)Instantiate (Cross);
		Moving_Building_Text_Clone = (GameObject)Instantiate (Moving_Building_Text);
	}		
}
