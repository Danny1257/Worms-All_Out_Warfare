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
	public GameObject Warning_Bubble;

	private Touch touch;
	private float ButtonsOffset;
	private Vector2 OriginalTouchPos;
	private ResourceManagementGold resourcesGold;

	private struct ButtonObjects
	{
		public GameObject Button_Image;
		public bool Locked;
	}

	private ButtonObjects barracks_Button_Clone, AmmoDump_Button_Clone, Bank_Button_Clone, HQ_Button_Clone, Medic_Button_Clone, MunitionsFactory_Button_Clone, Research_Building_Button_Clone, Oil_Refinery_Button_Clone, Trench_Horizontal_Button_Clone, Trench_Vertical_Button_Clone, UpArrow_Button_Clone, DownArrow_Button_Clone;
	private GameObject build_Menu_Clone, Warning_Bubble_Clone;
	public const float Ammo_Dump_Cost = 100;
	public const float Barracks_Cost = 200;
	public const float HQ_Cost = 500;
	public const float Medic_Cost = 250;
	public const float Munitions_Factory_Cost = 300;
	public const float Research_Lab_Cost = 300;
	public const float Bank_Cost = 280;
	public const float Oil_Refinary_Cost = 280;


	// Use this for initialization
	void Start () {
		buildingPlacement = GetComponent<BuildingPlacement>();
		trench_Placement = GetComponent<Trench_Placement> ();
		buildMenuOpen = false;
		ButtonsOffset = 0.0f;
		CamControls = GetComponent<CameraControls>();
		resourcesGold = GetComponentInChildren<ResourceManagementGold> ();
		Place_Leader_Image ();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 m = Input.mousePosition;
		m = new Vector3 (m.x, m.y + transform.position.y, transform.position.y);

		Vector3 p = Camera.main.ScreenToWorldPoint (m);
		Vector3 n = new Vector3 (0.0f, -0.667f, 0.0f);

		if (buildMenuOpen)
		{
			if ( Input.touchCount > 0)	// if player has touched the screen
			{
				touch = Input.touches[0];

				if (UpArrow_Button_Clone.Button_Image.guiTexture.HitTest(touch.position))
				{
					if (Warning_Bubble_Clone != null)
						Destroy(Warning_Bubble_Clone);
					ScrollUp();

				}
				else if (DownArrow_Button_Clone.Button_Image.guiTexture.HitTest(touch.position))
				{
					if (Warning_Bubble_Clone != null)
						Destroy(Warning_Bubble_Clone);
					ScrollDown();
				}
				else if (AmmoDump_Button_Clone.Button_Image.guiTexture.HitTest(touch.position))
				{
					if (AmmoDump_Button_Clone.Locked == false)	// Check of the building is locked
					{
						RemoveBuildMenu();
						buildingPlacement.SetItem (buildings [0]);	 // Set the building
						if (Warning_Bubble_Clone != null)
							Destroy(Warning_Bubble_Clone);   		// remove any warning bubbles on screen
					}
					else	// if its locked display a warning bubble
					{
						DisplayWarningLabel(AmmoDump_Button_Clone);
					}
				}
				else if (barracks_Button_Clone.Button_Image.guiTexture.HitTest(touch.position))
				{
					if (barracks_Button_Clone.Locked == false)
					{
						RemoveBuildMenu();
						buildingPlacement.SetItem(buildings[1]);
						if (Warning_Bubble_Clone != null)
							Destroy(Warning_Bubble_Clone);
					}
					else
					{
						DisplayWarningLabel(barracks_Button_Clone);
					}

				}
				else if (HQ_Button_Clone.Button_Image.guiTexture.HitTest(touch.position))
				{
					if (HQ_Button_Clone.Locked == false)
					{
						if (EnoughResources(HQ_Cost))
						{
							RemoveBuildMenu();
							buildingPlacement.SetItem(buildings[2]);
							if (Warning_Bubble_Clone != null)
								Destroy(Warning_Bubble_Clone);
						}
					}
					else
					{
						DisplayWarningLabel(HQ_Button_Clone);
					}
				}
				else if (Medic_Button_Clone.Button_Image.guiTexture.HitTest(touch.position))
				{
					if (Medic_Button_Clone.Locked == false)
					{
						RemoveBuildMenu();
						buildingPlacement.SetItem(buildings[3]);
					}
					else
					{
						DisplayWarningLabel(Medic_Button_Clone);
					}
				}
				else if (MunitionsFactory_Button_Clone.Button_Image.guiTexture.HitTest(touch.position))
				{
					if (MunitionsFactory_Button_Clone.Locked == false)
					{
						RemoveBuildMenu();
						buildingPlacement.SetItem(buildings[4]);
					}
					else
					{
						DisplayWarningLabel(MunitionsFactory_Button_Clone);
					}

				}
				else if (Research_Building_Button_Clone.Button_Image.guiTexture.HitTest(touch.position))
				{
					if (Research_Building_Button_Clone.Locked == false)
					{
						RemoveBuildMenu();
						buildingPlacement.SetItem(buildings[5]);
					}
					else
					{
						DisplayWarningLabel(Research_Building_Button_Clone);
					}
				}
				else if (Trench_Horizontal_Button_Clone.Button_Image.guiTexture.HitTest(touch.position))
				{
					if (Trench_Horizontal_Button_Clone.Locked == false)
					{
						RemoveBuildMenu();
						trench_Placement.SetTrench(buildings[6]);
					}
					else
					{
						DisplayWarningLabel(Trench_Horizontal_Button_Clone);
					}
				}
				else if (Trench_Vertical_Button_Clone.Button_Image.guiTexture.HitTest(touch.position))
				{
					if (Trench_Vertical_Button_Clone.Locked == false)
					{
						RemoveBuildMenu();
						trench_Placement.SetTrench(buildings[7]);
					}
					else
					{
						DisplayWarningLabel(Trench_Vertical_Button_Clone);
					}
				}
				else if (Oil_Refinery_Button_Clone.Button_Image.guiTexture.HitTest(touch.position))
				{
					if (Oil_Refinery_Button_Clone.Locked == false)
					{
						RemoveBuildMenu();
						buildingPlacement.SetItem(buildings[8]);
					}
					else
					{
						DisplayWarningLabel(Oil_Refinery_Button_Clone);
					}
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

	void DisplayWarningLabel(ButtonObjects Button)
	{
		if (Warning_Bubble_Clone != null) 
		{
			Destroy(Warning_Bubble_Clone);
		}
		Warning_Bubble_Clone = (GameObject)Instantiate(Warning_Bubble);
		Vector3 Temp = new Vector3(0.85f, Button.Button_Image.transform.position.y, 0.3f);
		Warning_Bubble_Clone.transform.position = Temp;
	}

	
	bool EnoughResources(float Amount_Needed)
	{
		if (resourcesGold.Total_Gold >= Amount_Needed)
		{
			return true;
		}
		else
			return false;
	}
	

	void OpenBuildMenu()
	{
		Debug.Log ("Hit BUild button");							
		Debug.Log ("Hit the GUI build button");

		build_Menu_Clone = (GameObject)Instantiate (Build_Menu);
		/*AmmoDump_Button_Clone = (GameObject)Instantiate (Buttons [0]);
		barracks_Button_Clone = (GameObject)Instantiate (Buttons [1]);
		HQ_Button_Clone = (GameObject)Instantiate (Buttons [4]);
		Medic_Button_Clone = (GameObject)Instantiate (Buttons [5]);
		/*MunitionsFactory_Button_Clone = (GameObject)Instantiate (Buttons [6]);
		Research_Building_Button_Clone = (GameObject)Instantiate (Buttons [7]);
		UpArrow_Button_Clone = (GameObject)Instantiate (Buttons [2]);
		DownArrow_Button_Clone = (GameObject)Instantiate (Buttons [3]);
		Trench_Horizontal_Button_Clone = (GameObject)Instantiate (Buttons [8]);
		Trench_Vertical_Button_Clone = (GameObject)Instantiate (Buttons [9]);
		Oil_Refinery_Button_Clone = (GameObject)Instantiate (Buttons [10]);*/
		buildMenuOpen = true;


		// Create the button clone objects -- and assign whether the button is locked or not.
		AmmoDump_Button_Clone.Button_Image = (GameObject) Instantiate(Buttons [0]);
		AmmoDump_Button_Clone.Locked = true;
		//Instantiate (AmmoDump_Button_Clone.Button_Image);

		barracks_Button_Clone.Button_Image = (GameObject) Instantiate(Buttons [1]);
		barracks_Button_Clone.Locked = true;
		//Instantiate (barracks_Button_Clone.Button_Image);

		HQ_Button_Clone.Button_Image = (GameObject) Instantiate(Buttons[4]);
		HQ_Button_Clone.Locked = false;
		//Instantiate (HQ_Button_Clone.Button_Image);

		Medic_Button_Clone.Button_Image = (GameObject) Instantiate(Buttons[5]);
		Medic_Button_Clone.Locked = true;
		//Instantiate (Medic_Button_Clone.Button_Image);

		MunitionsFactory_Button_Clone.Button_Image = (GameObject) Instantiate(Buttons[6]);
		MunitionsFactory_Button_Clone.Locked = true;
		//Instantiate (MunitionsFactory_Button_Clone.Button_Image);

		Research_Building_Button_Clone.Button_Image = (GameObject) Instantiate(Buttons[7]);
		Research_Building_Button_Clone.Locked = true;
		//Instantiate (MunitionsFactory_Button_Clone.Button_Image);

		Trench_Horizontal_Button_Clone.Button_Image = (GameObject) Instantiate(Buttons[8]);
		Trench_Horizontal_Button_Clone.Locked = true;
		//Instantiate (Trench_Horizontal_Button_Clone.Button_Image);

		Trench_Vertical_Button_Clone.Button_Image = (GameObject) Instantiate(Buttons[9]);
		Trench_Vertical_Button_Clone.Locked = true;
		//Instantiate (Trench_Vertical_Button_Clone.Button_Image);

		Oil_Refinery_Button_Clone.Button_Image = (GameObject) Instantiate(Buttons[10]);
		Oil_Refinery_Button_Clone.Locked = true;
		//Instantiate (Oil_Refinery_Button_Clone.Button_Image);

		UpArrow_Button_Clone.Button_Image = (GameObject) Instantiate(Buttons[2]);
		UpArrow_Button_Clone.Locked = false;
		//Instantiate (UpArrow_Button_Clone.Button_Image);

		DownArrow_Button_Clone.Button_Image = (GameObject) Instantiate(Buttons[3]);
		DownArrow_Button_Clone.Locked = false;
		//Instantiate (DownArrow_Button_Clone.Button_Image);
	}

	void ScrollUp()
	{
		for (int n = 0; n < Buttons.Length - 2; n++)
		{
			if (n == 0)
			{
				Vector3 newPos = new Vector3(AmmoDump_Button_Clone.Button_Image.transform.position.x, AmmoDump_Button_Clone.Button_Image.transform.position.y - 0.015f, AmmoDump_Button_Clone.Button_Image.transform.position.z);
				AmmoDump_Button_Clone.Button_Image.transform.position = newPos;
			}
			else if (n == 1)
			{
				Vector3 newPos = new Vector3(barracks_Button_Clone.Button_Image.transform.position.x, barracks_Button_Clone.Button_Image.transform.position.y - 0.015f, barracks_Button_Clone.Button_Image.transform.position.z);
				barracks_Button_Clone.Button_Image.transform.position = newPos;
			}
			else if (n == 2)
			{
				Vector3 newPos = new Vector3(HQ_Button_Clone.Button_Image.transform.position.x, HQ_Button_Clone.Button_Image.transform.position.y - 0.015f, HQ_Button_Clone.Button_Image.transform.position.z);
				HQ_Button_Clone.Button_Image.transform.position = newPos;
			}
			else if (n== 3)
			{
				Vector3 newPos = new Vector3(Medic_Button_Clone.Button_Image.transform.position.x, Medic_Button_Clone.Button_Image.transform.position.y - 0.015f, Medic_Button_Clone.Button_Image.transform.position.z);
				Medic_Button_Clone.Button_Image.transform.position = newPos;
			}
			else if (n== 4)
			{
				Vector3 newPos = new Vector3(MunitionsFactory_Button_Clone.Button_Image.transform.position.x, MunitionsFactory_Button_Clone.Button_Image.transform.position.y - 0.015f, MunitionsFactory_Button_Clone.Button_Image.transform.position.z);
				MunitionsFactory_Button_Clone.Button_Image.transform.position = newPos;
			}
			else if (n == 5) 
			{
				Vector3 newPos = new Vector3(Research_Building_Button_Clone.Button_Image.transform.position.x, Research_Building_Button_Clone.Button_Image.transform.position.y - 0.015f, Research_Building_Button_Clone.Button_Image.transform.position.z);
				Research_Building_Button_Clone.Button_Image.transform.position = newPos;
			}
			else if (n == 6)
			{ 
				Vector3 newPos = new Vector3(Trench_Horizontal_Button_Clone.Button_Image.transform.position.x, Trench_Horizontal_Button_Clone.Button_Image.transform.position.y - 0.015f, Trench_Horizontal_Button_Clone.Button_Image.transform.position.z);
				Trench_Horizontal_Button_Clone.Button_Image.transform.position = newPos;
			}
			else if (n == 7)
			{
				Vector3 newPos = new Vector3(Trench_Vertical_Button_Clone.Button_Image.transform.position.x, Trench_Vertical_Button_Clone.Button_Image.transform.position.y - 0.015f, Trench_Vertical_Button_Clone.Button_Image.transform.position.z);
				Trench_Vertical_Button_Clone.Button_Image.transform.position = newPos;
			}
			else 
			{ 
				Vector3 newPos = new Vector3(Oil_Refinery_Button_Clone.Button_Image.transform.position.x, Oil_Refinery_Button_Clone.Button_Image.transform.position.y - 0.015f, Oil_Refinery_Button_Clone.Button_Image.transform.position.z);
				Oil_Refinery_Button_Clone.Button_Image.transform.position = newPos;
			}

		}
	}

	void ScrollDown()
	{
		for (int n = 0; n < Buttons.Length - 2; n++)
		{
			if (n == 0)
			{
				Vector3 newPos = new Vector3(AmmoDump_Button_Clone.Button_Image.transform.position.x, AmmoDump_Button_Clone.Button_Image.transform.position.y + 0.015f, AmmoDump_Button_Clone.Button_Image.transform.position.z);
				AmmoDump_Button_Clone.Button_Image.transform.position = newPos;
			}
			else if (n == 1)
			{
				Vector3 newPos = new Vector3(barracks_Button_Clone.Button_Image.transform.position.x, barracks_Button_Clone.Button_Image.transform.position.y + 0.015f, barracks_Button_Clone.Button_Image.transform.position.z);
				barracks_Button_Clone.Button_Image.transform.position = newPos;
			}
			else if (n == 2)
			{
				Vector3 newPos = new Vector3(HQ_Button_Clone.Button_Image.transform.position.x, HQ_Button_Clone.Button_Image.transform.position.y + 0.015f, HQ_Button_Clone.Button_Image.transform.position.z);
				HQ_Button_Clone.Button_Image.transform.position = newPos;
			}
			else if (n == 3)
			{
				Vector3 newPos = new Vector3(Medic_Button_Clone.Button_Image.transform.position.x, Medic_Button_Clone.Button_Image.transform.position.y + 0.015f, Medic_Button_Clone.Button_Image.transform.position.z);
				Medic_Button_Clone.Button_Image.transform.position = newPos;
			}
			else if (n== 4)
			{
				Vector3 newPos = new Vector3(MunitionsFactory_Button_Clone.Button_Image.transform.position.x, MunitionsFactory_Button_Clone.Button_Image.transform.position.y + 0.015f, MunitionsFactory_Button_Clone.Button_Image.transform.position.z);
				MunitionsFactory_Button_Clone.Button_Image.transform.position = newPos;
			}
			else if (n == 5)
			{
				Vector3 newPos = new Vector3(Research_Building_Button_Clone.Button_Image.transform.position.x, Research_Building_Button_Clone.Button_Image.transform.position.y + 0.015f, Research_Building_Button_Clone.Button_Image.transform.position.z);
				Research_Building_Button_Clone.Button_Image.transform.position = newPos;
			}
			else if (n == 6)
			{ 
				Vector3 newPos = new Vector3(Trench_Horizontal_Button_Clone.Button_Image.transform.position.x, Trench_Horizontal_Button_Clone.Button_Image.transform.position.y + 0.015f, Trench_Horizontal_Button_Clone.Button_Image.transform.position.z);
				Trench_Horizontal_Button_Clone.Button_Image.transform.position = newPos;
			}
			else if (n == 7)
			{
				Vector3 newPos = new Vector3(Trench_Vertical_Button_Clone.Button_Image.transform.position.x, Trench_Vertical_Button_Clone.Button_Image.transform.position.y + 0.015f, Trench_Vertical_Button_Clone.Button_Image.transform.position.z);
				Trench_Vertical_Button_Clone.Button_Image.transform.position = newPos;
			}
			else 
			{ 
				Vector3 newPos = new Vector3(Oil_Refinery_Button_Clone.Button_Image.transform.position.x, Oil_Refinery_Button_Clone.Button_Image.transform.position.y + 0.015f, Oil_Refinery_Button_Clone.Button_Image.transform.position.z);
				Oil_Refinery_Button_Clone.Button_Image.transform.position = newPos;
			}

		}
	}

	void Place_Leader_Image()
	{
		Leader_Image = GameObject.FindGameObjectWithTag ("Leader");

		Vector3 pos = new Vector3 (0.125f, 0.82f, 0);
		Vector2 scale = new Vector3 (0.23f, 0.35f, 0);

		Leader_Image.transform.position = pos;
		Leader_Image.transform.localScale = scale;
	}

	void RemoveBuildMenu()
	{
		if (buildMenuOpen) {

			Destroy (build_Menu_Clone);
			Destroy(AmmoDump_Button_Clone.Button_Image);
			//DestroyImmediate(Bank_Button_Clone.Button_Image, true);
			Destroy(barracks_Button_Clone.Button_Image);
			Destroy(HQ_Button_Clone.Button_Image);
			Destroy(Medic_Button_Clone.Button_Image);
			Destroy(MunitionsFactory_Button_Clone.Button_Image);
			Destroy(Research_Building_Button_Clone.Button_Image);
			Destroy(Trench_Horizontal_Button_Clone.Button_Image);
			Destroy(Trench_Vertical_Button_Clone.Button_Image);
			Destroy(UpArrow_Button_Clone.Button_Image);
			Destroy(DownArrow_Button_Clone.Button_Image);
			Destroy(Oil_Refinery_Button_Clone.Button_Image);
			buildMenuOpen = false;
			if (Warning_Bubble_Clone != null)
				Destroy(Warning_Bubble_Clone);

		}
	}
}
