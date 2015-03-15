using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourceManagementGold : MonoBehaviour {

	public GUIText Gold_text;
	public GameObject Gold_Increment_Text;
	private float Gold_Increment;
	public float Total_Gold;
	public GameObject Gold_Collection;
	private float start_Time, gold_time;
	private GameObject Gold_Collector;
	private GameObject[] HQBuildings = null;
	private bool[] initialised = null;
	//private bool Gold_Collected;

	private struct HQBuildingList
	{
		public GameObject HQBuilding;
		public bool Gold_Collected;
		public bool initialised;
	}

	private struct GoldCollectionObject
	{
		public float gold_Time;
		public GameObject Gold_Collection_Object;
		public float gold_Accumulated;
	}
	
	private List<HQBuildingList> HQObjects = new List<HQBuildingList> ();
	private List<GoldCollectionObject> Gold_Collection_Objects = new List<GoldCollectionObject> ();

	// Use this for initialization
	void Start () {
		Total_Gold = 10000;
		Gold_Increment = 0;
		start_Time = Time.time;
		gold_time = 5.0f;


		//Gold_Collected = true;
	}
	
	// Update is called once per frame
	void Update () {

		if (Gold_Collection_Objects.Count > 0)
		{
			for (int q = 0; q < Gold_Collection_Objects.Count; q++)
			{
				GoldCollectionObject temp;
				temp.gold_Time = Gold_Collection_Objects[q].gold_Time - Time.deltaTime;
				temp.Gold_Collection_Object = Gold_Collection_Objects[q].Gold_Collection_Object;
				temp.gold_Accumulated = Gold_Collection_Objects[q].gold_Accumulated;
				Gold_Collection_Objects[q] = temp;
			}
		}
		// Increment each of the resources if their timer has reached 0 
		IncrementGold();

		if (HQObjects.Count > 0)
		{
			if (Gold_Collection_Objects.Count != 0)
			{
				for (int n = 0; n < Gold_Collection_Objects.Count; n++)
				{
					if (HQObjects[n].Gold_Collected == false)
					{
						if (Gold_Collection_Objects[n].Gold_Collection_Object != null)
						{
							Vector3 tempy = Camera.main.WorldToViewportPoint(HQObjects[n].HQBuilding.transform.position);
							tempy.y = Camera.main.WorldToViewportPoint(HQObjects[n].HQBuilding.transform.position).y + 0.16f;
							Gold_Collection_Objects[n].Gold_Collection_Object.transform.position = tempy;
						}
					}
				}
				if (Input.touchCount > 0)
				{
					for (int n = 0; n < Gold_Collection_Objects.Count; n++)
					{
						if (HQObjects[n].Gold_Collected == false)
						{
							if (Gold_Collection_Objects[n].Gold_Collection_Object.guiTexture.HitTest(Input.touches[0].position)) 
							{
								Destroy(Gold_Collection_Objects[n].Gold_Collection_Object);
								//Gold_Collection_Objects.RemoveAt(n);
								Total_Gold += Gold_Collection_Objects[n].gold_Accumulated;
								DisplayMoneyIncrementText(Gold_Collection_Objects[n]);
								HQBuildingList TempBuilding;
								TempBuilding.Gold_Collected = true;
								TempBuilding.HQBuilding = HQObjects[n].HQBuilding;
								TempBuilding.initialised = HQObjects[n].initialised;
								HQObjects[n] = TempBuilding;
								ParticleSystem[] particle = new ParticleSystem[HQObjects.Count];
								particle[n] = HQObjects[n].HQBuilding.GetComponentInChildren<ParticleSystem>();
								particle[n].Play();

								GoldCollectionObject TempCollectionObject;
								TempCollectionObject = Gold_Collection_Objects[n];
								TempCollectionObject.gold_Time = 5.0f;
								Gold_Collection_Objects[n] = TempCollectionObject;
							}
						}
					}
				}
			}
		}
		// Update all of the resource text displays
		Gold_text.text = "Gold: " + Total_Gold;

		// Delete Increment text's
		foreach(GameObject Increment in GameObject.FindGameObjectsWithTag("Increment_Text"))
		{
			if (Increment.transform.position.y > 3)
			{
				Destroy(Increment);
			}
		}
		
	}

	public void UpdateGold(float gold)
	{
		gold_time = 5.0f;	// reset the timer
		Gold_Increment += gold; // iincrease gold amount so there is a steady amount being incremented
	}

	public void SetHQBuildingList(Transform currentBuilding)
	{
		GameObject Building = currentBuilding.gameObject;
		HQBuildingList Temp;
		Temp.HQBuilding = Building;
		Temp.initialised = false;
		Temp.Gold_Collected = false;

		HQObjects.Add(Temp);

		GoldCollectionObject Temp2;
		Temp2.gold_Time = 5.0f;
		Temp2.Gold_Collection_Object = (GameObject)Instantiate(Gold_Collection);
		Temp2.gold_Accumulated = 0;
		Gold_Collection_Objects.Add(Temp2);
	}

	void DisplayMoneyIncrementText(GoldCollectionObject Collection_Symbol)
	{
		GameObject Temp;
		Temp = (GameObject)Instantiate (Gold_Increment_Text);
		Temp.transform.position = Collection_Symbol.Gold_Collection_Object.transform.position;
		Temp.guiText.text = ("+" + Collection_Symbol.gold_Accumulated);

		Temp.rigidbody.velocity = new Vector3 (0, 0.5f, 0);

	}

	void IncrementGold()
	{
		HQBuildings = null;
		HQBuildings = GameObject.FindGameObjectsWithTag("HQ");

		// Update Gold
		if (Gold_Collection_Objects.Count > 0)
		{
			for (int q = 0; q < Gold_Collection_Objects.Count; q++)
			{
				if (Gold_Collection_Objects[q].gold_Time <= 0) 
				{	
					GoldCollectionObject Temp;
					Temp = Gold_Collection_Objects[q];
					Temp.gold_Accumulated+= 100;
					Gold_Collection_Objects[q] = Temp;

					for (int n = 0; n < HQObjects.Count; n++)
					{
						if (Gold_Increment > 0)
						{
						
							if (HQObjects[n].Gold_Collected == true)
							{
								GameObject TempObject = (GameObject)Instantiate(Gold_Collection);
								Vector3 tempy = Camera.main.WorldToViewportPoint(HQBuildings[n].transform.position);
								tempy.y = Camera.main.WorldToViewportPoint(HQBuildings[n].transform.position).y + 0.46f;
								TempObject.transform.position = tempy;

								GoldCollectionObject TempCollector;
								TempCollector.Gold_Collection_Object = TempObject;
								TempCollector.gold_Time = 5.0f;
								TempCollector.gold_Accumulated = 100;
								Gold_Collection_Objects[n] = TempCollector;
								//Gold_Collection_Objects.Add(TempCollector);
								HQBuildingList TempBuilding;
								TempBuilding.Gold_Collected = false;
								TempBuilding.HQBuilding = HQObjects[n].HQBuilding;
								TempBuilding.initialised = HQObjects[n].initialised;
								HQObjects[n] = TempBuilding;
							}								
						}
					}

					GoldCollectionObject temp3;
					temp3.gold_Time = 5.0f;
					temp3.Gold_Collection_Object = Gold_Collection_Objects[q].Gold_Collection_Object;
					temp3.gold_Accumulated = Gold_Collection_Objects[q].gold_Accumulated;

					Gold_Collection_Objects[q] = temp3;		// reset the timer
				} /// End of Update gold
			}
		}
	}

	public void SpendResources(float gold)
	{
		Total_Gold -= gold;
	}
}
