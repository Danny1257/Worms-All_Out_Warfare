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

	public void Hello() {
		Debug.Log ("Hello");
	}

	public void OnTriggerEnter(Collider c) {
		if (c.tag == "Building" || c.tag == "Trench") {
			colliders.Add(c);
		}
	}
	
	public void OnTriggerExit(Collider c) {
		if (c.tag == "Building" || c.tag == "Trench") {
			colliders.Remove(c);
		}
		
	}


}
