using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinDrop : MonoBehaviour
{
    public GameObject player;
    public GameObject FloatingTextPrefab;
    public int bounty = 1;
    public HealthBar healthBar;
    public GameManager gameManager;

    private bool dropped = false;

    // Start is called before the first frame update
    void Start()
    {
        healthBar = GetComponent<HealthBar>();
        gameManager = player.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (healthBar.GetDead())
        {
            // print("test44");
            if (!dropped)
            {
                //print("test2");
                dropped = true;
                StartCoroutine(DropCoins(bounty));
            }
            
        }
    }

    IEnumerator DropCoins(int coin)
    {
        yield return new WaitForSeconds(0.5f);
        print("test");
        Vector3 newpos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        var x = Instantiate(FloatingTextPrefab, newpos, Quaternion.identity, transform);
        x.GetComponent<TextMesh>().color = Color.yellow;
        x.GetComponent<TextMesh>().text = coin.ToString();
        x.GetComponent<Transform>().LookAt(2 * transform.position - Camera.main.transform.position);
        gameManager.SetGold(gameManager.GetGold() + coin);
    }


}
