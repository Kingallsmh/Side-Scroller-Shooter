using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : BaseController {

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
        //throw new System.NotImplementedException();
        return Vector2.zero;
    }
}
