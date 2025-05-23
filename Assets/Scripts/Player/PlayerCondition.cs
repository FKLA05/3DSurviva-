using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class PlayerCondition : MonoBehaviour 
{
    public UICondition uiCondition;
    
    Condition stamina { get { return uiCondition.stamina; } }

    private void Update()
    {
        stamina.Add(stamina.passiveValue * Time.deltaTime);
    }

    public bool UseStamina(float amount)
    {
        if (stamina.curValue - amount < 0)
        {
            return false;
        }

        stamina.Subtract(amount);
        return true;
    }


}
