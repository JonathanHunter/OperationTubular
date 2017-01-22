using UnityEngine;
using System.Collections;

public class MenuViewToggler : MonoBehaviour {

    public GameObject mainView;
    public GameObject creditsView;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void viewMain()
    {
        creditsView.SetActive(false);
        mainView.SetActive(true);
    }

    public void viewCredits()
    {
        creditsView.SetActive(true);
        mainView.SetActive(false);
    }
}
