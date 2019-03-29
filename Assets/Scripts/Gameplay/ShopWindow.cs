using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopWindow : MonoBehaviour
{
    public GameObject shopMenu;
    public bool active;
    // Start is called before the first frame update
    void Start()
    {
        active = false;
        ShowShopUI(active);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            active = !active;
            print(active);
            ShowShopUI(active);
        }
    }
    
    void ShowShopUI(bool b)
    {
        shopMenu.SetActive(b);
    }


}
