using System.Collections;
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
    private float ChargeTime3 = 0.0f;

    private bool SkillOnline1 = false;
    private bool SkillOnline2 = false;
    private bool SkillOnline3 = false;

    public void Start()
    {
        for(int i = 0; i < 4; ++i)
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
    
    public void Update()
    {
        //Thunder Strike
        if (playerSkills[0].currentCooldown == 0)
        {
            if (Input.GetKey("1") && !SkillOnline2 && !SkillOnline3)
            {
                SkillOnline1 = true;
                ChargeTime += Time.deltaTime;
                if (ChargeTime < 1)
                {
                    ChargingBars[0].fillAmount = Mathf.Clamp(ChargeTime, 0, 1.0f);
                }
                else if (ChargeTime > 1 && ChargeTime < 2)
                {
                    ChargingBars[1].fillAmount = Mathf.Clamp(ChargeTime, 0, 2.0f) - 1;
                }
                else if (ChargeTime > 2 && ChargeTime < 3)
                {
                    ChargingBars[2].fillAmount = Mathf.Clamp(ChargeTime, 0, 3.0f) - 2;
                }
                else if (ChargeTime > 3)
                    ChargeTime = 3.0f;
                
            }
            if (Input.GetKeyUp("1") && !SkillOnline2 && !SkillOnline3)
            {
                ResetChargingBars();
                StartCoroutine(activateSkill(playerSkills[0].id));
            }
        }
        //Cyclone Axe
        if (playerSkills[1].currentCooldown == 0)
        {
            if (Input.GetKey("2") && !SkillOnline1 && !SkillOnline3)
            {
                SkillOnline2 = true;
                ChargeTime2 += Time.deltaTime;
                if (ChargeTime2 < 1)
                {
                    ChargingBars[0].fillAmount = Mathf.Clamp(ChargeTime2, 0, 1.0f);
                }
                else if (ChargeTime2 > 1 && ChargeTime2 < 2)
                {
                    ChargingBars[1].fillAmount = Mathf.Clamp(ChargeTime2, 0, 2.0f) - 1;
                }
                else if (ChargeTime2 > 2 && ChargeTime2 < 3)
                {
                    ChargingBars[2].fillAmount = Mathf.Clamp(ChargeTime2, 0, 3.0f) - 2;
                }
                else if (ChargeTime2 > 3)
                    ChargeTime2 = 3.0f;

            }
            if (Input.GetKeyUp("2") && !SkillOnline1 && !SkillOnline3)
            {
                ResetChargingBars();
                StartCoroutine(activateSkill(playerSkills[1].id));
            }
        }
        //Vampiric Blow
        if (playerSkills[2].currentCooldown == 0)
        {
            if (Input.GetKey("3") && !SkillOnline1 && !SkillOnline2)
            {
                SkillOnline3 = true;
                ChargeTime3 += Time.deltaTime;
                if (ChargeTime3 < 1)
                {
                    ChargingBars[0].fillAmount = Mathf.Clamp(ChargeTime3, 0, 1.0f);
                }
                else if (ChargeTime3 > 1 && ChargeTime3 < 2)
                {
                    ChargingBars[1].fillAmount = Mathf.Clamp(ChargeTime3, 0, 2.0f) - 1;
                }
                else if (ChargeTime3 > 2 && ChargeTime3 < 3)
                {
                    ChargingBars[2].fillAmount = Mathf.Clamp(ChargeTime3, 0, 3.0f) - 2;
                }
                else if (ChargeTime3 > 3)
                    ChargeTime3 = 3.0f;

            }
            if (Input.GetKeyUp("3") && !SkillOnline1 && !SkillOnline2)
            {
                ResetChargingBars();
                StartCoroutine(activateSkill(playerSkills[2].id));
            }
        }
        UpdateCooldown();
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
    

    private void UpdateCooldown()
    {
        for (int i = 0; i < playerSkills.Length; ++i)
        {
            if (playerSkills[i].currentCooldown > 0)
            {
                playerSkills[i].icon.fillAmount = 1 - playerSkills[i].currentCooldown / playerSkills[i].cooldown;
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
            case 2:
                ChargeLevel = (int)ChargeTime3 + 1;
                meleeDetector.SendMessage("LifeSteal", 8 * ChargeLevel);
                ChargeTime3 = 0;
                print("test");
                playerSkills[2].currentCooldown = playerSkills[2].cooldown;
                SkillOnline3 = false;
                yield return new WaitForSeconds(0.5f);
                break;
            default:
                print("Skill error");
                break;
        }
    }

}
