
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MiniGame.WorldOfShape
{

    public class GameManagerWorld : MonoBehaviour
    {

        public static GameManagerWorld instance;
        public int clearedLevels = 0;
        public GameObject LevelClearedPanelSize;
        public GameObject GamePlayPanelSize;
        public GameObject InitialPanelSize;
        public GameObject HowToPlayPanelSize;
        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }



        public void StartTheGame()
        {
            Handheld.Vibrate();
            AudioManagerWorld.instance.PlayButtonSound(0);

            GamePlayPanelSize.SetActive(true);
            InitialPanelSize.SetActive(false);
            GamePlayPanelWorld.instance.CreateQuestion();
            // GamePlayPanelWorld.instance.PlayMonitorContainer();
        }

        public void ActiveHowToPlayPanel()
        {
            Handheld.Vibrate();
            HowToPlayPanelSize.SetActive(true);
            AudioManagerWorld.instance.PlayButtonSound(1);
        }
        public void SolverTheProblemSound()
        {
            Handheld.Vibrate();

            AudioManagerWorld.instance.PlayButtonSound(1);
        }


        public void CloseHowToPlayPanel()
        {
            Handheld.Vibrate();
            HowToPlayPanelSize.SetActive(false);
        }

        public void OpenInitialPanel()
        {
            Handheld.Vibrate();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }



        public bool IsLevelCleared(int count = 9)
        {
            return clearedLevels >= count;

        }
    
    }

}