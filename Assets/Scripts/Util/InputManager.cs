using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Util
{

    public class InputManager : MonoBehaviour
    {
        //Look at this

        public static InputManager instance;
        public bool aPressed1, startPressed1, rtPressed1, ltPressed1;
        public float lStickX1, lStickY1, rStickX1, rStickY1;
        public bool aPressed2, startPressed2, rtPressed2, ltPressed2;
        public float lStickX2, lStickY2, rStickX2, rStickY2;

        //Don't look below this please

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKey(GetKeyCode("A", 1)))
            {
                aPressed1 = true;
            }
            else
            {
                aPressed1 = false;
            }
            if (Input.GetKey(GetKeyCode("A", 2)))
            {
                aPressed2 = true;
            }
            else
            {
                aPressed2 = false;
            }
            if (Input.GetKey(GetKeyCode("Start", 1)))
            {
                startPressed1 = true;
            }
            else
            {
                startPressed1 = false;
            }
            if (Input.GetKey(GetKeyCode("Start", 2)))
            {
                startPressed2 = true;
            }
            else
            {
                startPressed2 = false;
            }
            UpdateAxis();
        }

        void Awake()
        {
            if (instance == null)
            {
                DontDestroyOnLoad(this.gameObject);
                instance = this;
            }
            else if (this != instance)
            {
                Destroy(this.gameObject);
                return;
            }

        }

        private void UpdateAxis()
        {
            lStickX1 = Input.GetAxisRaw("LeftStickX1");
            lStickX2 = Input.GetAxisRaw("LeftStickX2");
            lStickY1 = Input.GetAxisRaw("LeftStickY1");
            lStickY2 = Input.GetAxisRaw("LeftStickY2");
            switch (Application.platform)
            {
                case RuntimePlatform.OSXDashboardPlayer:
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.OSXPlayer:
                case RuntimePlatform.OSXWebPlayer:
                    rStickX1 = Input.GetAxisRaw("RightOSXStickX1");
                    rStickX2 = Input.GetAxisRaw("RightOSXStickX2");
                    rStickY1 = Input.GetAxisRaw("RightOSXStickY1");
                    rStickY2 = Input.GetAxisRaw("RightOSXStickY2");
                    if (Input.GetAxisRaw("LeftOSXTrigger1") > 0)
                    {
                        ltPressed1 = true;
                    }
                    else
                    {
                        ltPressed1 = false;
                    }
                    if (Input.GetAxisRaw("RightOSXTrigger1") > 0)
                    {
                        rtPressed1 = true;
                    }
                    else
                    {
                        rtPressed1 = false;
                    }
                    if (Input.GetAxisRaw("LeftOSXTrigger2") > 0)
                    {
                        ltPressed2 = true;
                    }
                    else
                    {
                        ltPressed2 = false;
                    }
                    if (Input.GetAxisRaw("RightOSXTrigger2") > 0)
                    {
                        rtPressed2 = true;
                    }
                    else
                    {
                        rtPressed2 = false;
                    }
                    break;
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsWebPlayer:
                    rStickX1 = Input.GetAxisRaw("RightWinStickX1");
                    rStickX2 = Input.GetAxisRaw("RightWinStickX2");
                    rStickY1 = Input.GetAxisRaw("RightWinStickY1");
                    rStickY2 = Input.GetAxisRaw("RightWinStickY2");
                    if (Input.GetAxisRaw("LeftWinTrigger1") > 0)
                    {
                        ltPressed1 = true;
                    }
                    else
                    {
                        ltPressed1 = false;
                    }
                    if (Input.GetAxisRaw("RightWinTrigger1") > 0)
                    {
                        rtPressed1 = true;
                    }
                    else
                    {
                        rtPressed1 = false;
                    }
                    if (Input.GetAxisRaw("LeftWinTrigger2") > 0)
                    {
                        ltPressed2 = true;
                    }
                    else
                    {
                        ltPressed2 = false;
                    }
                    if (Input.GetAxisRaw("RightWinTrigger2") > 0)
                    {
                        rtPressed2 = true;
                    }
                    else
                    {
                        rtPressed2 = false;
                    }
                    break;
                default:
                    rStickX1 = Input.GetAxisRaw("RightWinStickX1");
                    rStickX2 = Input.GetAxisRaw("RightWinStickX2");
                    rStickY1 = Input.GetAxisRaw("RightWinStickY1");
                    rStickY2 = Input.GetAxisRaw("RightWinStickY2");
                    if (Input.GetAxisRaw("LeftLinuxTrigger1") > 0)
                    {
                        ltPressed1 = true;
                    }
                    else
                    {
                        ltPressed1 = false;
                    }
                    if (Input.GetAxisRaw("RightLinuxTrigger1") > 0)
                    {
                        rtPressed1 = true;
                    }
                    else
                    {
                        rtPressed1 = false;
                    }
                    if (Input.GetAxisRaw("LeftLinuxTrigger2") > 0)
                    {
                        ltPressed2 = true;
                    }
                    else
                    {
                        ltPressed2 = false;
                    }
                    if (Input.GetAxisRaw("RightLinuxTrigger2") > 0)
                    {
                        rtPressed2 = true;
                    }
                    else
                    {
                        rtPressed2 = false;
                    }
                    break;
            }                
        }

        private static KeyCode GetKeyCode(string btn, int controllerNum)
        {
            switch (btn)
            {
                case "A":
                    switch (Application.platform)
                    {
                        case RuntimePlatform.OSXDashboardPlayer:
                        case RuntimePlatform.OSXEditor:
                        case RuntimePlatform.OSXPlayer:
                        case RuntimePlatform.OSXWebPlayer:
                            switch (controllerNum)
                            {
                                case 1: return KeyCode.Joystick1Button16;
                                case 2: return KeyCode.Joystick2Button16;
                                default: return KeyCode.JoystickButton16;
                            }
                        default:
                            switch (controllerNum)
                            {
                                case 1: return KeyCode.Joystick1Button0;
                                case 2: return KeyCode.Joystick2Button0;
                                default: return KeyCode.JoystickButton0;
                            }
                    }
                case "Start":
                    switch (Application.platform)
                    {
                        case RuntimePlatform.OSXDashboardPlayer:
                        case RuntimePlatform.OSXEditor:
                        case RuntimePlatform.OSXPlayer:
                        case RuntimePlatform.OSXWebPlayer:
                            switch (controllerNum)
                            {
                                case 1: return KeyCode.Joystick1Button9;
                                case 2: return KeyCode.Joystick2Button9;
                                default: return KeyCode.JoystickButton9;
                            }
                        default:
                            switch (controllerNum)
                            {
                                case 1: return KeyCode.Joystick1Button7;
                                case 2: return KeyCode.Joystick2Button7;
                                default: return KeyCode.JoystickButton7;
                            }
                    }
            }
            return KeyCode.None;
        }
    }
}
