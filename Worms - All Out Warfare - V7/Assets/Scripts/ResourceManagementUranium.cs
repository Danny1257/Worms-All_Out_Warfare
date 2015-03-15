using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourceManagementUranium : MonoBehaviour {

	public GUIText Uranium_Text;
	public float Total_Uranium;
	public GameObject Uranium_Collection;
	public GameObject Uranium_Increment_Text;

	private float Uranium_Increment;
	private float start_Time, uranium_time;
	private GameObject Uranium_Collector;
	private GameObject[] HQBuildings = null;
	private bool[] initialised = null;

	private struct ResearchBuildingList
	{
		public GameObject ResearchBuilding;
		public bool Uranium_Collected;
		public bool initialised;
	}
	
	private struct UraniumCollectionObject
	{
		public float uranium_Time;
		public GameObject Uranium_Collection_Object;
		public float uranium_Accumulated;
	}
	
	private List<ResearchBuildingList> ResearchBuildingObjects = new List<ResearchBuildingList> ();
	private List<UraniumCollectionObject> Uranium_Collection_Objects = new List<UraniumCollectionObject> ();

	// Use this for initialization
	void Start () {
		Total_Uranium = 1000;
		start_Time = Time.time;
		uranium_time = 5.0f;
	}
	
	// Update is called once per frame
	void Update () {

		if (Uranium_Collection_Objects.Count > 0) {
				for (int q = 0; q < Uranium_Collection_Objects.Count; q++) {
						UraniumCollectionObject temp;
						temp.uranium_Time = Uranium_Collection_Objects [q].uranium_Time - Time.deltaTime;
						temp.Uranium_Collection_Object = Uranium_Collection_Objects [q].Uranium_Collection_Object;
						temp.uranium_Accumulated = Uranium_Collection_Objects [q].uranium_Accumulated;
						Uranium_Collection_Objects [q] = temp;
				}
		}

		IncrementUranium ();

		if (ResearchBuildingObjects.Count > 0)
		{
			if (Uranium_Collection_Objects.Count != 0)
			{
				for (int n = 0; n < Uranium_Collection_Objects.Count; n++)
				{
					if (ResearchBuildingObjects[n].Uranium_Collected == false)
					{
						if (Uranium_Collection_Objects[n].Uranium_Collection_Object != null)
						{
							Vector3 tempy = Camera.main.WorldToViewportPoint(ResearchBuildingObjects[n].ResearchBuilding.transform.position);
							tempy.y = Camera.main.WorldToViewportPoint(ResearchBuildingObjects[n].ResearchBuilding.transform.position).y + 0.16f;
							Uranium_Collection_Objects[n].Uranium_Collection_Object.transform.position = tempy;
						}
					}
				}
				if (Input.touchCount > 0)
				{
					for (int n = 0; n < Uranium_Collection_Objects.Count; n++)
					{
						if (ResearchBuildingObjects[n].Uranium_Collected == false)
						{
							if (Uranium_Collection_Objects[n].Uranium_Collection_Object.guiTexture.HitTest(Input.touches[0].position)) 
							{
								Destroy(Uranium_Collection_Objects[n].Uranium_Collection_Object);
								//Uranium_Collection_Objects.RemoveAt(n);
								Total_Uranium += Uranium_Collection_Objects[n].uranium_Accumulated;
								DisplayUraniumIncrementText(Uranium_Collection_Objects[n]);
								ResearchBuildingList TempBuilding;
								TempBuilding.Uranium_Collected = true;
								TempBuilding.ResearchBuilding = ResearchBuildingObjects[n].ResearchBuilding;
								TempBuilding.initialised = ResearchBuildingObjects[n].initialised;
								ResearchBuildingObjects[n] = TempBuilding;
								ParticleSystem[] particle = new ParticleSystem[ResearchBuildingObjects.Count];
								particle[n] = ResearchBuildingObjects[n].ResearchBuilding.GetComponentInChildren<ParticleSystem>();
								particle[n].Play();

								UraniumCollectionObject TempCollectionObject;
								TempCollectionObject = Uranium_Collection_Objects[n];
								TempCollectionObject.uranium_Time = 5.0f;
								Uranium_Collection_Objects[n] = TempCollectionObject;
							}
						}
					}
				}
			}
		}

		// Update all of the resource text displays
		Uranium_Text.text = "Uranium: " + Total_Uranium;

		// Delete Increment text's
		foreach(GameObject Increment in GameObject.FindGameObjectsWithTag("Increment_Text"))
		{
			if (Increment.transform.position.y > 3)
			{
				Destroy(Increment);
			}
		}

	}

	public void UpdateUranium(float uranium)
	{
		uranium_time = 5.0f; // reset the timer
		Uranium_Increment += uranium;
	}

	public void SetUraniumBuildingList(Transform currentBuilding)
	{
		GameObject Building = currentBuilding.gameObject;
		ResearchBuildingList Temp;
		Temp.ResearchBuilding = Building;
		Temp.initialised = false;
		Temp.Uranium_Collected = false;
		
		ResearchBuildingObjects.Add(Temp);
		
		UraniumCollectionObject Temp2;
		Temp2.uranium_Time = 5.0f;
		Temp2.Uranium_Collection_Object = (GameObject)Instantiate(Uranium_Collection);
		Temp2.uranium_Accumulated = 0;
		Uranium_Collection_Objects.Add(Temp2);
	}

	void DisplayUraniumIncrementText(UraniumCollectionObject CollectionObject)
	{
		GameObject Temp;
		Temp = (GameObject)Instantiate (Uranium_Increment_Text);
		Temp.transform.position = CollectionObject.Uranium_Collection_Object.transform.position;
		Temp.guiText.text = ("+" + CollectionObject.uranium_Accumulated);
		// apply velocity to the text --- float up the screen
		Temp.rigidbody.velocity = new Vector3 (0, 0.5f, 0);
	}

	void IncrementUranium()
	{
		// Update Uranium
		if (Uranium_Collection_Objects.Count > 0)
		{
			for (int q = 0; q < Uranium_Collection_Objects.Count; q++)
			{
				if (Uranium_Collection_Objects[q].uranium_Time <= 0) 
				{	
					UraniumCollectionObject Temp;
					Temp = Uranium_Collection_Objects[q];
					Temp.uranium_Accumulated+= 100;
					Uranium_Collection_Objects[q] = Temp;
					
					for (int n = 0; n < ResearchBuildingObjects.Count; n++)
					{
						if (Uranium_Increment > 0)
						{
							
							if (ResearchBuildingObjects[n].Uranium_Collected == true)
							{
								GameObject TempObject = (GameObject)Instantiate(Uranium_Collection);
								Vector3 tempy = Camera.main.WorldToViewportPoint(ResearchBuildingObjects[n].ResearchBuilding.transform.position);
								tempy.y = Camera.main.WorldToViewportPoint(ResearchBuildingObjects[n].ResearchBuilding.transform.position).y + 0.46f;
								TempObject.transform.position = tempy;
								
								UraniumCollectionObject TempCollector;
								TempCollector.Uranium_Collection_Object = TempObject;
								TempCollector.uranium_Time = 5.0f;
								TempCollector.uranium_Accumulated = 100;
								Uranium_Collection_Objects[n] = TempCollector;
								//Uranium_Collection_Objects.Add(TempCollector);
								ResearchBuildingList TempBuilding;
								TempBuilding.Uranium_Collected = false;
								TempBuilding.ResearchBuilding = ResearchBuildingObjects[n].ResearchBuilding;
								TempBuilding.initialised = ResearchBuildingObjects[n].initialised;
								ResearchBuildingObjects[n] = TempBuilding;
							}								
						}
					}
					
					UraniumCollectionObject temp3;
					temp3.uranium_Time = 5.0f;
					temp3.Uranium_Collection_Object = Uranium_Collection_Objects[q].Uranium_Collection_Object;
					temp3.uranium_Accumulated = Uranium_Collection_Objects[q].uranium_Accumulated;					
					Uranium_Collection_Objects[q] = temp3;		// reset the timer
				} /// End of Update gold
			}
		}
	}

	public void SpendResources(float uranium)
	{
		Total_Uranium -= uranium;
	}
}
