using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourceManagementAmmo : MonoBehaviour {
	
	public GUIText Ammo_Text;
	public float Total_Ammo;
	public GameObject Ammo_Collection;
	public GameObject Ammo_Increment_Text;
	private float Ammo_Increment;
	private float start_Time, ammo_time;
	private GameObject Ammo_Collector;
	private bool[] initialised = null;
	
	private struct MunitionsFactoryList
	{
		public GameObject MunitionsFactory;
		public bool Ammo_Collected;
		public bool initialised;
	}
	
	private struct AmmoCollectionObject
	{
		public float ammo_Time;
		public GameObject Ammo_Collection_Object;
		public float ammo_Accumulated;
	}
	
	private List<MunitionsFactoryList> MunitionsFactoryObjects = new List<MunitionsFactoryList> ();
	private List<AmmoCollectionObject> Ammo_Collection_Objects = new List<AmmoCollectionObject> ();
	
	// Use this for initialization
	void Start () {
		Total_Ammo = 1000;
		start_Time = Time.time;
		ammo_time = 5.0f;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Ammo_Collection_Objects.Count > 0) {
			for (int q = 0; q < Ammo_Collection_Objects.Count; q++) {
				AmmoCollectionObject temp;
				temp.ammo_Time = Ammo_Collection_Objects [q].ammo_Time - Time.deltaTime;
				temp.Ammo_Collection_Object = Ammo_Collection_Objects [q].Ammo_Collection_Object;
				temp.ammo_Accumulated = Ammo_Collection_Objects [q].ammo_Accumulated;
				Ammo_Collection_Objects [q] = temp;
			}
		}
		
		IncrementAmmo ();
		
		if (MunitionsFactoryObjects.Count > 0)
		{
			if (Ammo_Collection_Objects.Count != 0)
			{
				for (int n = 0; n < Ammo_Collection_Objects.Count; n++)
				{
					if (MunitionsFactoryObjects[n].Ammo_Collected == false)
					{
						if (Ammo_Collection_Objects[n].Ammo_Collection_Object != null)
						{
							Vector3 tempy = Camera.main.WorldToViewportPoint(MunitionsFactoryObjects[n].MunitionsFactory.transform.position);
							tempy.y = Camera.main.WorldToViewportPoint(MunitionsFactoryObjects[n].MunitionsFactory.transform.position).y + 0.16f;
							Ammo_Collection_Objects[n].Ammo_Collection_Object.transform.position = tempy;
						}
					}
				}
				if (Input.touchCount > 0)
				{
					for (int n = 0; n < Ammo_Collection_Objects.Count; n++)
					{
						if (MunitionsFactoryObjects[n].Ammo_Collected == false)
						{
							if (Ammo_Collection_Objects[n].Ammo_Collection_Object.guiTexture.HitTest(Input.touches[0].position)) 
							{
								Destroy(Ammo_Collection_Objects[n].Ammo_Collection_Object);
								//Ammo_Collection_Objects.RemoveAt(n);
								Total_Ammo += Ammo_Collection_Objects[n].ammo_Accumulated;
								DisplayAmmoIncrementText(Ammo_Collection_Objects[n]);
								MunitionsFactoryList TempBuilding;
								TempBuilding.Ammo_Collected = true;
								TempBuilding.MunitionsFactory = MunitionsFactoryObjects[n].MunitionsFactory;
								TempBuilding.initialised = MunitionsFactoryObjects[n].initialised;
								MunitionsFactoryObjects[n] = TempBuilding;
								ParticleSystem[] particle = new ParticleSystem[MunitionsFactoryObjects.Count];
								particle[n] = MunitionsFactoryObjects[n].MunitionsFactory.GetComponentInChildren<ParticleSystem>();
								particle[n].Play();

								AmmoCollectionObject TempCollectionObject;
								TempCollectionObject = Ammo_Collection_Objects[n];
								TempCollectionObject.ammo_Time = 5.0f;
								Ammo_Collection_Objects[n] = TempCollectionObject;
							}
						}
					}
				}
			}
		}
		
		// Update all of the resource text displays
		Ammo_Text.text = "Ammo: " + Total_Ammo;

		// Delete Increment text's
		foreach(GameObject Increment in GameObject.FindGameObjectsWithTag("Increment_Text"))
		{
			if (Increment.transform.position.y > 3)
			{
				Destroy(Increment);
			}
		}
		
	}
	
	public void UpdateAmmo(float ammo)
	{
		ammo_time = 5.0f; // reset the timer
		Ammo_Increment += ammo;
	}
	
	public void SetAmmoBuildingList(Transform currentBuilding)
	{
		GameObject Building = currentBuilding.gameObject;
		MunitionsFactoryList Temp;
		Temp.MunitionsFactory = Building;
		Temp.initialised = false;
		Temp.Ammo_Collected = false;
		
		MunitionsFactoryObjects.Add(Temp);
		
		AmmoCollectionObject Temp2;
		Temp2.ammo_Time = 5.0f;
		Temp2.Ammo_Collection_Object = (GameObject)Instantiate(Ammo_Collection);
		Temp2.ammo_Accumulated = 0;
		Ammo_Collection_Objects.Add(Temp2);
	}

	void DisplayAmmoIncrementText(AmmoCollectionObject CollectionObject)
	{
		GameObject Temp;
		Temp = (GameObject)Instantiate (Ammo_Increment_Text);
		Temp.transform.position = CollectionObject.Ammo_Collection_Object.transform.position;
		Temp.guiText.text = ("+" + CollectionObject.ammo_Accumulated);
		// apply velocity to the text --- float up the screen
		Temp.rigidbody.velocity = new Vector3 (0, 0.5f, 0);
	}
	
	void IncrementAmmo()
	{
		// Update Ammo
		if (Ammo_Collection_Objects.Count > 0)
		{
			for (int q = 0; q < Ammo_Collection_Objects.Count; q++)
			{
				if (Ammo_Collection_Objects[q].ammo_Time <= 0) 
				{	
					AmmoCollectionObject Temp;
					Temp = Ammo_Collection_Objects[q];
					Temp.ammo_Accumulated+= 100;
					Ammo_Collection_Objects[q] = Temp;
					
					for (int n = 0; n < MunitionsFactoryObjects.Count; n++)
					{
						if (Ammo_Increment > 0)
						{
							
							if (MunitionsFactoryObjects[n].Ammo_Collected == true)
							{
								GameObject TempObject = (GameObject)Instantiate(Ammo_Collection);
								Vector3 tempy = Camera.main.WorldToViewportPoint(MunitionsFactoryObjects[n].MunitionsFactory.transform.position);
								tempy.y = Camera.main.WorldToViewportPoint(MunitionsFactoryObjects[n].MunitionsFactory.transform.position).y + 0.46f;
								TempObject.transform.position = tempy;
								
								AmmoCollectionObject TempCollector;
								TempCollector.Ammo_Collection_Object = TempObject;
								TempCollector.ammo_Time = 5.0f;
								TempCollector.ammo_Accumulated = 100;
								Ammo_Collection_Objects[n] = TempCollector;
								//Ammo_Collection_Objects.Add(TempCollector);
								MunitionsFactoryList TempBuilding;
								TempBuilding.Ammo_Collected = false;
								TempBuilding.MunitionsFactory = MunitionsFactoryObjects[n].MunitionsFactory;
								TempBuilding.initialised = MunitionsFactoryObjects[n].initialised;
								MunitionsFactoryObjects[n] = TempBuilding;
							}								
						}
					}
					
					AmmoCollectionObject temp3;
					temp3.ammo_Time = 5.0f;
					temp3.Ammo_Collection_Object = Ammo_Collection_Objects[q].Ammo_Collection_Object;
					temp3.ammo_Accumulated = Ammo_Collection_Objects[q].ammo_Accumulated;					
					Ammo_Collection_Objects[q] = temp3;		// reset the timer
				} /// End of Update gold
			}
		}
	}
	
	public void SpendResources(float ammo)
	{
		Total_Ammo -= ammo;
	}
}
