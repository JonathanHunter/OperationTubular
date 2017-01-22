using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Assets.Scripts.Manager
{

    public class CreditsManager : MonoBehaviour
    {
        //set this to true to begin credits
        public bool startCredits = false;

        float timer, maxTimer = 3f;
        int count = 0;
        public MaskableGraphic[] creditBlurbs;

        float fadeSpeed = 0.05f;

        // Use this for initialization
        void Start()
        {
            timer = 0;
        }

        // Update is called once per frame
        void Update()
        {
            if (startCredits)
            {
                foreach (GameObject p in GameManager.instance.players)
                    p.SetActive(false);
                if (timer > 0)
                {
                    timer -= Time.deltaTime;
                }
                else
                {
                    switch (count)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                            StartCoroutine(FadeIn(creditBlurbs[count].gameObject, 0));
                            break;
                        case 4:
                            creditBlurbs[2].transform.parent = creditBlurbs[1].transform;
                            creditBlurbs[3].transform.parent = creditBlurbs[1].transform;
                            StartCoroutine(FadeOut(creditBlurbs[1].gameObject));
                            break;
                        case 5:
                            StartCoroutine(FadeIn(creditBlurbs[4].gameObject, 0));
                            break;
                        case 6:
                            StartCoroutine(FadeOut(creditBlurbs[4].gameObject));
                            break;
                        case 7:
                            StartCoroutine(FadeIn(creditBlurbs[5].gameObject, 0));
                            break;
                        case 8:
                            StartCoroutine(FadeOut(creditBlurbs[5].gameObject));
                            break;
                        default:
                            Scripts.Manager.GameManager.instance.LoadTitle();
                            break;
                    }
                    count++;
                    timer = maxTimer;
                }
            }
        }

        IEnumerator FadeIn(GameObject parent, float delay)
        {
            yield return new WaitForSeconds(delay);
            parent.SetActive(true);
            for (float f = 0f; f <= 1.01f; f += 0.1f)
            {
                AdjustAlphaPlusChildren(parent, f);
                yield return new WaitForSeconds(fadeSpeed);
            }
        }

        IEnumerator FadeOut(GameObject parent)
        {
            for (float f = 1f; f >= 0; f -= 0.1f)
            {
                AdjustAlphaPlusChildren(parent, f);
                yield return new WaitForSeconds(fadeSpeed);
            }
            parent.SetActive(false);
        }

        void AdjustAlphaPlusChildren(GameObject parent, float val)
        {
            List<MaskableGraphic> tempChildren = new List<MaskableGraphic>(parent.GetComponentsInChildren<MaskableGraphic>());
            if (tempChildren != null)
            {
                foreach (MaskableGraphic obj in tempChildren)
                {
                    obj.color = new Color(obj.color.r, obj.color.b, obj.color.g, val);
                }
            }
        }





    }
}
