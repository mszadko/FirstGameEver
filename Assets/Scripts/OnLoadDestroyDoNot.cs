using UnityEngine;
using System.Collections;

public class OnLoadDestroyDoNot : MonoBehaviour {//Yoda styled name becouse there is actuall function DontDestroyOnLoad

	// Use this for initialization
	void Start ()
    {
        DontDestroyOnLoad(gameObject);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
