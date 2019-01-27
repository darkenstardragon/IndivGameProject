using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    private HashSet<Collider> colliders;
    private HashSet<string> colliderNames;
    private float damage = 0;

    private void Start()
    {
        colliders = new HashSet<Collider>();
        colliderNames = new HashSet<string>();
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
        
        if (col.tag == "enemy" && !colliderNames.Contains(col.name))
        {
            colliders.Add(col);
            colliderNames.Add(col.name);
            print("name: " + col.name);
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if(col.tag == "enemy")
        {
            print("remove: " + col.name);
            colliderNames.Remove(col.name);
            colliders.Remove(col);
        }
    }

    private void LateUpdate()
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
