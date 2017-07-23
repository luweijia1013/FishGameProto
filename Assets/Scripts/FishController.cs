using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FishController : MonoBehaviour {

    private bool is_bottle_touched;
    private bool is_bubble_alive_lastframe;
    private GameObject bottle_touched;
	// Use this for initialization
	void Start () {
        is_bottle_touched = false;
        is_bubble_alive_lastframe = false;//last frame of the aliveness(avoid the influence of order of the scripts on this variable)
	}

    // Update is called once per frame
    void Update()
    {
        if (is_bottle_touched)
        {
            if (Input.GetKey("space") && !(is_bubble_alive_lastframe || Input.GetKey("j"))) {
                bottle_touched.SetActive(false);
                bottle_touched = null;
                is_bottle_touched = false;

                System.Random ra = new System.Random();
                this.transform.parent.GetComponent<PlayerController>().oxygen_beinfluenced += ra.Next(-15, 40);
            }
            is_bubble_alive_lastframe = this.transform.parent.GetComponentInChildren<ParticleSystem>().IsAlive();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Bottle")
        {
            Debug.Log("Enter! " + other.gameObject);
            is_bottle_touched = true;
            bottle_touched = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Bottle")
        {
            Debug.Log("Exit! " + other.gameObject);
            if (other.gameObject != bottle_touched)
            {
                Debug.Log("Error! Not the same gameobject trigger");
            }
            else
            {
                is_bottle_touched = false;
                bottle_touched = null;
            }
        }
    }
}
