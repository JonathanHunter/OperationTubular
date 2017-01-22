using UnityEngine;
using System.Collections;
using Assets.Scripts.Util;
using Assets.Scripts.Manager;

public class PauseMenu : MonoBehaviour
{

    public GameObject PauseCanvas;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (InputManager.instance.startPressed1 || InputManager.instance.startPressed2)
        {
            if (GameManager.IsPaused == true)
            {
                GameManager.IsPaused = false;

            }
            else
            {
                GameManager.IsPaused = true;
            }

            PauseCanvas.SetActive(GameManager.IsPaused);
        }
    }
}
