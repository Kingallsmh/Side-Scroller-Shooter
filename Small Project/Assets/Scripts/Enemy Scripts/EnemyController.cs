using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : BaseController {

    public void SetInputDirection(int numOfInput, Vector2 setDirection)
    {
        if(numOfInput == 0)
        {
            directionInput = setDirection;
        }
    }

    public void SetButtonInput(int numOfBtn, bool input)
    {
        switch (numOfBtn)
        {
            case 0: btn0 = input; break;
            case 1: btn1 = input; break;
            case 2: btn2 = input; break;
            case 3: btn3 = input; break;
        }
    }

    public override bool GatherButton(int numOfBtn)
    {
        //throw new System.NotImplementedException();
        return true;
    }

    public override void GatherInput()
    {
        //throw new System.NotImplementedException();
    }

    public override Vector2 GetDirectionInput(int numOfInput)
    {
        if (numOfInput == 0)
        {
            return directionInput;
        }
        return Vector2.zero;
    }
}
