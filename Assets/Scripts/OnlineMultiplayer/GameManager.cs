using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace OnlineMultiplayer
{
    public class GameManager : MonoBehaviour
    {

        public static GameManager instance;
        public FullGameManager fullGameManager;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            } else
            {
                Destroy(this);
            }
        }
    }
}
