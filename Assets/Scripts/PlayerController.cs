using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    private GameObject fish;
    private GameObject bubble;
    [Tooltip("The speed of the fish(Reserved for adjusting)")]
    public float forward_speed;
    [Tooltip("The speed of the fish's rotation(Reserved for adjusting)")]
    public float rotate_speed;
    public Canvas[] canvases;

    private MotionBlurEffect[] blurcontroller;
    private bool speed_up;
    private bool is_move;
    private float oxygen;

    // Use this for initialization
    void Start () {
        fish = this.transform.Find("Fish").gameObject;
        bubble = this.transform.Find("Bubble").gameObject;

        blurcontroller = this.GetComponentsInChildren<MotionBlurEffect>();
        for (var i = 0; i < blurcontroller.Length; i++)
        {
            blurcontroller[i].enabled = false;
        }

        speed_up = false;
        oxygen = 100;
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown("space"))
        {
            if (speed_up)
            {
                return;
            }
            PlayBubble(forward_speed);
        }
        if (Input.GetKey("w"))
        {
            is_move = true;
            MoveForward();
        }
        else
        {
            is_move = false;
        }
        if (Input.GetAxis("Horizontal") != 0)
        {
            //The degree of the rotation
            float hor = Input.GetAxis("Horizontal");
            MoveRotate(hor);
        }
        if (Input.GetKey("j") && is_move)
        { 
            speed_up = true;
            SpeedUpBlur();
        }
        else
        {
            speed_up = false;
            SpeedDownBlur();
        }

        //oxygen lost by time
        OxygenCost();
    }

    void LateUpdate()
    {
        for(var i = 0; i < canvases.Length; i++)
        {
            int oxygen_int = (int)oxygen;
            Text text = canvases[i].GetComponentInChildren<Text>();
            text.text = "";
            text.text = "Oxygen: " + oxygen_int.ToString();
            if (oxygen <= 0)
            {
                oxygen = 0;
                text.text = "No Oxygen!";
                this.transform.GetComponent<PlayerController>().enabled = false;
            }
            if (oxygen < 20)
            {
                text.color = Color.red;
            }
        }
    }

    /**
    * This function is called when the player attacks.(banned when speed up)
    */
    void PlayBubble(float forward_speed)
    {
        ParticleSystem bubble_particle = bubble.transform.GetComponent<ParticleSystem>();
        bool bubble_is_play = bubble_particle.IsAlive();
        //Debug.Log(bubble_is_play + "" + Time.time);
        if (!bubble_is_play)
        {
            oxygen -= 5;
            bubble_particle.Play();
        }
    }

    void MoveForward()
    {
        //whether speed is boosted.
        var speed = speed_up ? forward_speed * 2 : forward_speed;
        this.transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    void MoveRotate(float degree)
    {
        transform.Rotate(0, degree * Time.deltaTime * rotate_speed, 0);
    }

    void SpeedUpBlur()
    {
        for (var i = 0; i < blurcontroller.Length; i++)
        {
            blurcontroller[i].enabled = true;
            if (blurcontroller[i].Intensity < 0.1)
            {
                blurcontroller[i].Intensity += (Time.deltaTime * 0.2f);
            }
        }
       

    }

    void SpeedDownBlur()
    {
        for (var i = 0; i < blurcontroller.Length; i++)
        {
            if (blurcontroller[i].Intensity > 0)
            {
                blurcontroller[i].Intensity -= (Time.deltaTime * 0.2f);
            }
            else {
                blurcontroller[i].Intensity = 0;
                blurcontroller[i].enabled = false;
            }
        }
    }

    void OxygenCost()
    {
        var oxygen_cost = speed_up ? 2 : 1;
        oxygen = oxygen - oxygen_cost * Time.deltaTime;
    }
}
