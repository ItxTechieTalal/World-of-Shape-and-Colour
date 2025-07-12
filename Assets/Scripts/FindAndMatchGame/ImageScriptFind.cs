using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace MiniGame.FindAndMatch

{

    public class ImageScriptFind : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private Canvas canvas;
        private RectTransform rectTransform;
        private CanvasGroup canvasGroup;
        private Transform originalParent;
        private Vector2 initialAnchoredPos;


        private void Awake()
        {

            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>(); // ✅ Needed to enable/disable raycasts

            // initialPos = rectTransform;

            // Find the canvas automatically
            canvas = GetComponentInParent<Canvas>();
            if (canvas == null)
            {
                Debug.LogError("No Canvas found in parent hierarchy!");
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            originalParent = transform.parent;
            initialAnchoredPos = rectTransform.anchoredPosition;

            canvasGroup.blocksRaycasts = false; // Allow drop detection
        }

        public void OnDrag(PointerEventData eventData)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }

        // public void OnEndDrag(PointerEventData eventData)
        // {

        //     canvasGroup.blocksRaycasts = true;

        //     // If dropped on a valid target (tagged "Container"), snap into place
        //     if (eventData.pointerEnter != null && eventData.pointerEnter.CompareTag("Container"))
        //     {
        //         bool isSameQuestion = GamePlayPanelCreative.instance.AreSpritesFromSameQuestion(this.gameObject.GetComponent<Image>().sprite, eventData.pointerEnter.GetComponent<Image>().sprite);
        //         if (isSameQuestion)
        //         {
        //             Destroy(this.gameObject);
        //         }

        //     }
        //     else
        //     {
        //         transform.SetParent(originalParent);
        //         // rectTransform.anchoredPosition = Vector2.zero;
        //     }
        // }

        public void OnEndDrag(PointerEventData eventData)
        {

            canvasGroup.blocksRaycasts = true;

            // If dropped on a valid target (tagged "Container"), snap into place
            if (eventData.pointerEnter != null && eventData.pointerEnter.CompareTag("Container"))
            {
                Image img = eventData.pointerEnter.transform.GetComponent<Image>();
                // Sprite img2 = eventData.pointerEnter.transform.GetChild(2).GetComponent<Image>().sprite;
                bool isSameQuestion = GamePlayPanelFind.instance.AreSpritesFromSameQuestion
                (
                this.gameObject,
                eventData.pointerEnter
                );

                if (isSameQuestion)
                {
                    GamePlayPanelFind.instance.PlayAnswerMusic();
                    foreach (Transform child in eventData.pointerEnter.transform)
                    {
                        child.gameObject.SetActive(true);
                    }
                    img.color = Color.green;
                    LeanTween.scale(img.gameObject, Vector3.one * 0.8f, 0.3f).setLoopPingPong(2);
                    LeanTween.delayedCall(1f, () =>
                    {
                        if (img != null)
                        {
                            img.color = Color.white
                        ;
                        }
                    });
                    DestroyImmediate(this.gameObject);
                }
                else
                {
                    img.color = Color.red;
                    // Shake left and right repeatedly for 1 sec
                    float originalX = img.transform.localPosition.x;

                    LeanTween.moveLocalX(img.gameObject, originalX + 10f, 0.1f)
                        .setLoopPingPong(3);

                    AudioManagerFind.instance.PlayButtonSound(5);
                    LeanTween.delayedCall(1f, () => img.color = Color.white);
                    transform.SetParent(originalParent);
                    rectTransform.anchoredPosition = initialAnchoredPos;
                }


            }
            else
            {
                transform.SetParent(originalParent);
                rectTransform.anchoredPosition = initialAnchoredPos;
            }
        }
    }

}