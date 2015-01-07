using UnityEngine;
using System.Collections;

public class BuildingManager : MonoBehaviour {

	public GameObject[] buildings;
	private BuildingPlacement buildingPlacement;

	public LayerMask buildButtonMask, buildingButtonMask, fortButtonMask, towerButtonMask;
	private bool buildMenuOpen;

	public GameObject Build_Button, Build_Menu, Barracks_Button, Fort_Button, Tower_Button;
	private GameObject build_Menu_Clone, barracks_Button_Clone, fort_Button_Clone, tower_Button_Clone;

	// Use this for initialization
	void Start () {
		buildingPlacement = GetComponent<BuildingPlacement>();
		buildMenuOpen = false;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 m = Input.mousePosition;
		m = new Vector3(m.x, m.y+ transform.position.y, transform.position.y);
		
		Vector3 p = Camera.main.ScreenToWorldPoint(m);
		Vector3 n = new Vector3 (0.0f, -0.667f, 0.0f);

		if (Input.GetMouseButtonDown (0))
		{
			if(Build_Button.guiTexture.HitTest(Input.mousePosition))
			{
				Debug.Log("Hit BUild button");
				if (!buildMenuOpen)
				{
					build_Menu_Clone = (GameObject)Instantiate(Build_Menu);
					barracks_Button_Clone = (GameObject)Instantiate(Barracks_Button);
					fort_Button_Clone = (GameObject)Instantiate(Fort_Button);
					tower_Button_Clone = (GameObject)Instantiate(Tower_Button);
					Debug.Log("Hit the GUI build button");
					buildMenuOpen = true;
				}
				
			}
			else if(buildMenuOpen)
			{
				if (barracks_Button_Clone.guiTexture.HitTest(Input.mousePosition))
				{
					RemoveBuildMenu();
					buildingPlacement.SetItem(buildings [0]);
					Debug.Log("hit barracks_button");
				}
				else if (fort_Button_Clone.guiTexture.HitTest(Input.mousePosition))
				{
					RemoveBuildMenu();
					buildingPlacement.SetItem(buildings [1]);
					Debug.Log("hit fort_button");
				}
				else if (tower_Button_Clone.guiTexture.HitTest(Input.mousePosition))
				{
					RemoveBuildMenu();
					buildingPlacement.SetItem(buildings [2]);
					Debug.Log("hit fort_button");
				}
				else if (build_Menu_Clone.guiTexture.HitTest(Input.mousePosition))
				{
					Debug.Log("Hit the menu");
				}					
				else
				{
					RemoveBuildMenu();
				}
			}
		}
	}

	void OnGUI()
	{

	}

	void RemoveBuildMenu()
	{
		if (buildMenuOpen) {

			Destroy (build_Menu_Clone);
			Destroy (barracks_Button_Clone);
			Destroy (fort_Button_Clone);
			Destroy (tower_Button_Clone);
			buildMenuOpen = false;
		}
	}
}
