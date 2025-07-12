namespace MiniGame.Math
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class Characters : MonoBehaviour
    {
        public Image characterState;

        public float timeToWait = 0.1f;
        public bool shouldIncreaseSize = false;


        void OnEnable()
        {

            if (shouldIncreaseSize)
            {
                StartCoroutine(DelayedIncreaseSize());
            }
            else
            {
                StartCoroutine("ManageStates");
            }

        }

        void OnDisable()
        {
            LeanTween.cancel(characterState.rectTransform);
            characterState.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            StopAllCoroutines();
        }

        public IEnumerator ManageStates()
        {
            while (true)
            {
                yield return new WaitForSeconds(timeToWait);
                characterState.transform.localScale = new Vector3(1f, 1f, 1f);
                yield return new WaitForSeconds(timeToWait);
                characterState.transform.localScale = new Vector3(-1f, 1f, 1f);
            }

        }

        public void IncreaseSize()
        {
            LeanTween.scale(characterState.rectTransform, new Vector3(1f, 1f, 1f), 1f).setOnComplete(() => StartCoroutine("ManageStates"));

        }
        IEnumerator DelayedIncreaseSize()
        {
            yield return new WaitForSeconds(0.1f); // small buffer
            IncreaseSize();
        }



    }

}