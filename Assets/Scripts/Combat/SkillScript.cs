using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class SkillScript : MonoBehaviour
{

    private Animator anim;
    private CharacterMovement characterMovement;

    public const float RUSHING_TIME = 1.5f;
    public const float DODGING_TIME = 0.5f;

    public Skill[] allSkills;
    public Skill[] playerSkills;

    public Texture barsBackgroundTexture;
    public Image[] ChargingBars;
    public Image ChargingBarBackground;
    public Image ComboBar;

    public GameObject meleeDetector;
    public GameObject AoeDetector;

    public Image DodgingBar;

    //public GameObject AoEPrefab;
    //public GameObject ThunderStrikePrefab;
    // Start is called before the first frame update

    private float ChargeTime = 0.0f;
    private float ChargeTime2 = 0.0f;
    private float ChargeTime3 = 0.0f;

    private bool SkillOnline1 = false;
    private bool SkillOnline2 = false;
    private bool SkillOnline3 = false;

    private bool SkillOnline = false;
    private bool autoAttackOnline = false;

    private bool onGround = true;

    public const float DodgingCooldown = 1;
    public float CurrentDodgingCooldown;

    private bool isMoving = false;

    private float timeSinceLastAttack = 0.0f;
    private int attackingStep = 0;
    private const float timeToInterruptAttackChain = 1.0f;

    private int currentCombo = 0;
    private const int maxCombo = 20;
    private float currentComboTime = 0.0f;
    private const float comboResetTime = 5.0f;
    private float currentComboFillAmount = 0.0f;
    private const float comboFillingSpeed = 0.005f;
    private const float errorCorrectionFloat = 0.01f;

    private bool readyToCounter = false;
    private float currentCounterAttackTime = 0.0f;
    private const float COUNTER_ATTACK_TIME = 2.0f;

    public void Start()
    {
        characterMovement = FindObjectOfType<CharacterMovement>();
        anim = GetComponentInChildren<Animator>();
        CurrentDodgingCooldown = DodgingCooldown;
        for(int i = 0; i < 9; ++i)
        {
            playerSkills[i].id = allSkills[i].id;
            playerSkills[i].name = allSkills[i].name;
            playerSkills[i].description = allSkills[i].description;
            playerSkills[i].icon = allSkills[i].icon;
            playerSkills[i].cooldown = allSkills[i].cooldown;
            playerSkills[i].currentCooldown = allSkills[i].currentCooldown;
        }
        SetChargingBarVisibility(false);
        ResetChargingBars();
        ComboBar.fillAmount = 0.0f;
        /*
         * 0 - Thunder Strike
         * 1 - Cyclone
         * 2 - AA chain #1
         * 3 - Rush
         * 4 - AA chain #2
         * 5 - AA chain #3
         * 6 - Parry
         * 7 - Combo Release Jump Attack
         * 8 - Counter Attack from Parry (#6)
        */
    }
    
    public void Update()
    {
        if(onGround)
            ProcessSkills();
        UpdateCooldown();
        SetChargingBarVisibility(IsCharging());


        timeSinceLastAttack += Time.deltaTime;
        currentComboTime += Time.deltaTime;
        //print(attackingStep);
        if(timeSinceLastAttack > timeToInterruptAttackChain)
        {
            anim.SetTrigger("StopAA");
            attackingStep = 0;
            timeSinceLastAttack = 0.0f;
        }
        //print(readyToCounter);
        if (readyToCounter)
        {
            currentCounterAttackTime += Time.deltaTime;
            if(currentCounterAttackTime > COUNTER_ATTACK_TIME)
            {
                readyToCounter = false;
                currentCounterAttackTime = 0.0f;
            }
        }

        ProcessComboBarSystem();
    }

    public void LateUpdate()
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

        if (CurrentDodgingCooldown > 0)
            CurrentDodgingCooldown -= Time.deltaTime;
        else if (CurrentDodgingCooldown < 0)
            CurrentDodgingCooldown = 0;

        DodgingBar.fillAmount = 1 - CurrentDodgingCooldown / DodgingCooldown;
    }
    
    private void ProcessSkills()
    {
        //Dodging
        if (CurrentDodgingCooldown == 0)
        {
            if (Input.GetKey(KeyCode.LeftShift) && !SkillOnline1 && !SkillOnline2 && !SkillOnline3 && !SkillOnline)
            {
                print("1");
                SkillOnline = true;
                CurrentDodgingCooldown = DodgingCooldown;
                StartCoroutine(ActivateSkill(99));
            }
        }

        //Thunder Strike
        if (playerSkills[0].currentCooldown == 0)
        {
            if (Input.GetKey("1") && !SkillOnline2 && !SkillOnline3 && !SkillOnline)
            {
                SkillOnline1 = true;
                characterMovement.SetCharging(true);
                anim.SetBool("isCharging", true);
                if (isMoving)
                {
                    anim.SetBool("isMovingWhileCharging", true);
                    //print("trueee");
                }
                else
                {
                    anim.SetBool("isMovingWhileCharging", false);
                    //print("falseee");
                }
                
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
            if (Input.GetKeyUp("1") && !SkillOnline2 && !SkillOnline3 && !SkillOnline)
            {
                ResetChargingBars();
                StartCoroutine(ActivateSkill(playerSkills[0].id));
            }
        }
        //Cyclone Axe
        if (playerSkills[1].currentCooldown == 0)
        {
            if (Input.GetKey("2") && !SkillOnline1 && !SkillOnline3 && !SkillOnline)
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
            if (Input.GetKeyUp("2") && !SkillOnline1 && !SkillOnline3 && !SkillOnline)
            {
                ResetChargingBars();
                StartCoroutine(ActivateSkill(playerSkills[1].id));
            }
        }
        //Vampiric Blow
        /*
        if (playerSkills[2].currentCooldown == 0)
        {
            if (Input.GetKey("3") && !SkillOnline1 && !SkillOnline2 && !SkillOnline)
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
            if (Input.GetKeyUp("3") && !SkillOnline1 && !SkillOnline2 && !SkillOnline)
            {
                ResetChargingBars();
                StartCoroutine(ActivateSkill(playerSkills[2].id));
            }
        }*/

        // Auto Attack
        if (Input.GetMouseButtonDown(0) && OtherSkillsAreNotOnline())
        {
            if (!readyToCounter)
            {
                autoAttackOnline = true;
                if (attackingStep == 0)
                {
                    anim.SetTrigger("attack");
                    timeSinceLastAttack = 0.0f;
                    SkillOnline = true;
                    StartCoroutine(ActivateSkill(playerSkills[2].id));
                }
                else if (attackingStep == 1)
                {
                    //print("222222222222");
                    anim.SetTrigger("attack2");
                    timeSinceLastAttack = 0.0f;
                    SkillOnline = true;
                    StartCoroutine(ActivateSkill(playerSkills[4].id));
                }
                else if (attackingStep == 2)
                {
                    //print("33333333333");
                    anim.SetTrigger("attack3");
                    timeSinceLastAttack = 0.0f;
                    SkillOnline = true;
                    StartCoroutine(ActivateSkill(playerSkills[5].id));
                }
            }
            else
            {
                //print("inn");
                anim.SetTrigger("attack3");
                SkillOnline = true;
                StartCoroutine(ActivateSkill(playerSkills[8].id));
            }
        }
        

        if (playerSkills[3].currentCooldown == 0)
        {
            
            if (Input.GetKeyDown("4") && OtherSkillsAreNotOnline())
            {
                SkillOnline = true;
                StartCoroutine(ActivateSkill(playerSkills[3].id));
            }
        }

        if (playerSkills[6].currentCooldown == 0)
        {
            if (Input.GetKeyDown(KeyCode.E) && OtherSkillsAreNotOnline())
            {
                anim.SetBool("isParrying", true);
                SkillOnline = true;
                StartCoroutine(ActivateSkill(playerSkills[6].id));
            }
        }

        if (currentCombo == maxCombo)
        {
            if (Input.GetKeyDown(KeyCode.Q) && OtherSkillsAreNotOnline())
            {
                anim.SetBool("ReleaseCombo", true);
                SkillOnline = true;
                StartCoroutine(ActivateSkill(playerSkills[7].id));
            }
        }

        if(readyToCounter && Input.GetMouseButtonDown(0))
        {
            transform.SendMessage("SetParrying", false);
            characterMovement.SetMovementDisable(false);
            anim.SetBool("isParrying", false);
            playerSkills[6].currentCooldown = playerSkills[6].cooldown;
            SkillOnline = false;

            //print("inn");
            anim.SetTrigger("attack3");
            SkillOnline = true;
            StartCoroutine(ActivateSkill(playerSkills[8].id));
        }
    }

    private bool OtherSkillsAreNotOnline()
    {
        return !SkillOnline1 && !SkillOnline2 && !SkillOnline3 && !SkillOnline;
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


    private IEnumerator ActivateSkill(int id)
    {
        switch (id)
        {
            case 0:
                int ChargeLevel = (int)ChargeTime + 1;
                meleeDetector.SendMessage("DealDamage", 10 * ChargeLevel);
                anim.SetBool("isMovingWhileCharging", false);
                anim.SetBool("isCharging", false);
                characterMovement.SetCharging(false);
                ChargeTime = 0;
                playerSkills[0].currentCooldown = playerSkills[0].cooldown;
                characterMovement.SetMovementDisable(true);
                yield return new WaitForSeconds(0.5f);
                characterMovement.SetMovementDisable(false);
                SkillOnline1 = false;
                
                break;
            case 1:
                ChargeLevel = (int)ChargeTime2 + 1;
                for(int i = 0; i < 3; ++i)
                {
                    AoeDetector.SendMessage("DealDamage", 5 * ChargeLevel);
                    yield return new WaitForSeconds(0.25f);
                }
                ChargeTime2 = 0;
                playerSkills[1].currentCooldown = playerSkills[1].cooldown;
                yield return new WaitForSeconds(0.5f);
                SkillOnline2 = false;
                break;
            case 2:
                /* :: Old Vampiric Blow Skill
                ChargeLevel = (int)ChargeTime3 + 1;
                meleeDetector.SendMessage("LifeSteal", 8 * ChargeLevel);
                ChargeTime3 = 0;
                playerSkills[2].currentCooldown = playerSkills[2].cooldown;
                yield return new WaitForSeconds(0.5f);
                SkillOnline3 = false;
                */

                // AUTO ATTACK CHAIN 1
                meleeDetector.SendMessage("KnockBack", 5);
                playerSkills[2].currentCooldown = playerSkills[2].cooldown;
                characterMovement.SetMovementDisable(true);
                //transform.SendMessage("AfterCastMoving", 0.05f);
                yield return new WaitForSeconds(0.3f);
                characterMovement.SetMovementDisable(false);


                // yield return new WaitForSeconds(0.5f);
                ++attackingStep;
                SkillOnline = false;
                autoAttackOnline = false;
                anim.SetTrigger("StopAA");
                break;
            case 3: // RUSH
                characterMovement.Rush();
                yield return new WaitForSeconds(0.5f + RUSHING_TIME);
                playerSkills[3].currentCooldown = playerSkills[3].cooldown;
                SkillOnline = false;
                break;

            case 4: // AUTO ATTACK CHAIN 2
                meleeDetector.SendMessage("KnockBack", 10);
                playerSkills[4].currentCooldown = playerSkills[4].cooldown;
                characterMovement.SetMovementDisable(true);
                //transform.SendMessage("AfterCastMoving", 0.05f);
                yield return new WaitForSeconds(0.3f);
                characterMovement.SetMovementDisable(false);
                //yield return new WaitForSeconds(0.5f);
                ++attackingStep;
                SkillOnline = false;
                autoAttackOnline = false;
                anim.SetTrigger("StopAA");
                break;

            case 5: // AUTO ATTACK CHAIN 3
                characterMovement.SetMovementDisable(true);
                yield return new WaitForSeconds(0.2f);
                AoeDetector.SendMessage("KnockBack", 15);
                playerSkills[5].currentCooldown = playerSkills[5].cooldown;

                characterMovement.AfterCastMoving(0.05f);
                
                yield return new WaitForSeconds(0.3f);
                characterMovement.SetMovementDisable(false);
                yield return new WaitForSeconds(0.3f);
                
                SkillOnline = false;
                autoAttackOnline = false;
                anim.SetTrigger("StopAA");
                attackingStep = 0;
                break;

            case 6: // PARRY
                characterMovement.SetMovementDisable(true);
                transform.SendMessage("SetParrying", true);
                yield return new WaitForSeconds(1.0f);
                transform.SendMessage("SetParrying", false);
                characterMovement.SetMovementDisable(false);
                anim.SetBool("isParrying", false);
                playerSkills[6].currentCooldown = playerSkills[6].cooldown;
                SkillOnline = false;
                break;

            case 7: // ULTIMATE
                characterMovement.SetMovementDisable(true);
                float[] timeAndSpeed = {1.0f, 3.0f};
                characterMovement.AfterCastMoving(timeAndSpeed);
                yield return new WaitForSeconds(1.0f);
                for (int i = 0; i < 3; i++)
                {
                    AoeDetector.SendMessage("KnockBack", 10);
                    yield return new WaitForSeconds(0.1f);
                }

                characterMovement.SetMovementDisable(false);
                playerSkills[7].currentCooldown = playerSkills[7].cooldown;
                SkillOnline = false;
                anim.SetBool("ReleaseCombo", false);
                currentCombo = 0;
                break;

            case 8: // COUNTER ATTACK
                readyToCounter = false;
                transform.SendMessage("SetDodging", true);
                characterMovement.SetMovementDisable(true);
                float[] timeAndSpeed2 = { 1.0f, 3.0f };
                characterMovement.AfterCastMoving(timeAndSpeed2);
                yield return new WaitForSeconds(0.6f);
                for (int i = 0; i < 3; i++)
                {
                    AoeDetector.SendMessage("KnockBack", 10);
                    yield return new WaitForSeconds(0.1f);
                }

                characterMovement.SetMovementDisable(false);
                playerSkills[8].currentCooldown = playerSkills[8].cooldown;
                SkillOnline = false;
                anim.SetTrigger("StopAA");
                
                currentCounterAttackTime = 0.0f;
                transform.SendMessage("SetDodging", false);
                break;
            case 99: // DODGE
                //print("2");
                characterMovement.Dodge();
                transform.SendMessage("SetDodging", true);
                yield return new WaitForSeconds(DODGING_TIME);
                transform.SendMessage("SetDodging", false);
                SkillOnline = false;
                break;
            default:
                print("Skill error");
                break;
        }
    }

    private void ProcessComboBarSystem()
    {
        if (currentComboTime > comboResetTime)
        {
            currentCombo = 0;
            currentComboTime = 0.0f;
        }
        if (currentCombo > maxCombo) currentCombo = maxCombo;

        float comboValue = (float)currentCombo / (float)maxCombo;
        if (currentComboFillAmount <= comboValue - errorCorrectionFloat)
        {
            ComboBar.fillAmount += comboFillingSpeed;
            currentComboFillAmount += comboFillingSpeed;
            //print("++");
        }
        else if (currentComboFillAmount > comboValue + errorCorrectionFloat)
        {
            currentComboFillAmount -= comboFillingSpeed;
            ComboBar.fillAmount -= comboFillingSpeed;
            //print("--");
        }
    }

    private void PlayerIsOnGround(bool b)
    {
        onGround = b;
    }

    private void SetMoving(bool b)
    {
        isMoving = b;
    }

    private bool IsCharging()
    {
        return SkillOnline1 || SkillOnline2 || SkillOnline3;
    }

    private void SetChargingBarVisibility(bool b)
    {
        ChargingBarBackground.gameObject.SetActive(b);
        for(int i = 0; i < 3; i++)
        {
            ChargingBars[i].gameObject.SetActive(b);
        }
    }

    private void IncreaseComboPoint()
    {
        ++currentCombo;
        currentComboTime = 0.0f;
        //print(currentCombo);
    }

    public void ReadyCounterAttack()
    {
        readyToCounter = true;
    }
}
