using UnityEngine;
using System.Collections;
using Assets.Scripts.Util;
using Assets.Scripts.Manager;


public class LoadOnInput : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (InputManager.instance.startPressed1 || InputManager.instance.startPressed2 || Input.GetKeyUp(KeyCode.Return))
        {
            GameManager.instance.LoadMenu();
        }
	}
}
