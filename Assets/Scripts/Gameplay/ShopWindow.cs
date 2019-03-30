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
    }
    
    void ShowShopUI(bool b)
    {
        shopMenu.SetActive(b);
    }

    public void AttackSpeedIncrease()
    {
        skillScript.AttackSpeedLevel += 1;
    }

    public void AttackSpeedDecrease()
    {
        skillScript.AttackSpeedLevel -= 1;
    }

    public void UltimateCostLevelIncrease()
    {
        skillScript.UltimateCostLevel += 1;
    }

    public void UltimateCostLevelDecrease()
    {
        skillScript.UltimateCostLevel -= 1;
    }
}
