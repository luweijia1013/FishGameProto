using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Joystick event
/// </summary>
public class MobilePlayerController : MonoBehaviour
{
    //Class Variable Member
    public float speed;

    //Class Function Member
    void OnEnable()
    {
        EasyJoystick.On_JoystickMove += On_JoystickMove;
        EasyJoystick.On_JoystickMoveEnd += On_JoystickMoveEnd;
    }

    void OnDisable()
    {
        EasyJoystick.On_JoystickMove -= On_JoystickMove;
        EasyJoystick.On_JoystickMoveEnd -= On_JoystickMoveEnd;
    }

    void OnDestroy()
    {
        EasyJoystick.On_JoystickMove -= On_JoystickMove;
        EasyJoystick.On_JoystickMoveEnd -= On_JoystickMoveEnd;
    }


    void On_JoystickMoveEnd(MovingJoystick move)
    {
        //if (move.joystickName == "Move_Turn_Joystick")
        //{
        //    GetComponent<Animation>().CrossFade("idle");
        //}
    }
    void On_JoystickMove(MovingJoystick move)
    {

        if (move.joystickName == "New joystick")
        {

            if (move.joystickAxis.y != 0 || move.joystickAxis.x != 0)
            {
                this.transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));
            }
            else
            {
                return;
            }

            Vector3 vecAbsDirection;
            double dAbsRadian = 0;
            double dAbsDegree = 0;
            if (move.joystickAxis.x == 0)
            {
                if(move.joystickAxis.y > 0)
                {
                    dAbsRadian = Math.PI / 2;
                }
                else
                {
                    dAbsRadian = -Math.PI / 2;
                }
            }
            else if (move.joystickAxis.x > 0)
            {
                dAbsRadian = Math.Atan((double)move.joystickAxis.y / move.joystickAxis.x);
            }
            else
            {
                dAbsRadian = Math.Atan((double)move.joystickAxis.y / move.joystickAxis.x) + Math.PI;
            }
            dAbsDegree = dAbsRadian * 180 / Math.PI;
            vecAbsDirection = new Vector3(0, (float)-dAbsDegree, 0);
            this.transform.eulerAngles = vecAbsDirection;
        }
    }

 }