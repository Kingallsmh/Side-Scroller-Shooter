using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipStats : MonoBehaviour {

    public int Hp = 1;
    public int MaxHp = 1;
    public int Atk = 1;    

    private void Start()
    {
        Hp = MaxHp;    
    }

    public void AdjustHealth(int amount)
    {
        Hp += amount;
        Mathf.Clamp(Hp, 0, MaxHp);
    }

    public bool IsDead()
    {
        return Hp == 0;
    }
}
