using UnityEngine;
using System.Collections;
using System.Collections.Generic;	// Allow for lists
public class CheckForCollisions : MonoBehaviour {

	public List<Collider> colliders = new List<Collider>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider c) {
		if (c.tag == "Building" || c.tag == "Trench") {
			colliders.Add(c);
		}
	}
	
	void OnTriggerExit(Collider c) {
		if (c.tag == "Building" || c.tag == "Trench") {
			colliders.Remove(c);
		}
		
	}


}
