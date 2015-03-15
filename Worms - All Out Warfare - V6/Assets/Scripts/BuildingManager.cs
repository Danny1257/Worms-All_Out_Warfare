using UnityEngine;
using System.Collections;

public class BuildingManager : MonoBehaviour {

	public GameObject[] buildings;
	private BuildingPlacement buildingPlacement;
	private Trench_Placement trench_Placement;
	private CameraControls CamControls;
	public LayerMask buildButtonMask, buildingButtonMask, fortButtonMask, towerButtonMask;
	public bool buildMenuOpen, scrolling_Up, scrolling_Down;
	private GameObject Leader_Image;

	public GameObject[] Buttons;
	private GameObject[] ButtonsClone;
	public GameObject Build_Button, Build_Menu, Barracks_Button, Cross_Button, Trench_button;
	private GameObject build_Menu_Clone, barracks_Button_Clone, AmmoDump_Button_Clone, Bank_Button_Clone, HQ_Button_Clone, Medic_Button_Clone, MunitionsFactory_Button_Clone, Research_Building_Button_Clone, Trench_Button_Clone, UpArrow_Button_Clone, DownArrow_Button_Clone;
	private Touch touch;
	private float ButtonsOffset;
	private Vector2 OriginalTouchPos;
	// Use this for initialization
	void Start () {
		buildingPlacement = GetComponent<BuildingPlacement>();
		trench_Placement = GetComponent<Trench_Placement> ();
		buildMenuOpen = false;
		ButtonsOffset = 0.0f;
		CamControls = GetComponent<CameraControls>();

		Place_Leader_Image ();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 m = Input.mousePosition;
		m = new Vector3 (m.x, m.y + transform.position.y, transform.position.y);

		Vector3 p = Camera.main.ScreenToWorldPoint (m);
		Vector3 n = new Vector3 (0.0f, -0.667f, 0.0f);

		if (Input.GetMouseButtonDown (0))
		{
			if (buildMenuOpen)
			{
				if ( Input.touchCount > 0)	// if player has touched the screen
				{
					touch = Input.touches[0];
					if (UpArrow_Button_Clone.guiTexture.HitTest(touch.position))
					{
						ScrollUp();							
					}
					else if (DownArrow_Button_Clone.guiTexture.HitTest(touch.position))
					{
						ScrollDown();
					}
					else if (AmmoDump_Button_Clone.guiTexture.HitTest(touch.position))
					{
						RemoveBuildMenu();
						buildingPlacement.SetItem (buildings [0]);
					}
					else if (barracks_Button_Clone.guiTexture.HitTest(touch.position))
					{
						RemoveBuildMenu();
						buildingPlacement.SetItem(buildings[1]);
					}
					else if (HQ_Button_Clone.guiTexture.HitTest(touch.position))
					{
						RemoveBuildMenu();
						buildingPlacement.SetItem(buildings[2]);
					}
					else if (Medic_Button_Clone.guiTexture.HitTest(touch.position))
					{
						RemoveBuildMenu();
						buildingPlacement.SetItem(buildings[3]);
					}
					else if (MunitionsFactory_Button_Clone.guiTexture.HitTest(touch.position))
					{
						RemoveBuildMenu();
						buildingPlacement.SetItem(buildings[4]);
					}
					else if (Research_Building_Button_Clone.guiTexture.HitTest(touch.position))
					{
						RemoveBuildMenu();
						buildingPlacement.SetItem(buildings[5]);
					}
					else if (Trench_Button_Clone.guiTexture.HitTest(touch.position))
					{
						RemoveBuildMenu();
						trench_Placement.SetTrench(buildings[6]);
					}
					else if (build_Menu_Clone.guiTexture.HitTest(touch.position))
					{
						Debug.Log("HitMenu");
					}
					else
					{
						RemoveBuildMenu();
					}
				}			
			}
			else if (Build_Button.guiTexture.HitTest (Input.mousePosition))
			{
				if (!buildMenuOpen)
				{
					OpenBuildMenu();
				}
			}
		}
	}
	

	void OpenBuildMenu()
	{
		Debug.Log ("Hit BUild button");							
		Debug.Log ("Hit the GUI build button");

		build_Menu_Clone = (GameObject)Instantiate (Build_Menu);
		AmmoDump_Button_Clone = (GameObject)Instantiate (Buttons [0]);
		barracks_Button_Clone = (GameObject)Instantiate (Buttons [1]);
		HQ_Button_Clone = (GameObject)Instantiate (Buttons [4]);
		Medic_Button_Clone = (GameObject)Instantiate (Buttons [5]);
		MunitionsFactory_Button_Clone = (GameObject)Instantiate (Buttons [6]);
		Research_Building_Button_Clone = (GameObject)Instantiate (Buttons [7]);
		UpArrow_Button_Clone = (GameObject)Instantiate (Buttons [2]);
		DownArrow_Button_Clone = (GameObject)Instantiate (Buttons [3]);
		Trench_Button_Clone = (GameObject)Instantiate (Buttons [8]);
		buildMenuOpen = true;
	}

	void ScrollUp()
	{
		for (int n = 0; n < Buttons.Length - 2; n++)
		{
			if (n == 0)
			{
				Vector3 newPos = new Vector3(AmmoDump_Button_Clone.transform.position.x, AmmoDump_Button_Clone.transform.position.y - 0.04f, AmmoDump_Button_Clone.transform.position.z);
				AmmoDump_Button_Clone.transform.position = newPos;
			}
			else if (n == 1)
			{
				Vector3 newPos = new Vector3(barracks_Button_Clone.transform.position.x, barracks_Button_Clone.transform.position.y - 0.04f, barracks_Button_Clone.transform.position.z);
				barracks_Button_Clone.transform.position = newPos;
			}
			else if (n == 2)
			{
				Vector3 newPos = new Vector3(HQ_Button_Clone.transform.position.x, HQ_Button_Clone.transform.position.y - 0.04f, HQ_Button_Clone.transform.position.z);
				HQ_Button_Clone.transform.position = newPos;
			}
			else if (n== 3)
			{
				Vector3 newPos = new Vector3(Medic_Button_Clone.transform.position.x, Medic_Button_Clone.transform.position.y - 0.04f, Medic_Button_Clone.transform.position.z);
				Medic_Button_Clone.transform.position = newPos;
			}
			else if (n== 4)
			{
				Vector3 newPos = new Vector3(MunitionsFactory_Button_Clone.transform.position.x, MunitionsFactory_Button_Clone.transform.position.y - 0.04f, MunitionsFactory_Button_Clone.transform.position.z);
				MunitionsFactory_Button_Clone.transform.position = newPos;
			}
			else if (n == 5) 
			{
				Vector3 newPos = new Vector3(Research_Building_Button_Clone.transform.position.x, Research_Building_Button_Clone.transform.position.y - 0.04f, Research_Building_Button_Clone.transform.position.z);
				Research_Building_Button_Clone.transform.position = newPos;
			}
			else
			{
				Vector3 newPos = new Vector3(Trench_Button_Clone.transform.position.x, Trench_Button_Clone.transform.position.y - 0.04f, Trench_Button_Clone.transform.position.z);
				Trench_Button_Clone.transform.position = newPos;
			}

		}
	}

	void ScrollDown()
	{
		for (int n = 0; n < Buttons.Length - 2; n++)
		{
			if (n == 0)
			{
				Vector3 newPos = new Vector3(AmmoDump_Button_Clone.transform.position.x, AmmoDump_Button_Clone.transform.position.y + 0.04f, AmmoDump_Button_Clone.transform.position.z);
				AmmoDump_Button_Clone.transform.position = newPos;
			}
			else if (n == 1)
			{
				Vector3 newPos = new Vector3(barracks_Button_Clone.transform.position.x, barracks_Button_Clone.transform.position.y + 0.04f, barracks_Button_Clone.transform.position.z);
				barracks_Button_Clone.transform.position = newPos;
			}
			else if (n == 2)
			{
				Vector3 newPos = new Vector3(HQ_Button_Clone.transform.position.x, HQ_Button_Clone.transform.position.y + 0.04f, HQ_Button_Clone.transform.position.z);
				HQ_Button_Clone.transform.position = newPos;
			}
			else if (n == 3)
			{
				Vector3 newPos = new Vector3(Medic_Button_Clone.transform.position.x, Medic_Button_Clone.transform.position.y + 0.04f, Medic_Button_Clone.transform.position.z);
				Medic_Button_Clone.transform.position = newPos;
			}
			else if (n== 4)
			{
				Vector3 newPos = new Vector3(MunitionsFactory_Button_Clone.transform.position.x, MunitionsFactory_Button_Clone.transform.position.y + 0.04f, MunitionsFactory_Button_Clone.transform.position.z);
				MunitionsFactory_Button_Clone.transform.position = newPos;
			}
			else if (n == 5)
			{
				Vector3 newPos = new Vector3(Research_Building_Button_Clone.transform.position.x, Research_Building_Button_Clone.transform.position.y + 0.04f, Research_Building_Button_Clone.transform.position.z);
				Research_Building_Button_Clone.transform.position = newPos;
			}
			else
			{
				Vector3 newPos = new Vector3(Trench_Button_Clone.transform.position.x, Trench_Button_Clone.transform.position.y + 0.04f, Trench_Button_Clone.transform.position.z);
				Trench_Button_Clone.transform.position = newPos;
			}

		}
	}

	void Place_Leader_Image()
	{
		Leader_Image = GameObject.FindGameObjectWithTag ("Leader");

		Vector3 pos = new Vector3 (0.125f, 0.82f, 0);

		Leader_Image.transform.position = pos;
	}

	void RemoveBuildMenu()
	{
		if (buildMenuOpen) {

			Destroy (build_Menu_Clone);
			Destroy(AmmoDump_Button_Clone);
			Destroy(Bank_Button_Clone);
			Destroy(barracks_Button_Clone);
			Destroy(HQ_Button_Clone);
			Destroy(Medic_Button_Clone);
			Destroy(MunitionsFactory_Button_Clone);
			Destroy(Research_Building_Button_Clone);
			Destroy(Trench_Button_Clone);
			Destroy(UpArrow_Button_Clone);
			Destroy(DownArrow_Button_Clone);
			buildMenuOpen = false;
		}
	}
}
