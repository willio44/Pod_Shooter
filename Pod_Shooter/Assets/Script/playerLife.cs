using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class playerLife : NetworkBehaviour {

    public RectTransform healthBar;
    public const int maxLife = 100;
    private NetworkStartPosition[] spawnPoints;

    [SyncVar(hook = "UpdateLife")]
    int currentLife = maxLife;


	// Use this for initialization
	void Start () {
		if(isLocalPlayer)
        {
            spawnPoints = FindObjectsOfType<NetworkStartPosition>();
        }
	}
	
    public void TakeDamage( int damage)
    {
        if(isServer)
        {
            currentLife -= damage;
            if(currentLife <= 0)
            {
                currentLife = 0;
                Die();
            }
        }
    }
    private void UpdateLife(int newLife)
    {
        healthBar.sizeDelta = new Vector2(newLife, healthBar.sizeDelta.y);
    }


    private void Die()
    {
        RpcReSpawn();
    }

    [ClientRpc]
    private void RpcReSpawn()
    {
        if(isLocalPlayer)
        {
            currentLife = maxLife;
            Vector3 spawnpoint = Vector3.zero;
            if(spawnPoints != null && spawnPoints.Length > 0)
                {
                spawnpoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
                }
            transform.position = spawnpoint;
        }
    }
    // Update is called once per frame
    void Update () {
		
	}
}
