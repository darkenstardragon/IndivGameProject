﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class UserStats : MonoBehaviour
{
    public Skill[] allSkills;
    public Skill[] playerSkills;

    public Texture barsBackgroundTexture;
    public Image[] ChargingBars;

    public GameObject meleeDetector;
    public GameObject AoeDetector;
    // Start is called before the first frame update
    private float ChargeTime = 0.0f;
    private float ChargeTime2 = 0.0f;

    private bool SkillOnline1 = false;
    private bool SkillOnline2 = false;

    public void Start()
    {
        for(int i = 0; i < 2; ++i)
        {
            playerSkills[i].id = allSkills[i].id;
            playerSkills[i].name = allSkills[i].name;
            playerSkills[i].description = allSkills[i].description;
            playerSkills[i].icon = allSkills[i].icon;
            playerSkills[i].cooldown = allSkills[i].cooldown;
            playerSkills[i].currentCooldown = allSkills[i].currentCooldown;
        }
        ResetChargingBars();
    }
    /*
    public void OnGUI()
    {
        Rect rect1 = new Rect(Screen.width / 2 - 200, Screen.height - 64, 32, 32);
        Rect rect2 = new Rect(Screen.width / 2 - 150, Screen.height - 64, 32, 32);

        if (GUI.Button (rect1, "1"))
        {
            activateSkill(playerSkills[0].id);
        }

        if (rect1.Contains(Event.current.mousePosition))
        {
            GUI.DrawTexture(new Rect(Input.mousePosition.x + 20, Screen.height - Input.mousePosition.y - 150, 200, 200), barsBackgroundTexture);
            GUI.Label(new Rect(Input.mousePosition.x + 20, Screen.height - Input.mousePosition.y - 150, 200, 200),
                playerSkills[0].name + "\n" +
                "Description: " + playerSkills[0].description + "\n" +
                "Skill ID: " + playerSkills[0].id);
        }

        if (GUI.Button(rect2, "2"))
        {
            activateSkill(playerSkills[1].id);
        }

        if (rect2.Contains(Event.current.mousePosition))
        {
            GUI.DrawTexture(new Rect(Input.mousePosition.x + 20, Screen.height - Input.mousePosition.y - 150, 200, 200), barsBackgroundTexture);
            GUI.Label(new Rect(Input.mousePosition.x + 20, Screen.height - Input.mousePosition.y - 150, 200, 200),
                playerSkills[1].name + "\n" +
                "Description: " + playerSkills[1].description + "\n" +
                "Skill ID: " + playerSkills[1].id);
        }
    }
    */
    public void Update()
    {
        if (playerSkills[0].currentCooldown == 0)
        {
            if (Input.GetKey("1") && !SkillOnline2)
            {
                SkillOnline1 = true;
                ChargeTime += Time.deltaTime;
                if (ChargeTime < 1)
                {
                    ChargingBars[0].fillAmount = ChargeTime;
                }
                else if (ChargeTime > 1 && ChargeTime < 2)
                {
                    ChargingBars[1].fillAmount = ChargeTime - 1;
                }
                else if (ChargeTime > 2 && ChargeTime < 3)
                {
                    ChargingBars[2].fillAmount = ChargeTime - 2;
                }
                else if (ChargeTime > 3)
                    ChargeTime = 3.0f;
                
            }
            if (Input.GetKeyUp("1") && !SkillOnline2)
            {
                
                ResetChargingBars();
                StartCoroutine(activateSkill(playerSkills[0].id));
            }
        }
        if (playerSkills[1].currentCooldown == 0)
        {
            if (Input.GetKey("2") && !SkillOnline1)
            {
                SkillOnline2 = true;
                ChargeTime2 += Time.deltaTime;
                if (ChargeTime2 < 1)
                {
                    ChargingBars[0].fillAmount = ChargeTime2;
                }
                else if (ChargeTime2 > 1 && ChargeTime2 < 2)
                {
                    ChargingBars[1].fillAmount = ChargeTime2 - 1;
                }
                else if (ChargeTime2 > 2 && ChargeTime2 < 3)
                {
                    ChargingBars[2].fillAmount = ChargeTime2- 2;
                }
                else if (ChargeTime2 > 3)
                    ChargeTime2 = 3.0f;

            }
            if (Input.GetKeyUp("2") && !SkillOnline1)
            {
                
                ResetChargingBars();
                StartCoroutine(activateSkill(playerSkills[1].id));
            }
        }

        for (int i = 0; i < playerSkills.Length; ++i)
        {
            if (playerSkills[i].currentCooldown > 0)
            {
                playerSkills[i].icon.fillAmount = 1 - playerSkills[i].currentCooldown / playerSkills[i].cooldown;
            }
            
        }
    }

    public void FixedUpdate()
    {
        for (int i = 0; i < playerSkills.Length; ++i)
        {
            if (playerSkills[i].currentCooldown > 0)
            {
                playerSkills[i].currentCooldown -= Time.deltaTime;
            }
            else if(playerSkills[i].currentCooldown < 0)
            {
                playerSkills[i].currentCooldown = 0;
            }
        }
    }
    
    private void ResetChargingBars()
    {
        for (int i = 0; i < ChargingBars.Length; ++i)
        {
            ChargingBars[i].fillAmount = 0;
        }
    }

    private IEnumerator activateSkill(int id)
    {
        switch (id)
        {
            case 0:
                int ChargeLevel = (int)ChargeTime + 1;
                meleeDetector.SendMessage("useSkill", 10 * ChargeLevel);
                ChargeTime = 0;
                playerSkills[0].currentCooldown = playerSkills[0].cooldown;
                SkillOnline1 = false;
                yield return new WaitForSeconds(0.5f);
                break;
            case 1:
                ChargeLevel = (int)ChargeTime2 + 1;
                for(int i = 0; i < 3; ++i)
                {
                    AoeDetector.SendMessage("useSkill", 5 * ChargeLevel);
                    yield return new WaitForSeconds(0.25f);
                }
                ChargeTime2 = 0;
                playerSkills[1].currentCooldown = playerSkills[1].cooldown;
                SkillOnline2 = false;
                yield return new WaitForSeconds(0.5f);
                break;
            default:
                print("Skill error");
                break;
        }
    }

}
