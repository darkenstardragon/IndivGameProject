using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    private HashSet<Collider> colliders;
    private HashSet<string> colliderNames;
    private float damage = 0;
    private bool isVampirisim = false;

    public Transform player;

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
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if(col.tag == "enemy")
        {
            colliderNames.Remove(col.name);
            colliders.Remove(col);
        }
    }

    private void LateUpdate()
    {

        if(damage > 0)
        {
            float totalDamage = 0.0f;
            foreach(Collider col in colliders)
            {
                col.SendMessage("TakeDamage", damage);
                totalDamage += damage;
            }
            damage = 0;
            if (isVampirisim)
            {
                isVampirisim = !isVampirisim;
                player.SendMessage("HealDamage", totalDamage / 2);
                totalDamage = 0.0f;
            }
        }
    }
    public void useSkill(int d)
    {
        damage = d;
    }
    public void LifeSteal(int d)
    {
        damage = d;
        isVampirisim = true;
    }
}
