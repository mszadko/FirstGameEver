using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class GameManager : MonoBehaviour {

    public CameraController cameraScript;
    public HUDManager HUDScript;
    public static GameManager instance = null;
    public BoardManager boardScript;
    public int PlayersNumber = -1;
    public GameObject[] prefabsOfPlayers;//this one got prefabst to create
    public List<GameObject> listOfPlayers;//this one got created models on the scene;
    //part for UI
    public Canvas mainMenu;
    public Canvas exitMenu;
    public Canvas choosePlayerNumer;
    public HUDManager HUD;

    private List<Vector3> spawnpoints;
    private List<int> spawnrotations;
    private WaitForSeconds roundDelay;

    void Awake()
    {
        roundDelay = new WaitForSeconds(3);
        listOfPlayers = new List<GameObject>();
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);//if we got already one game manager we need to destroy this one
        }
        DontDestroyOnLoad(gameObject);
        boardScript = GetComponent<BoardManager>();

        spawnpoints = boardScript.ReturnSpawnPositions(PlayersNumber);
        spawnrotations = boardScript.ReturnSpawnRotations(PlayersNumber);

        //UI part
        mainMenu.enabled = true;
        exitMenu.enabled = false;
        choosePlayerNumer.enabled = false;

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (exitMenu.enabled)
            {
                exitMenu.enabled = false;
            }
            else
            {
                exitMenu.enabled = true;
            }
        }
    }
    void SpawnPlayers(int n)//change spawnpoints
    {
        for (int i = 0; i < n; i++)
        {
            listOfPlayers.Add(Instantiate(prefabsOfPlayers[i], spawnpoints[i], Quaternion.Euler(0, 0, spawnrotations[i])) as GameObject);
        }
    }
    void SetCameraAndHudTargets()
    {
        GameObject[] playersTransform = new GameObject[listOfPlayers.Count];
        for (int i = 0; i < playersTransform.Length; i++)
        {
            playersTransform[i] = listOfPlayers[i];
        }
        cameraScript.players = playersTransform;
        HUDScript.players = playersTransform;

    }
    void ResetPlayers()
    {
        for (int i = 0; i < PlayersNumber; i++)
        {
            listOfPlayers[i].GetComponent<Player>().Reset();
            listOfPlayers[i].transform.position= new Vector3(spawnpoints[i].x, spawnpoints[i].y, spawnpoints[i].z);
            listOfPlayers[i].transform.eulerAngles = new Vector3(0, 0, spawnrotations[i]);
        }
    }

    //game loop part
    IEnumerator GameLoop()
    {
        yield return StartCoroutine(StartRound());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnd());
        yield return null;
        StartCoroutine(GameLoop());
    }

    IEnumerator StartRound()
    {
        HUD.GetComponentInChildren<Text>().text = "Prepare!";
        boardScript.ResetMap();
        ResetPlayers();
        yield return roundDelay;     
    }
    IEnumerator RoundPlaying()
    {
        HUD.GetComponentInChildren<Text>().text = "";
        int activePlayers;
        do
        {
            activePlayers = 0;
            for (int i = 0; i < PlayersNumber; i++)
            {
                if (listOfPlayers[i].GetComponent<Player>().active)
                    activePlayers++;
            }
            yield return null;
        } while (activePlayers > 1); 
    }
    IEnumerator RoundEnd()
    {
        
        for (int i = 0; i < PlayersNumber; i++)
        {
            if (listOfPlayers[i].GetComponent<Player>().active)
            {
                HUD.UpdateWins(i);//player i wins++
                HUD.GetComponentInChildren<Text>().text = "Player " + (i + 1) + " won!";
            }
                
        }
        yield return roundDelay;
    }
    //public methods for UI buttons
    public void PressedStart()
    {
        mainMenu.enabled = false;
        exitMenu.enabled = false;
        choosePlayerNumer.enabled = true;

    }
    public void PressedExit()
    {
        mainMenu.enabled = false;
        exitMenu.enabled = true;
        choosePlayerNumer.enabled = false;
    }
    public void Pressed2()
    {
        PlayersNumber = 2;
        mainMenu.enabled = false;
        exitMenu.enabled = false;
        choosePlayerNumer.enabled = false;
        SetEntireGame();
    }
    public void Pressed3()
    {
        PlayersNumber = 3;
        mainMenu.enabled = false;
        exitMenu.enabled = false;
        choosePlayerNumer.enabled = false;
        SetEntireGame();
    }
    public void Pressed4()
    {
        PlayersNumber = 4;
        mainMenu.enabled = false;
        exitMenu.enabled = false;
        choosePlayerNumer.enabled = false;
        SetEntireGame();
    }
    public void ExitGame()
    { 
        Application.Quit();
    }
    public void DontExitGame()
    {
        mainMenu.enabled = true;
        exitMenu.enabled = false;
        choosePlayerNumer.enabled = false;
    }

    void SetEntireGame()
    {
        SpawnPlayers(PlayersNumber);
        SetCameraAndHudTargets();
        HUD.SpawnHUD();
        boardScript.SetGameArea();
        StartCoroutine(GameLoop());
    }
}

