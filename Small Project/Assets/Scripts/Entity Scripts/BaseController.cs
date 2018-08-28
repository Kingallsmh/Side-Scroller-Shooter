using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController : MonoBehaviour {

    protected Vector2 directionInput;
    protected bool btn0, btn1, btn2, btn3;

    public abstract void GatherInput();
    public abstract Vector2 GetDirectionInput(int numOfInput);
    public abstract bool GatherButton(int numOfBtn);
}
