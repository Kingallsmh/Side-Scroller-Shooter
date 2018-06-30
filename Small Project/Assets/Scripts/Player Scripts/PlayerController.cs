using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    Vector2 directionInput;
    bool btn1, btn2, btn3;

    // Update is called once per frame
    public void GatherInput() {
        directionInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        btn1 = Input.GetButton("Button1");
        btn2 = Input.GetButton("Button2");
        btn3 = Input.GetButton("Button3");
    }

    public Vector2 GetDirectionInput(int numOfInput) {
        if (numOfInput == 0) {
            return directionInput;
        }
        return Vector2.zero;
    }

    public bool GatherButton(int numOfBtn) {
        switch (numOfBtn) {
            case 0:
                //Extra input
                break;
            case 1: return btn1;
            case 2: return btn2;
            case 3: return btn3;
        }
        return false;
    }
    
}


