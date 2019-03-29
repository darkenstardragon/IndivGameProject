using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text goldText;

    private int gold;
    // Start is called before the first frame update
    void Start()
    {
        gold = 0;
        UpdateGold();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGold();
    }

    void UpdateGold()
    {
        goldText.text = "GOLD: " + gold;
    }

    public void SetGold(int x)
    {
        gold = x;
    }

    public int GetGold()
    {
        return gold;
    }
}
