﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar2 : MonoBehaviour
{
    public GameObject FloatingTextPrefab;
    public Image currentHealthBar;
    public Text ratioText;

    private float hitpoint = 150;
    private float maxHitpoint = 150;

    private bool isDead = false;
    private bool isParry = false;
    private bool isDodging = false;

    private void Start()
    {
        UpdateHealthbar();

    }

    private void UpdateHealthbar()
    {
        float ratio = hitpoint / maxHitpoint;
        currentHealthBar.rectTransform.localScale = new Vector3(ratio, 1, 1);
        //ratioText.text = (ratio * 100).ToString("0") + '%';
        ratioText.text = hitpoint.ToString();
    }

    private void TakeDamage(float damage)
    {
        if (!isParry && !isDodging)
        {
            hitpoint -= damage;
        }
        else if (isParry)
        {
            ShowFloatingText("Blocked!");
        }
        else if (isDodging)
        {
            ShowFloatingText("Dodged!");
        }
        if(hitpoint <= 0)
        {
            hitpoint = 0;
            isDead = true;
        }
        UpdateHealthbar();
        if(FloatingTextPrefab != null && transform.tag != "Player")
        {
            ShowFloatingText(damage);
        }
    }

    private void ShowFloatingText(float damage)
    {
        var x = Instantiate(FloatingTextPrefab, transform.position, Quaternion.identity, transform);
        x.GetComponent<TextMesh>().text = damage.ToString();
        x.GetComponent<Transform>().LookAt(2 * transform.position - Camera.main.transform.position);
    }

    private void ShowFloatingText(string s)
    {
        var x = Instantiate(FloatingTextPrefab, transform.position, Quaternion.identity, transform);
        x.GetComponent<TextMesh>().text = s;
        x.GetComponent<Transform>().LookAt(2 * transform.position - Camera.main.transform.position);
    }

    private void HealDamage(float heal)
    {
        hitpoint += heal;
        if(hitpoint > maxHitpoint)
        {
            hitpoint = maxHitpoint;
        }
        UpdateHealthbar();
    }

    private void LateUpdate()
    {
        if (isDead)
        {
            ratioText.text = "DEAD";
            if(transform.tag == "enemy")
            {
                transform.SendMessage("SetDead");
                Destroy(gameObject, 5);
            }
        }
    }

    private void SetParrying(bool b)
    {
        isParry = b;
    }

    private void SetDodging(bool b)
    {
        isDodging = b;
    }
}
