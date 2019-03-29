using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopWindow : MonoBehaviour
{
    public GameObject shopMenu;
    public bool active;
    public GameObject player;
    public SkillScript skillScript;
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
    }
    
    void ShowShopUI(bool b)
    {
        shopMenu.SetActive(b);
    }

    public void test()
    {
        print("testtt");
    }
}
