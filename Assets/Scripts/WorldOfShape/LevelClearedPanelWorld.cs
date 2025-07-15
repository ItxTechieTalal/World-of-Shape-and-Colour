namespace MiniGame.WorldOfShape
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class LevelClearedPanelWorld : MonoBehaviour
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
            AudioManagerWorld.instance.PlayButtonSound(7);

        }
        public void RepeatGame()
        {
            // GamePlayPanelWorld.instance.isCreatingQuestion = true;
            Handheld.Vibrate();
            GameManagerWorld.instance.clearedLevels = 0;
            GamePlayPanelWorld.instance.miniLevel = 0;

            this.gameObject.SetActive(false);
            StarsContainerWorld.instance.SetAllStarsNormal();
            AudioManagerWorld.instance.PlayButtonSound(8);
            // GamePlayPanelWorld.instance.StartCoroutine(GamePlayPanelWorld.instance.MonitorContainer());
            // GamePlayPanelWorld.instance.isCreatingQuestion = false;
            GamePlayPanelWorld.instance.CreateQuestion();

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