using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {

    [HideInInspector]
    public GameObject[] players;
    int[] wins = new int[4];
	// Use this for initialization
	
	void Update()
    {
        UpdateText();
    }
	
    public void SpawnHUD()
    {
        for (int i = 0; i < players.Length; i++)
        {
			//spawn HUDComponent(text with wins + healthbar)on the HUDCanvas and send references to this object to a player
            players[i].GetComponent<Player>().HUDComponent = Instantiate(players[i].GetComponent<Player>().HUDComponent, gameObject.transform, false) as GameObject;
			//now player in this field dont have a prefab of this component but a gameObject that is actually in the scene
        }
    }
    void UpdateText()
    {
        int activePlayersCount = 0;
        for (int i = 0; i < players.Length; i++)
        {
            //worst line of code i've ever written
            players[i].GetComponent<Player>().HUDComponent.GetComponentInChildren<Text>().text = "Player:" + (i + 1) + "\nWins:" + wins[i] + "\n" + players[i].GetComponent<Player>().ammoInMag + "/" + players[i].GetComponent<Player>().ammo;//

            if (players[i].GetComponent<Player>().active)
            {
                activePlayersCount++;
            }
        }
    }
    public void UpdateWins(int winnerNumber)
    {
        wins[winnerNumber]++;
    }
}

