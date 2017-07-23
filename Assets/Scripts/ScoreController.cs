using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour {

    public PlayerController player;
    [HideInInspector]
    public float start_time;
	// Use this for initialization
	void Start () {
        start_time = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (player.oxygen <= 0)
        {
            this.transform.GetComponent<ScoreController>().enabled = false;
        }
        int score = 0;
        if (start_time == 0)
        {
            start_time = Time.time;
            score = 0;
        }
        else
        {
            score = (int)((Time.time - start_time) * 40);
        }
        this.GetComponent<Text>().text = "Score: " + score;
	}
}
