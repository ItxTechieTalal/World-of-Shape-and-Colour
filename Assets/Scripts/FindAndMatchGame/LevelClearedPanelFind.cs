namespace MiniGame.FindAndMatch
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class LevelClearedPanelFind : MonoBehaviour
    {
        public GameObject Decoration;
        public GameObject greatContainer;
        // Start is called before the first frame update
        void OnEnable()
        {
            Invoke(nameof(PlayGreat), 0.5f);
            ManageDecoration();
            ManageGreatContainer();
            // GamePlayPanelFind.instance.EmptyAll();


        }
        void OnDisable()
        {
            TurnOffDecorationAnim();
        }
        void PlayGreat()
        {
            AudioManagerFind.instance.PlayButtonSound(7);

        }
        public void RepeatGame()
        {
            GamePlayPanelFind.instance.isCreatingQuestion = true;
            Handheld.Vibrate();
            GameManagerFind.instance.clearedLevels = 0;
            GamePlayPanelFind.instance.miniLevel = 0;

            this.gameObject.SetActive(false);
            StarsContainerFind.instance.SetAllStarsNormal();
            AudioManagerFind.instance.PlayButtonSound(8);
            GamePlayPanelFind.instance.StartCoroutine(GamePlayPanelFind.instance.MonitorContainer());
            GamePlayPanelFind.instance.isCreatingQuestion = false;
            GamePlayPanelFind.instance.CreateQuestion();

        }

        void ManageDecoration()
        {
            for (int i = 0; i < Decoration.transform.childCount; i++)
            {
                Transform child = Decoration.transform.GetChild(i);
                float originalY = child.localPosition.y;
                float floatAmount = Random.Range(10f, 20f);
                float duration = Random.Range(1f, 2f);

                LeanTween.moveLocalY(
                    child.gameObject,
                    originalY + floatAmount,
                    duration
                )
                .setEaseInOutSine()
                .setLoopPingPong();
            }
        }
        void TurnOffDecorationAnim()
        {
            for (int i = 0; i < Decoration.transform.childCount; i++)
            {
                LeanTween.cancel(Decoration.transform.GetChild(i).gameObject);
            }
            LeanTween.cancel(greatContainer);
        }

        public void ManageGreatContainer()
        {
            greatContainer.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            LeanTween.scale(greatContainer, new Vector3(0.6f, 0.6f, 0.6f), 1.5f).setLoopPingPong();

        }

    }

}