using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    private GameObject fish;
    private GameObject bubble;
    private Animator fish_animator;
    [Tooltip("The speed of the fish(Reserved for adjusting)")]
    public float forward_speed;
    [Tooltip("The speed of the fish's rotation(Reserved for adjusting)")]
    public float rotate_speed;
    public Canvas[] canvases;
    public GameObject hp_prefab;
    [HideInInspector]
    public float oxygen;
    [HideInInspector]
    public float oxygen_attack;
    [HideInInspector]
    public float oxygen_beinfluenced;

    private MotionBlurEffect[] blurcontroller;
    private bool speed_up;
    private bool is_move;

    // Use this for initialization
    void Start () {
        fish = this.transform.Find("Fish").gameObject;
        bubble = this.transform.Find("Bubble").gameObject;
        fish_animator = this.GetComponentInChildren<Animator>();

        blurcontroller = this.GetComponentsInChildren<MotionBlurEffect>();
        for (var i = 0; i < blurcontroller.Length; i++)
        {
            blurcontroller[i].enabled = false;
        }

        speed_up = false;
        oxygen = 100;
        oxygen_attack = 0;
        oxygen_beinfluenced = 0;
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
            fish_animator.SetBool("is_move", true);
            MoveForward();
        }
        else
        {
            is_move = false;
        }
        if (Input.GetAxis("Horizontal") != 0)
        {
            //The degree of the rotation
            fish_animator.SetBool("is_move", true);
            float hor = Input.GetAxis("Horizontal");
            MoveRotate(hor);
        }
        if (!Input.GetKey("w") && Input.GetAxis("Horizontal") == 0)
        {
            fish_animator.SetBool("is_move", false);
        }
        if (Input.GetKey("j") && is_move)
        { 
            speed_up = true;
            fish_animator.SetFloat("speed_boast", 1.5f);
            SpeedUpBlur();
        }
        else
        {
            speed_up = false;
            fish_animator.SetFloat("speed_boast", 1.0f);
            SpeedDownBlur();
        }

        //oxygen lost by time
        OxygenCal();
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
            if (oxygen > 100)
            {
                oxygen = 100;
            }
            if (oxygen < 20)
            {
                text.color = Color.red;
            }
            else
            {
                text.color = Color.black;
            }
        }
        oxygen_attack = 0;
        //oxygen_beinfluenced = 0;
    }

    /**
    * This function is called when the player attacks.(banned when speed up)
    */
    void PlayBubble(float forward_speed)
    {
        ParticleSystem bubble_particle = bubble.transform.GetComponent<ParticleSystem>();
        bool bubble_is_play = bubble_particle.IsAlive();
        if (!bubble_is_play)
        {
            oxygen_attack -= 5;
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

    void OxygenCal()
    {
        var oxygen_cost = speed_up ? 3 : 1;
        oxygen = oxygen - oxygen_cost * Time.deltaTime;
        if (oxygen_attack != 0)
        {
            oxygen += oxygen_attack;
            GameObject hp = (GameObject)Instantiate(hp_prefab, new Vector3(0, 5, 0) + this.transform.Find("Fish").position, new Quaternion());
            hp.GetComponent<AlwaysFace>().Target = this.transform.Find("Fish").gameObject;
            hp.GetComponentInChildren<TextMesh>().text = (oxygen_attack > 0 ? ("+" + oxygen_attack.ToString()) : oxygen_attack.ToString());
            oxygen_attack = 0;
        }
        if (oxygen_beinfluenced != 0)
        {
            oxygen += oxygen_beinfluenced;
            GameObject hp = (GameObject)Instantiate(hp_prefab, new Vector3(0, 10, 0) + this.transform.Find("Fish").position, new Quaternion());
            hp.GetComponent<AlwaysFace>().Target = this.transform.Find("Fish").gameObject;
            hp.GetComponentInChildren<TextMesh>().text = (oxygen_beinfluenced > 0 ? ("+" + oxygen_beinfluenced.ToString()) : oxygen_beinfluenced.ToString());
            oxygen_beinfluenced = 0;
        }
    }
}
