using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    public float damage = 10;
    public bool activate;
    private void OnTriggerStay(Collider col)
    {
        if(activate && col.tag == "enemy")
        {
            col.SendMessage("TakeDamage", damage);
            activate = !activate;
        }
    }

    public void useSkill(int d)
    {
        damage = d;
        activate = true;
    }
}
