namespace MiniGame.FindAndMatch
{ 
    using UnityEngine;
    using UnityEngine.UI;

    public class StarsContainerFind : MonoBehaviour
    {
        public const int MINI_LEVEL_TARGET = 5;

        public GameObject[] stars;
        public GameObject[] starStates;
        public Sprite goldenStar;
        public Sprite normalStar;
        public static StarsContainerFind instance;
        public float starScaleSize;
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
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            // stars[0].transform.GetChild(0).gameObject.SetActive(true);
        }

        // Update is called once per frame
        void Update()
        {

        }
        public void LevelCleared(int starNumber)
        {

            if (starNumber < stars.Length)
            {
                stars[starNumber].GetComponent<Image>().sprite = goldenStar;
                LeanTween.scale(stars[starNumber], new Vector3(starScaleSize, starScaleSize, starScaleSize), 0.5f).setLoopPingPong();

            }
        }
        public void MiniLevelCleared(int starNumber, int count)
        {
            if (starNumber < stars.Length)
            {
                stars[starNumber].transform.GetChild(count).gameObject.SetActive(true);
             
            }

            if (GameManagerFind.instance.IsLevelCleared() && count >= MINI_LEVEL_TARGET - 1)
            {
                SetAllStarsNormal();
            }

        }


        public void SetAllStarsNormal()
        {
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].GetComponent<Image>().color = Color.white;

                LeanTween.cancel(stars[i]);
                LeanTween.scale(stars[i], new Vector3(1f, 1f, 1f), 0.5f);
                stars[i].GetComponent<Image>().sprite = normalStar;
                for (int j = 0; j < MINI_LEVEL_TARGET; j++)
                {
                    stars[i].transform.GetChild(j).gameObject.SetActive(false);
                }
            }
        }
        public void LevelSkipped(int starNumber)
        {
            if (starNumber < stars.Length)
            {

                stars[starNumber].GetComponent<Image>().color = Color.gray;
            }
        }



    }

}