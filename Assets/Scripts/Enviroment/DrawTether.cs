using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Enviroment
{

    public class DrawTether : MonoBehaviour
    {

        public Player.Player player1, player2;

        public Material beamMaterial;

        public LineRenderer l1, l2;

        // Use this for initialization
        void Start()
        {
            l1.SetVertexCount(2);
            l2.SetVertexCount(2);
            l1.material = beamMaterial;
            l2.material = beamMaterial;
            l1.SetWidth(0.1f, 0.1f);
            l2.SetWidth(0.1f, 0.1f);
        }

        // Update is called once per frame
        void Update()
        {
            if (player1.health > 0)
            {
                l1.SetPosition(0, transform.position);
                l1.SetPosition(1, player1.transform.position);
                l1.enabled = true;
            }
            else
            {
                l1.enabled = false;
            }
            if (player2 != null)
            {
                if (player2.health > 0)
                {

                    l2.SetPosition(0, transform.position);
                    l2.SetPosition(1, player2.transform.position);
                    l2.enabled = true;
                }
                else
                {
                    l2.enabled = false;
                }
            }
        }
    }
}
