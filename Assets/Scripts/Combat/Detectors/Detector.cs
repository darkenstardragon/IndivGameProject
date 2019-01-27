using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    private HashSet<Collider> colliders;
    private float damage = 0;

    private void Start()
    {
        colliders = new HashSet<Collider>();
    }
    /*
    private void OnTriggerStay(Collider col)
    {
        if(col.tag == "enemy" && damage > 0)
        {
            col.SendMessage("TakeDamage", damage);
            damage = 0;
        }
    }
    */
    private void OnTriggerEnter(Collider col)
    {
        if(col.tag == "enemy")
        {
            colliders.Add(col);
            print("added");
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if(col.tag == "enemy")
        {
            colliders.Remove(col);
        }
    }

    private void Update()
    {

        if(damage > 0)
        {
            foreach(Collider col in colliders)
            {
                col.SendMessage("TakeDamage", damage);
            }
            damage = 0;
        }
    }
    public void useSkill(int d)
    {
        print("col count = " + colliders.Count);
        damage = d;
    }
}
