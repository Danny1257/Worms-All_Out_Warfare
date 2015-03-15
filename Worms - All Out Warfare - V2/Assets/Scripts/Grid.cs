﻿using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour {

	public LayerMask GroundMask;
	public Material GroundMaterial;
	public Material GroundAccepted;
	public Material GroundNotAccepted;
	public GameObject plane;
	public int width = 20;
	public int height = 20;
	private GameObject [,] grid = new GameObject[20,20];

	void Awake() 
	{
		for (int x = 0; x < width; x++) 
		{
			for (int z = 0; z < height; z++)
			{
				GameObject gridPlane = (GameObject)Instantiate(plane);
				gridPlane.transform.position = new Vector3(gridPlane.transform.position.x + (x * plane.transform.localScale.x),
				                                           gridPlane.transform.position.y,
				                                           gridPlane.transform.position.z + (z * plane.transform.localScale.z));

				grid[x,z] = gridPlane; 
			}
		}
	}

	void OnGUI()
	{

	}
	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.touchCount > 0)
		{
			for (int x = 0; x < width; x++)
			{
				for (int z = 0; z < height; z++)
				{
					RaycastHit hit = new RaycastHit();
					//Ray ray = new Ray(new Vector3(p.x, Camera.main.transform.position.y, p.z), n);
					Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
					if (Physics.Raycast(ray,out hit, Mathf.Infinity, GroundMask))	// has hit the plane
					{
						grid[x, z].renderer.material = GroundMaterial;
						hit.collider.gameObject.renderer.material = GroundAccepted;
					}


				}
			}
		}	
	}
}