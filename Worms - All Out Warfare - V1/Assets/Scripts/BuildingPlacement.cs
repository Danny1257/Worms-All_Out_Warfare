﻿using UnityEngine;
using System.Collections;

public class BuildingPlacement : MonoBehaviour {

	private PlaceableBuilding placeAbleBuilding;
	private Transform currentBuilding;
	private bool hasPlaced;
	public LayerMask buildingsMask;
	public LayerMask MenuMask;
	public LayerMask buildButtonMask;
	public GameObject Tick;
	private PlaceableBuilding placeAbleBuildingOld;
	private bool Movingbuilding;
	private GameObject TickClone;
	private Touch touch;
	private CameraControls CamControls;
	public GameObject groundPlane;
	// Use this for initialization
	void Start () {
		Movingbuilding = false;
	}

	// Update is called once per frame
	void Update () {
		Vector3 m = Input.mousePosition;
		Vector3 pos = new Vector3(0, 20, -8);
		m = new Vector3(m.x, m.y, transform.position.y);

		Vector3 p = camera.ScreenToWorldPoint(m);
		Vector3 n = new Vector3 (0.0f, -0.667f, 0.0f);

		if (currentBuilding != null && !hasPlaced) 
		{
			if (Input.GetMouseButtonDown(0))
			{
				if (TickClone.guiTexture.HitTest(Input.mousePosition)) 	// if user clicks the tick then place the buildin
				{
					if (IsLegalPosition())
					{
						hasPlaced = true;
						Movingbuilding = false;
						Destroy (TickClone);
					}
				}
			}
			else
			{			/// allow the player to drag the building around the screen			

				//currentBuilding.position = new Vector3(p.x, 0 + currentBuilding.localScale.y/2, p.z); // here change code to make building follow the players finger until another button is pressed to confirm the location?
				//Movingbuilding = true;			
				//Vector3 pos = new Vector3(0, 20, -8);
				//transform.position = pos;

				if ( Input.touchCount > 0)	// if player has touched the screen
				{
					touch = Input.touches[0];
					// Check if the touch has first hit the building using raycasting
					RaycastHit hit = new RaycastHit();
					//Ray ray = new Ray(new Vector3(p.x, Camera.main.transform.position.y, p.z), n);
					Ray ray = Camera.main.ScreenPointToRay(touch.position);
					if (Physics.Raycast(ray,out hit, Mathf.Infinity, buildingsMask))	// has hit building
					{
						CamControls = GetComponent<CameraControls>();
						CamControls.SetCameraState(false);
						//CameraStationary = true;
						switch (touch.phase)
						{
						case TouchPhase.Began:
							Debug.Log("BEGIN MOVEMENT");
							Movingbuilding = true;
							break;
						case TouchPhase.Moved:
							Vector3 raypoint = ray.GetPoint((hit.distance));
							currentBuilding.position = new Vector3(raypoint.x, 0 + currentBuilding.localScale.y/2, raypoint.z); // map building to finger position
							break;
						case TouchPhase.Stationary:
							CamControls.SetCameraState(true);
							break;						
						case TouchPhase.Ended:
							CamControls.SetCameraState(true);
							break;
						case TouchPhase.Canceled:
							CamControls.SetCameraState(true);
							break;
						}
					}
				}
				else {
					CamControls.SetCameraState(true);
				}
			}
		}
		else {
			if (Input.GetMouseButtonDown(0)) {
				RaycastHit hit = new RaycastHit();
				//Ray ray = new Ray(new Vector3(p.x, Camera.main.transform.position.y, p.z), n);
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(ray,out hit, Mathf.Infinity, buildingsMask)) {		// out sets the orignal value directly
					if (placeAbleBuildingOld != null) {
							placeAbleBuildingOld.SetSelected(false);
					}
					hit.collider.gameObject.GetComponent<PlaceableBuilding>().SetSelected(true);
					placeAbleBuildingOld = hit.collider.gameObject.GetComponent<PlaceableBuilding>();
				}
				else if (Physics.Raycast(ray,out hit, Mathf.Infinity, MenuMask))
				{
					if (placeAbleBuildingOld != null) {
							placeAbleBuildingOld.SetSelected(false);
					}
					Debug.Log("Hit MENU!!");
					//hit.collider.gameObject.GetComponent<PlaceableBuilding>().SetSelected(true);
					placeAbleBuildingOld.SetSelected(true);
				}
				else
				{
					if (placeAbleBuildingOld != null) {
						placeAbleBuildingOld.SetSelected(false);
					}
				}
			}
		}
	}

	bool IsLegalPosition() {
		if (placeAbleBuilding.colliders.Count > 0) {	// count is the length of list
			return false;
		}
		return true;
	}

	public void SetItem(GameObject b) {
		hasPlaced = false;
		currentBuilding = ((GameObject)Instantiate(b)).transform;
		currentBuilding.name = b.name;
		placeAbleBuilding = currentBuilding.GetComponent<PlaceableBuilding>();
		TickClone = (GameObject)Instantiate(Tick);
	}

	public void MoveBuildings() 
	{
		Vector3 m = Input.mousePosition;
		
		m = new Vector3(m.x, m.y, transform.position.y);
		
		Vector3 p = camera.ScreenToWorldPoint(m);
		currentBuilding.position = new Vector3(p.x, 0 + currentBuilding.localScale.y/2, p.z); // here change code to make building follow the players finger until another button is pressed to confirm the location?
		Movingbuilding = true;			
		Vector3 pos = new Vector3(0, 20, -8);
		transform.position = pos;
	}
}
