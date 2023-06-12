using Gameplay.Player;
using Managers;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MapManager : MonoBehaviour
{

    [SerializeField] private Camera mainMenuCamera;

    [SerializeField] private List<ZonePoints> SpawnPoints;




    void Start()
    {

        SetPlayerSpawn(4);

        RespawnActivePlayers();


    }

    // Update is called once per frame
    void Update()
    {

        
    }

    //Sets the player spawns the map locations, Does not spawn them just sets the transform
    public void SetPlayerSpawn(int zone)
    {
        Player[] players = GameManager.GetInstance().GetPlayers();
        for(int i = 0; i < players.Length; i++)
        {
            GameManager.GetInstance().GetPlayer(i).SetRespawnPoint(SpawnPoints[zone].points[i].transform);
        }
    }

    public void RespawnActivePlayers()
    {
        Player[] players = GameManager.GetInstance().GetPlayers();
        for (int i = 0; i < players.Length; i++)
        {
            if (GameManager.GetInstance().GetPlayer(i).gameObject.activeSelf)
            {
                GameManager.GetInstance().GetPlayer(i).Respawn();
            }

        }
    }


    //Sets the map spawns and actually spawns the placer into the map

    public void InitializeMap(int zone)
    {
        mainMenuCamera.gameObject.SetActive(false);

        SetPlayerSpawn(zone);

        RespawnActivePlayers();
    }


}




[System.Serializable]
public class ZonePoints
{
    public string pointname;

    public GameObject[] points; 
}
