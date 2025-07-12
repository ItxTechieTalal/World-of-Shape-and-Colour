
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MiniGame.FindAndMatch
{

    public class GameManagerFind : MonoBehaviour
    {

        public static GameManagerFind instance;
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
            AudioManagerFind.instance.PlayButtonSound(0);

            GamePlayPanelSize.SetActive(true);
            InitialPanelSize.SetActive(false);
            GamePlayPanelFind.instance.CreateQuestion();
            GamePlayPanelFind.instance.PlayMonitorContainer();
        }

        public void ActiveHowToPlayPanel()
        {
            Handheld.Vibrate();
            HowToPlayPanelSize.SetActive(true);
            AudioManagerFind.instance.PlayButtonSound(1);
        }
        public void SolverTheProblemSound()
        {
            Handheld.Vibrate();

            AudioManagerFind.instance.PlayButtonSound(1);
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