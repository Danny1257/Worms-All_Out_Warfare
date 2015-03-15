using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourceManagementOil : MonoBehaviour {
	
	public GUIText Oil_Text;
	public float Total_Oil;
	public GameObject Oil_Collection;
	public GameObject Oil_Increment_Text;
	
	private float Oil_Increment;
	private float start_Time, oil_time;
	private GameObject Oil_Collector;
	private bool[] initialised = null;
	
	private struct OilRefineryList
	{
		public GameObject OilRefinery;
		public bool Oil_Collected;
		public bool initialised;
	}
	
	private struct OilCollectionObject
	{
		public float oil_Time;
		public GameObject Oil_Collection_Object;
		public float oil_Accumulated;
	}
	
	private List<OilRefineryList> OilRefineryObjects = new List<OilRefineryList> ();
	private List<OilCollectionObject> Oil_Collection_Objects = new List<OilCollectionObject> ();
	
	// Use this for initialization
	void Start () {
		Total_Oil = 1000;
		start_Time = Time.time;
		oil_time = 5.0f;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Oil_Collection_Objects.Count > 0) {
			for (int q = 0; q < Oil_Collection_Objects.Count; q++) {
				OilCollectionObject temp;
				temp.oil_Time = Oil_Collection_Objects [q].oil_Time - Time.deltaTime;
				temp.Oil_Collection_Object = Oil_Collection_Objects [q].Oil_Collection_Object;
				temp.oil_Accumulated = Oil_Collection_Objects [q].oil_Accumulated;
				Oil_Collection_Objects [q] = temp;
			}
		}
		
		IncrementOil ();
		
		if (OilRefineryObjects.Count > 0)
		{
			if (Oil_Collection_Objects.Count != 0)
			{
				for (int n = 0; n < Oil_Collection_Objects.Count; n++)
				{
					if (OilRefineryObjects[n].Oil_Collected == false)
					{
						if (Oil_Collection_Objects[n].Oil_Collection_Object != null)
						{
							Vector3 tempy = Camera.main.WorldToViewportPoint(OilRefineryObjects[n].OilRefinery.transform.position);
							tempy.y = Camera.main.WorldToViewportPoint(OilRefineryObjects[n].OilRefinery.transform.position).y + 0.16f;
							Oil_Collection_Objects[n].Oil_Collection_Object.transform.position = tempy;
						}
					}
				}
				if (Input.touchCount > 0)
				{
					for (int n = 0; n < Oil_Collection_Objects.Count; n++)
					{
						if (OilRefineryObjects[n].Oil_Collected == false)
						{
							if (Oil_Collection_Objects[n].Oil_Collection_Object.guiTexture.HitTest(Input.touches[0].position)) 
							{
								Destroy(Oil_Collection_Objects[n].Oil_Collection_Object);
								//Oil_Collection_Objects.RemoveAt(n);
								Total_Oil += Oil_Collection_Objects[n].oil_Accumulated;
								DisplayOilIncrementText(Oil_Collection_Objects[n]);
								OilRefineryList TempBuilding;
								TempBuilding.Oil_Collected = true;
								TempBuilding.OilRefinery = OilRefineryObjects[n].OilRefinery;
								TempBuilding.initialised = OilRefineryObjects[n].initialised;
								OilRefineryObjects[n] = TempBuilding;
								ParticleSystem[] particle = new ParticleSystem[OilRefineryObjects.Count];
								particle[n] = OilRefineryObjects[n].OilRefinery.GetComponentInChildren<ParticleSystem>();
								particle[n].Play();

								OilCollectionObject TempCollectionObject;
								TempCollectionObject = Oil_Collection_Objects[n];
								TempCollectionObject.oil_Time = 5.0f;
								Oil_Collection_Objects[n] = TempCollectionObject;

							}
						}
					}
				}
			}
		}
		
		// Update all of the resource text displays
		Oil_Text.text = "Oil: " + Total_Oil;

		// Delete Increment text's
		foreach(GameObject Increment in GameObject.FindGameObjectsWithTag("Increment_Text"))
		{
			if (Increment.transform.position.y > 3)
			{
				Destroy(Increment);
			}
		}
		
	}
	
	public void UpdateOil(float oil)
	{
		oil_time = 5.0f; // reset the timer
		Oil_Increment += oil;
	}
	
	public void SetOilBuildingList(Transform currentBuilding)
	{
		GameObject Building = currentBuilding.gameObject;
		OilRefineryList Temp;
		Temp.OilRefinery = Building;
		Temp.initialised = false;
		Temp.Oil_Collected = false;
		
		OilRefineryObjects.Add(Temp);
		
		OilCollectionObject Temp2;
		Temp2.oil_Time = 5.0f;
		Temp2.Oil_Collection_Object = (GameObject)Instantiate(Oil_Collection);
		Temp2.oil_Accumulated = 0;
		Oil_Collection_Objects.Add(Temp2);
	}

	void DisplayOilIncrementText(OilCollectionObject CollectionObject)
	{
		GameObject Temp;
		Temp = (GameObject)Instantiate (Oil_Increment_Text);
		Temp.transform.position = CollectionObject.Oil_Collection_Object.transform.position;
		Temp.guiText.text = ("+" + CollectionObject.oil_Accumulated);
		// apply velocity to the text --- float up the screen
		Temp.rigidbody.velocity = new Vector3 (0, 0.5f, 0);
	}
	
	void IncrementOil()
	{
		// Update Oil
		if (Oil_Collection_Objects.Count > 0)
		{
			for (int q = 0; q < Oil_Collection_Objects.Count; q++)
			{
				if (Oil_Collection_Objects[q].oil_Time <= 0) 
				{	
					OilCollectionObject Temp;
					Temp = Oil_Collection_Objects[q];
					Temp.oil_Accumulated+= 100;
					Oil_Collection_Objects[q] = Temp;
					
					for (int n = 0; n < OilRefineryObjects.Count; n++)
					{
						if (Oil_Increment > 0)
						{
							
							if (OilRefineryObjects[n].Oil_Collected == true)
							{
								GameObject TempObject = (GameObject)Instantiate(Oil_Collection);
								Vector3 tempy = Camera.main.WorldToViewportPoint(OilRefineryObjects[n].OilRefinery.transform.position);
								tempy.y = Camera.main.WorldToViewportPoint(OilRefineryObjects[n].OilRefinery.transform.position).y + 0.46f;
								TempObject.transform.position = tempy;
								
								OilCollectionObject TempCollector;
								TempCollector.Oil_Collection_Object = TempObject;
								TempCollector.oil_Time = 5.0f;
								TempCollector.oil_Accumulated = 100;
								Oil_Collection_Objects[n] = TempCollector;
								//Oil_Collection_Objects.Add(TempCollector);
								OilRefineryList TempBuilding;
								TempBuilding.Oil_Collected = false;
								TempBuilding.OilRefinery = OilRefineryObjects[n].OilRefinery;
								TempBuilding.initialised = OilRefineryObjects[n].initialised;
								OilRefineryObjects[n] = TempBuilding;
							}								
						}
					}
					
					OilCollectionObject temp3;
					temp3.oil_Time = 5.0f;
					temp3.Oil_Collection_Object = Oil_Collection_Objects[q].Oil_Collection_Object;
					temp3.oil_Accumulated = Oil_Collection_Objects[q].oil_Accumulated;					
					Oil_Collection_Objects[q] = temp3;		// reset the timer
				} /// End of Update gold
			}
		}
	}
	
	public void SpendResources(float oil)
	{
		Total_Oil -= oil;
	}
}
