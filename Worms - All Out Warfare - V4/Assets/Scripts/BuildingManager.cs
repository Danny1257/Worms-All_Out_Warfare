using UnityEngine;
using System.Collections;

public class BuildingManager : MonoBehaviour {

	public GameObject[] buildings;
	private BuildingPlacement buildingPlacement;

	public LayerMask buildButtonMask, buildingButtonMask, fortButtonMask, towerButtonMask;
	public bool buildMenuOpen;

	public GameObject[] Buttons;
	public GameObject Build_Button, Build_Menu, Barracks_Button, Fort_Button, Tower_Button, Cross_Button, Trench_button;
	private GameObject build_Menu_Clone, barracks_Button_Clone, fort_Button_Clone, tower_Button_Clone, cross_Button_Clone, trench_Button_Clone;
	private Touch touch;

	// Use this for initialization
	void Start () {
		buildingPlacement = GetComponent<BuildingPlacement>();
		buildMenuOpen = false;

	}
	
	// Update is called once per frame
	void Update () {
		Vector3 m = Input.mousePosition;
		m = new Vector3 (m.x, m.y + transform.position.y, transform.position.y);

		Vector3 p = Camera.main.ScreenToWorldPoint (m);
		Vector3 n = new Vector3 (0.0f, -0.667f, 0.0f);

		if (Input.GetMouseButtonDown (0))
		{
			if (Build_Button.guiTexture.HitTest (Input.mousePosition))
			{
				if (!buildMenuOpen)
				{
					OpenBuildMenu();
				}

			}
			else if (buildMenuOpen)
			{
				if (Cross_Button.guiTexture.HitTest (Input.mousePosition))
				{
					RemoveBuildMenu();
					buildMenuOpen = false;
					Debug.Log ("Hit Cross_Button");
				}
				else if (Barracks_Button.guiTexture.HitTest (Input.mousePosition)) {
					RemoveBuildMenu ();
					buildingPlacement.SetItem (buildings [0]);
					Debug.Log ("hit barracks_button");
				}
				else if (Fort_Button.guiTexture.HitTest (Input.mousePosition)) {
					RemoveBuildMenu ();
					buildingPlacement.SetItem (buildings [1]);
					Debug.Log ("hit fort_button");
				}
				else if (Tower_Button.guiTexture.HitTest (Input.mousePosition)) {
					RemoveBuildMenu ();
					buildingPlacement.SetItem (buildings [2]);
					Debug.Log ("hit fort_button");
				}
				else if (Trench_button.guiTexture.HitTest (Input.mousePosition)) {
					RemoveBuildMenu();
					buildingPlacement.SetItem(buildings[4]);
					Debug.Log("hit Trench Button");
				}
				else if (Build_Menu.guiTexture.HitTest (Input.mousePosition)) {
					Debug.Log ("Hit the menu");
				}			
			}
		}
	}

	void OpenBuildMenu()
	{
		Debug.Log ("Hit BUild button");							
		Debug.Log ("Hit the GUI build button");
		build_Menu_Clone = (GameObject)Instantiate(Build_Menu);
		barracks_Button_Clone = (GameObject)Instantiate(Barracks_Button);
		fort_Button_Clone = (GameObject)Instantiate(Fort_Button);
		tower_Button_Clone = (GameObject)Instantiate(Tower_Button);
		cross_Button_Clone = (GameObject)Instantiate(Cross_Button);
		trench_Button_Clone = (GameObject)Instantiate(Trench_button);
		buildMenuOpen = true;
	}

	void RemoveBuildMenu()
	{
		if (buildMenuOpen) {
			Destroy (cross_Button_Clone);
			Destroy (build_Menu_Clone);
			Destroy (barracks_Button_Clone);
			Destroy (fort_Button_Clone);
			Destroy (tower_Button_Clone);
			Destroy (trench_Button_Clone);
			buildMenuOpen = false;
		}
	}
}
