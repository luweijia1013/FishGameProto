using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private GameObject fish;
    private GameObject bubble;

	// Use this for initialization
	void Start () {
        fish = this.transform.Find("Fish").gameObject;
        bubble = this.transform.Find("Bubble").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("space"))
        {
            ParticleSystem bubble_particle = bubble.transform.GetComponent<ParticleSystem>();
            bool bubble_is_play = bubble_particle.IsAlive();
            Debug.Log(bubble_is_play+""+Time.time);
            if (!bubble_is_play)
            {
                bubble_particle.Play();
            }
        }
	}
}
