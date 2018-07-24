using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBody : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        transform.parent.GetComponent<Rocket>().ProcessCollision(collision); 
    }

}
