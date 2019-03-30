using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopWindow : MonoBehaviour
{
    public GameObject shopMenu;
    public bool active;
    public GameObject player;
    public SkillScript skillScript;

    public Text attackSpeedLevel;
    public Text ultimateCostLevel;
    public Text ultimateDamageLevel;

    public Text dodgeAttackUnlockLevel;
    public Text dodgeCooldownLevel;
    public Text dodgeAttackDamageLevel;
    public Text dodgeIframeDurationLevel;

    public Text counterAttackUnlockLevel;
    public Text parryCooldownLevel;
    public Text counterAttackDamageLevel;
    public Text parryDurationLevel;

    // Start is called before the first frame update
    void Start()
    {
        active = false;
        ShowShopUI(active);
        skillScript = player.GetComponent<SkillScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            active = !active;
            print(active);
            ShowShopUI(active);
            skillScript.SetAutoAttackDisable(active);
        }
        UpdateText();
    }

    void UpdateText()
    {
        attackSpeedLevel.text = skillScript.AttackSpeedLevel + "/" + skillScript.AttackSpeedSize;
        ultimateCostLevel.text = skillScript.UltimateCostLevel + "/" + skillScript.UltimateCostSize;
        ultimateDamageLevel.text = skillScript.UltimateDamageLevel + "/" + skillScript.UltimateDamageSize;

        dodgeAttackUnlockLevel.text = skillScript.DodgeAttackUnlock ? "1/1" : "0/1";
        dodgeCooldownLevel.text = skillScript.DodgeCooldownLevel + "/" + skillScript.DodgeCooldownSize;
        dodgeAttackDamageLevel.text = skillScript.DodgeAttackDamageLevel + "/" + skillScript.DodgeAttackDamageSize;
        dodgeIframeDurationLevel.text = skillScript.DodgeIframeDurationLevel + "/" + skillScript.DodgeIframeDurationSize;

        counterAttackUnlockLevel.text = skillScript.CounterAttackUnlock ? "1/1" : "0/1";
        parryCooldownLevel.text = skillScript.ParryCooldownLevel + "/" + skillScript.ParryCooldownSize;
        parryDurationLevel.text = skillScript.ParryDurationLevel + "/" + skillScript.ParryDurationSize;
        counterAttackDamageLevel.text = skillScript.CounterAttackDamageLevel + "/" + skillScript.CounterAttackDamageSize;
    }
    
    void ShowShopUI(bool b)
    {
        shopMenu.SetActive(b);
    }

    public void SetAttackSpeedLevel(int v)
    {
        skillScript.AttackSpeedLevel += v;
    }

    public void SetUltimateCostLevel(int v)
    {
        skillScript.UltimateCostLevel += v;
    }

    public void SetUltimateDamageLevel(int v)
    {
        skillScript.UltimateDamageLevel += v;
    }

    public void DodgeAttackUnlock(bool b)
    {
        skillScript.DodgeAttackUnlock = b;
    }

    public void SetDodgeCooldownLevel(int v)
    {
        skillScript.DodgeCooldownLevel += v;
    }

    public void SetDodgeAttackDamageLevel(int v)
    {
        skillScript.DodgeAttackDamageLevel += v;
    }

    public void SetDodgeIframeDurationLevel(int v)
    {
        skillScript.DodgeIframeDurationLevel += v;
    }

    public void CounterAttackUnlock(bool b)
    {
        skillScript.CounterAttackUnlock = true;
    }

    public void SetParryCooldownLevel(int v)
    {
        skillScript.ParryCooldownLevel += v;
    }

    public void SetParryDurationLevel(int v)
    {
        skillScript.ParryDurationLevel += v;
    }

    public void SetCounterAttackDamageLevel(int v)
    {
        skillScript.CounterAttackDamageLevel += v;
    }
}
