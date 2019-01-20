using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class UserStats : MonoBehaviour
{
    public Skill[] allSkills;
    public Skill[] playerSkills;

    public Texture barsBackgroundTexture;

    public GameObject meleeDetection;
    // Start is called before the first frame update

    public void Start()
    {
        playerSkills[0].id = allSkills[0].id;
        playerSkills[0].name = allSkills[0].name;
        playerSkills[0].description = allSkills[0].description;
        playerSkills[0].icon = allSkills[0].icon;

        playerSkills[1].id = allSkills[1].id;
        playerSkills[1].name = allSkills[1].name;
        playerSkills[1].description = allSkills[1].description;
        playerSkills[1].icon = allSkills[1].icon;
    }

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

    public void Update()
    {
        if (Input.GetKeyDown("1"))
            activateSkill(playerSkills[0].id);
        if (Input.GetKeyDown("2"))
            activateSkill(playerSkills[1].id);
    }

    private void activateSkill(int id)
    {
        switch (id)
        {
            case 0:
                meleeDetection.SendMessage("useSkill", 10);
                break;
            case 1:
                print("Use skill 2");
                break;
            default:
                print("Skill error");
                break;
        }
    }

}
