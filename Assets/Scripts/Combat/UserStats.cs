using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class UserStats : MonoBehaviour
{
    public Skill[] allSkills;
    public Skill[] playerSkills;

    public Texture barsBackgroundTexture;

    public GameObject meleeDetector;
    public GameObject AoeDetector;
    // Start is called before the first frame update

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
        if (Input.GetKeyDown("1") && playerSkills[0].currentCooldown == 0)
            activateSkill(playerSkills[0].id);
        if (Input.GetKeyDown("2") && playerSkills[1].currentCooldown == 0)
            activateSkill(playerSkills[1].id);
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
    private void activateSkill(int id)
    {
        switch (id)
        {
            case 0:
                meleeDetector.SendMessage("useSkill", 10);
                playerSkills[0].currentCooldown = playerSkills[0].cooldown;
                break;
            case 1:
                AoeDetector.SendMessage("useSkill", 20);
                playerSkills[1].currentCooldown = playerSkills[1].cooldown;
                break;
            default:
                print("Skill error");
                break;
        }
    }

}
