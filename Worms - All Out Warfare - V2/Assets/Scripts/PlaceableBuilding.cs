﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;	// Allow for lists

public class PlaceableBuilding : MonoBehaviour {
	
	public List<Collider> colliders = new List<Collider>();
	public bool isSelected;
	public bool buttonsClicked;
	private bool menuCreated;
	private float boxLeft, boxTop, boxWidth, boxHeight;
	public GameObject menus;
	private GameObject currentMenu;
	private double menuWidth;

	void OnGUI() {
		boxLeft = Screen.width - 420;
		boxTop = Screen.height - 140;
		boxWidth = 400;
		boxHeight = 120;	

	}

	// Update is called once per frame
	void Update () {
		menuWidth = 300;
		Vector3 menuPosition = new Vector3 (0.0f, 0.5f, 0.0f);
		if (isSelected) {
			if (!menuCreated) { 
				currentMenu = (GameObject)Instantiate(menus);
				menuCreated = true;
			}
		}
		else if (menuCreated) {
			CloseMenu();
		}
	}

	void OnTriggerEnter(Collider c) {
		if (c.tag == "Building") {
			colliders.Add(c);
		}
	}

	void OnTriggerExit(Collider c) {
		if (c.tag == "Building") {
			colliders.Remove(c);
		}
	}

	public void CloseMenu()
	{		
		Destroy(currentMenu);
		menuCreated = false;
	}

	public void SetMenu(bool m)
	{
		menuCreated = m;
	}

	public void SetSelected(bool s) {
		isSelected = s;
	}
}
