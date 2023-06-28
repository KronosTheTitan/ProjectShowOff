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

        [SerializeField] private List<ZonePoints> spawnPoints;
        [SerializeField] private List<HillPoints> hillLocations;

        [SerializeField] private Hill hill;
        [SerializeField] private GameObject hillObject;
        [SerializeField] private GameManager manager;
        [SerializeField] private UIManager uiManager;

        [SerializeField] private Camera winnerCamera;
        [SerializeField] private GameObject podium;

        [SerializeField] private GameObject twoWaySplitScreen;
        [SerializeField] private GameObject threeWaySplitScreen;
        [SerializeField] private GameObject fourWaySplitScreen;

        [SerializeField] private GameObject scoresAndTimer;

        [SerializeField] private GameObject tutorialScreen;

        [SerializeField] private float timeRemaining;
        public bool timerIsRunning = false;
        public TextMeshProUGUI timeText;



        void Start()
        {

            SetPlayerSpawn(4);

            RespawnActivePlayers();

            //manager.OnGameOver += GameOverScreen;
            

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
                    GameOverScreen(GameManager.GetInstance().GetHighestScoringPlayer());
                }

            }
        }

        //Sets the player spawns the map locations, Does not spawn them just sets the transform
        public void SetPlayerSpawn(int zone)
        {
            Player[] players = GameManager.GetInstance().GetPlayers();
            for (int i = 0; i < players.Length; i++)
            {
                GameManager.GetInstance().GetPlayer(i).SetRespawnPoint(spawnPoints[zone].points[i].transform);
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
            hill.SetHillSites(hillLocations[zone].points);
            hill.ResetSite();

        }


        //Sets the map spawns and actually spawns the placer into the map

        public void InitializeMap(int zone)
        {
            SetPlayerSpawn(zone);
            hillObject.SetActive(true);
            StartCoroutine(TutorialTime());
        }

        public void ResetToMainMenu()
        {
            scoresAndTimer.SetActive(false);
            GameManager.GetInstance().ReplayGame();
        }

        private void GameOverScreen(Player winner)
        {
            //winner.transform.position = podium.transform.position;

            SetPlayerSpawn(5);
            hillObject.SetActive(false);

            winner.SetRespawnPoint(podium.transform);
            

            winnerCamera.gameObject.SetActive(true);
            
            scoresAndTimer.SetActive(false);
            StartCoroutine(VictoryTime());
        }

        IEnumerator VictoryTime()
        {
            RespawnActivePlayers();
            GameManager.GetInstance().gameState = GameManager.GameStates.Victory;
            uiManager.UpdateSplitScreen();
            
            yield return new WaitForSeconds(10f);
            winnerCamera.gameObject.SetActive(false);
            mainMenuCamera.gameObject.SetActive(true);
            SetPlayerSpawn(4);
            GameManager.GetInstance().gameState = GameManager.GameStates.MainMenu;
            RespawnActivePlayers();
        }

        IEnumerator TutorialTime()
        {
           
            tutorialScreen.SetActive(true);
            RespawnActivePlayers();
            yield return new WaitForSeconds(2f);
            mainMenuCamera.gameObject.SetActive(false);
            tutorialScreen.SetActive(false);
            scoresAndTimer.SetActive(true);
            GameManager.GetInstance().gameState = GameManager.GameStates.InMatch;
            
            uiManager.UpdateSplitScreen();
            timerIsRunning = true;
            timeRemaining = 180;
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