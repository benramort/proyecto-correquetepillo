using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;


namespace OnlineMultiplayer
{
    public class PointManager : NetworkBehaviour
    {

        public int maxPoints = 100;
        public float tickTime; //Time to reduce 1 point in seconds
        public GameObject catchArea;
        public GameObject labelHolder;
        public Animator animator;
        private bool gameEnded = false;

        public NetworkVariable<bool> isTarget = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        private NetworkVariable<int> points = new NetworkVariable<int>(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        private float timePassed;
        private Coroutine catchCoroutine;
        private TextMeshProUGUI pointsText;

        // Start is called before the first frame update
        public override void OnNetworkSpawn()
        {
            isTarget.OnValueChanged += (previousValue, newValue) =>
            {
                labelHolder.SetActive(newValue);
            };
            base.OnNetworkSpawn();
            if (IsOwner && IsServer) isTarget.Value = true;
            if (!IsOwner)
            {
                labelHolder.SetActive(isTarget.Value);
                return;
            };
            //gameManager = GameObject.Find("Scripter").GetComponent<GameManager>(); Reactivar esto
            pointsText = transform.root.Find("Interface").GetComponentInChildren<TextMeshProUGUI>();
            points.Value = maxPoints;
            animator = GetComponentInChildren<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            //Debug.Log("Puntos: " + points.Value);
            if (!IsOwner) return;
            ReducePoints();
        }

        private void ReducePoints()
        {
            if (isTarget.Value)
            {
                if (timePassed >= tickTime)
                {
                    timePassed = 0;
                    if (points.Value >= 1)
                    {
                        points.Value--;
                        // Actualiza los puntos en FullGameManager
                        //if (IsOwner)
                        //{
                        //    for (int i = 0; i < FullGameManager.INSTANCE.playerDataList.Count; i++)
                        //    {
                        //        if (FullGameManager.INSTANCE.playerDataList[i].clientId == OwnerClientId)
                        //        {
                        //            var pd = FullGameManager.INSTANCE.playerDataList[i];
                        //            FullGameManager.INSTANCE.playerDataList[i] = new FullGameManager.PlayerData(
                        //                pd.clientId, pd.playerType, pd.isReady, pd.playerPosition, points.Value);
                        //            break;
                        //        }
                        //    }
                        //}

                        pointsText.text = points.Value.ToString();
                    }
                }
                else
                {
                    timePassed += Time.deltaTime;
                }


            }
            if (points.Value <= 0 && !gameEnded)
            {
                gameEnded = true;
                FullGameManager.INSTANCE.EndGameRpc();
            }
        }

        public void Catch(InputAction.CallbackContext context)
        {
            if (context.performed)
            {

                if (catchCoroutine == null)
                {
                    animator.SetTrigger("attack");
                    catchCoroutine = StartCoroutine(CatchCoroutine());
                }
                else
                {
                    Debug.Log("Alredy catching");
                }

            }
        }

        IEnumerator CatchCoroutine()
        {
            catchArea.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            catchArea.SetActive(false);
            yield return new WaitForSeconds(0.01f);
            catchCoroutine = null;
        }

        [Rpc(SendTo.Owner)]
        public void GetCatchedRpc(RpcParams rpcParams = new RpcParams())
        {
            gameObject.GetComponent<PointManager>().isTarget.Value = false;
            labelHolder.SetActive(false);
            gameObject.GetComponent<Movement>().Freeze(FreezeType.CATCHFREEZE);
        }
    }
}
