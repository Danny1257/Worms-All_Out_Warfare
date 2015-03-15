using UnityEngine;
using System.Collections;

public class BuildingPlacement : MonoBehaviour {

	private PlaceableBuilding placeAbleBuilding;
	private Transform currentBuilding;
	private bool hasPlaced;
	public LayerMask buildingsMask;
	public LayerMask MotionMask;
	public LayerMask MenuMask;
	public LayerMask buildButtonMask;
	public GameObject Tick, Cross, MovingBuilding_Text;
	private PlaceableBuilding placeAbleBuildingOld;
	private CheckForCollisions checkForCollisions;
	private bool Movingbuilding;
	private GameObject TickClone, CrossClone, MovingBuilding_Text_Clone;
	private Touch touch;
	private CameraControls CamControls;
	public GameObject groundPlane;
	private Grid grid;
	public BoxCollider motionCollider;
	//private AudioSource audio;

	// Use this for initialization
	void Start () {
		Movingbuilding = false;
		CamControls = GetComponent<CameraControls>();
		checkForCollisions = GetComponentInChildren<CheckForCollisions> ();
		grid = GetComponent<Grid> ();
		CamControls.SetCameraState (true);

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
						Destroy (CrossClone);
						Destroy (MovingBuilding_Text_Clone);
						audio.Play();
					}
				}

				if (CrossClone.guiTexture.HitTest(Input.mousePosition))
				{
					Destroy(currentBuilding.gameObject);
					Movingbuilding = false;
					Destroy (TickClone);
					Destroy (CrossClone);
					Destroy (MovingBuilding_Text_Clone);
				}
			}
			else
			{			/// allow the player to drag the building around the screen	
				motionCollider = currentBuilding.collider as BoxCollider;
				RaycastHit hit = new RaycastHit();
				Ray ray = Camera.main.ScreenPointToRay(touch.position);
				if (Physics.Raycast(ray,out hit, Mathf.Infinity, buildingsMask))	// has hit building
				{
					Movingbuilding = true;
				}

				if ( Input.touchCount > 0)	// if player has touched the screen
				{
					touch = Input.touches[0];
					if (Movingbuilding)
					{

						if (Physics.Raycast(ray,out hit, Mathf.Infinity, MotionMask))	// has hit building
						{
							Debug.Log("Hit Building");
							CamControls.SetCameraState(false);
							Vector3 currentGrid = grid.ActiveGrid.position;
							//CameraStationary = true;
							switch (touch.phase)
							{
							case TouchPhase.Began:
								Debug.Log("BEGIN MOVEMENT");
								Movingbuilding = true;
								break;
							case TouchPhase.Moved:
								Vector3 raypoint = ray.GetPoint((hit.distance));
								currentBuilding.position = new Vector3(currentGrid.x, currentGrid.y + 0.06142227f, currentGrid.z);
								//currentBuilding.position = new Vector3(raypoint.x, 0 + currentBuilding.localScale.y/2, raypoint.z); // map building to finger position
								break;
							case TouchPhase.Stationary:
								CamControls.SetCameraState(true);
								Movingbuilding = false;
								break;						
							case TouchPhase.Ended:
								CamControls.SetCameraState(true);
								Movingbuilding = false;
								break;
								CamControls.SetCameraState(true);
								break;
							}
						}
					}
				}
				else {
					CamControls.SetCameraState(true);
				}
			}
		}
		else {
						// Not moving a building
			//Debug.Log("Else statement");
			if (Input.GetMouseButtonDown(0)) {
				RaycastHit hit = new RaycastHit();
				//Ray ray = new Ray(new Vector3(p.x, Camera.main.transform.position.y, p.z), n);
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(ray,out hit, Mathf.Infinity, buildingsMask)) {		// out sets the orignal value directly
					if (placeAbleBuildingOld != null) {
							placeAbleBuildingOld.SetSelected(false);
					}
					hit.collider.gameObject.GetComponentInParent<PlaceableBuilding>().SetSelected(true);
					placeAbleBuildingOld = hit.collider.gameObject.GetComponentInParent<PlaceableBuilding>();
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
		checkForCollisions = currentBuilding.GetComponentInChildren<CheckForCollisions> ();
		//checkForCollisions = GetComponentInChildren<CheckForCollisions> ();
		if (checkForCollisions.colliders.Count > 0) {	// count is the length of list
			return false;
		}
		return true;
	}

	public void SetItem(GameObject b) {
		Debug.Log ("Setting the item");
		hasPlaced = false;
		currentBuilding = ((GameObject)Instantiate(b)).transform;
		currentBuilding.name = b.name;
		placeAbleBuilding = currentBuilding.GetComponent<PlaceableBuilding>();
		TickClone = (GameObject)Instantiate(Tick);
		CrossClone = (GameObject)Instantiate (Cross);
		MovingBuilding_Text_Clone = (GameObject)Instantiate (MovingBuilding_Text);
	}

	public void MoveBuildings() 
	{
		Vector3 m = Input.mousePosition;
		
		m = new Vector3(m.x, m.y, transform.position.y);
		
		Vector3 p = camera.ScreenToWorldPoint(m);
		currentBuilding.position = new Vector3(p.x, 0 + currentBuilding.localScale.y/2, p.z); // here change code to make building follow the players finger until another button is pressed to confirm the location?
		Movingbuilding = true;
		// lock the camera in place while moving the buildings
		Vector3 pos = new Vector3(0, 20, -8);
		transform.position = pos;
	}

	public void OnTriggerEnter(Collider motionCollider)
	{
		Debug.Log("Hit Building");
	}
}
