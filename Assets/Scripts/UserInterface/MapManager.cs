using Gameplay;
using Gameplay.Player;
using Managers;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using TMPro;

namespace Managers
{
    public class MapManager : MonoBehaviour
    {

        [SerializeField] private Camera mainMenuCamera;

        [SerializeField] private List<ZonePoints> SpawnPoints;
        [SerializeField] private List<HillPoints> HillLocations;

        [SerializeField] private Hill hill;
        [SerializeField] private GameManager manager;
        [SerializeField] private UIManager uiManager;

        [SerializeField] private Camera winnerCamera;
        [SerializeField] private GameObject podium;

        [SerializeField] private GameObject TwowaySplitScreen;
        [SerializeField] private GameObject ThreeWaySplitScreen;
        [SerializeField] private GameObject FourWaySplitScreen;

        public float timeRemaining = 180;
        public bool timerIsRunning = false;
        public TextMeshProUGUI timeText;



        void Start()
        {

            SetPlayerSpawn(4);

            RespawnActivePlayers();

            manager.OnGameOver += gameOverScreen;

        }

        // Update is called once per frame
        void Update()
        {
            if (timerIsRunning)
            {
                if (timeRemaining > 0)
                {
                    timeRemaining -= Time.deltaTime;
                    DisplayTime(timeRemaining);
                }
                else
                {
                    Debug.Log("Time has run out!");
                    timeRemaining = 0;
                    timerIsRunning = false;
                    gameOverScreen(GameManager.GetInstance().GetHighestScoringPlayer());
                }

            }
        }

        //Sets the player spawns the map locations, Does not spawn them just sets the transform
        public void SetPlayerSpawn(int zone)
        {
            Player[] players = GameManager.GetInstance().GetPlayers();
            for (int i = 0; i < players.Length; i++)
            {
                GameManager.GetInstance().GetPlayer(i).SetRespawnPoint(SpawnPoints[zone].points[i].transform);
            }
            SetHillLocations(zone);
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



        public void SetHillLocations(int zone)
        {
            hill.SetHillSites(HillLocations[zone].points);

        }


        //Sets the map spawns and actually spawns the placer into the map

        public void InitializeMap(int zone)
        {
            mainMenuCamera.gameObject.SetActive(false);
            winnerCamera.gameObject.SetActive(false);
            SetPlayerSpawn(zone);
            uiManager.UpdateSplitScreen();
            RespawnActivePlayers();
            timerIsRunning = true;
            timeRemaining = 180;
            
        }

        public void ResetToMainMenu()
        {
            GameManager.GetInstance().ReplayGame();
        }

        private void gameOverScreen(Player winner)
        {
            winner = GameManager.GetInstance().GetHighestScoringPlayer();
            winnerCamera.gameObject.SetActive(true);
            winner.transform.position = podium.transform.position;
            StartCoroutine(VictoryTime());
        }

        IEnumerator VictoryTime()
        {
            
            TwowaySplitScreen.SetActive(false);
            
            ThreeWaySplitScreen.SetActive(false);
           
            FourWaySplitScreen.SetActive(false);
            
            yield return new WaitForSeconds(10f);
            winnerCamera.gameObject.SetActive(false);
            SetPlayerSpawn(4);
            mainMenuCamera.gameObject.SetActive(true);
            RespawnActivePlayers();
            
        }

        void DisplayTime(float timeToDisplay)
        {
            timeToDisplay += 1;
            float minutes = Mathf.FloorToInt(timeToDisplay / 60);
            float seconds = Mathf.FloorToInt(timeToDisplay % 60);
            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

    }

    
}



[System.Serializable]
public class ZonePoints
{
    public string pointname;

    public GameObject[] points; 
}

[System.Serializable]
public class HillPoints
{
    public string pointname;

    public GameObject[] points;
}