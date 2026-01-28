
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MiniGame.WorldOfShape
{

    public class GameManagerWorldV2 : MonoBehaviour
    {

        public static GameManagerWorldV2 instance;
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
        #region miniLevel
        public int miniLevel = 0;

        public void IncrMiniLevel()
        {
            miniLevel++;
            StarsContainerWorldV2.instance.MiniLevelCleared(clearedLevels, miniLevel - 1);
            GamePlayWorldV2.instance.SpawnRandom();
        }
        public int GetMiniLevel()
        {
            return miniLevel;
        }

        public void ResetMiniLevel()
        {
            miniLevel = 0;

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

        #endregion

        public bool IsLevelCleared(int count = 9)
        {
            return clearedLevels >= count;

        }


        #region SingleFuncs
        public void SkipButton()
        {
            // LeanTween.cancelAll();
            // isCreatingQuestion = true;
            Handheld.Vibrate();
            //  StopCoroutine(createQuestionRoutine);

            bool isLevelCleared = IsLevelCleared(9);
            ResetMiniLevel();
            GamePlayWorldV2.instance.SpawnRandom();

            if (isLevelCleared)
            {
                LevelClearedPanelSize.gameObject.SetActive(true);
                // ClearChilds();
                // StartCoroutine(ClearChilds());
                return;
            }
            AudioManagerWorld.instance.PlayButtonSound(6);

            StarsContainerWorld.instance.LevelSkipped(clearedLevels);
            clearedLevels++;
            // isCreatingQuestion = false;
            // if (isCreatingQuestion == false && createQuestionRoutine == null)
            // {
            //     CreateQuestion();
            //  }

        }
        #endregion

    }



}