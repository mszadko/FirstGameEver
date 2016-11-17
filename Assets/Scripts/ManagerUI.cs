using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ManagerUI : MonoBehaviour {
    public Canvas mainmenu;
    public Canvas exitmenu;
    public Canvas chooseplayer;
    public int players=-1;
	// Use this for initialization
	void Start ()
    {
        mainmenu.enabled = true;
        exitmenu.enabled = false;
        chooseplayer.enabled = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
	}
    public void PressedStart()
    {
        mainmenu.enabled = false;
        exitmenu.enabled = false;
        chooseplayer.enabled = true;
        
    }
    public void PressedExit()
    {
        mainmenu.enabled = false;
        exitmenu.enabled = true;
        chooseplayer.enabled = false;
    }
    public void Pressed2()
    {
        players = 2;
        mainmenu.enabled = false;
        exitmenu.enabled = false;
        chooseplayer.enabled = false;
    }
    public void Pressed3()
    {
        players = 3;
        mainmenu.enabled = false;
        exitmenu.enabled = false;
        chooseplayer.enabled = false;
    }
    public void Pressed4()
    {
        players = 4;
        mainmenu.enabled = false;
        exitmenu.enabled = false;
        chooseplayer.enabled = false;
    }
    public void ExitYes()
    {
        Application.Quit();
    }
    public void ExitNo()
    {
        mainmenu.enabled = true;
        exitmenu.enabled = false;
        chooseplayer.enabled = false;
    }
}
