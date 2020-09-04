using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform pInicio, pFim;
    public RectTransform background;
    private float backgroundWidth;
    private Vector3 lastCameraPosition;

    [System.Serializable]
    struct Background
    {
        public GameObject backgroundGameObject;
        public Vector2 parallaxFactors;
    }
    [SerializeField] private List<Background> backgrounds;

    private void Awake()
    {
        backgroundWidth = background.sizeDelta.x;
    }

    void Start()
    {
        //lastCameraPosition = this.transform.position;
    }

    private void LateUpdate()
    {
        Vector2 deltaSize = Vector2.Lerp(new Vector2(((1920f - backgroundWidth) / 2f), 0f),
            new Vector2(((backgroundWidth - 1920f) / 2f), 0f),
            (pFim.position.x - this.transform.position.x) / (pFim.position.x - pInicio.position.x));

        background.anchoredPosition = deltaSize;

        //Vector3 deltaMovement = this.transform.position - lastCameraPosition;

        //foreach (var background in backgrounds)
        //{
        //    background.backgroundGameObject.transform.position += new Vector3(deltaMovement.x * background.parallaxFactors.x, deltaMovement.y * background.parallaxFactors.y, background.backgroundGameObject.transform.position.z);
        //}
        //// for (int i = 0; i < backgrounds.Count; i++)
        //// {
        ////     backgrounds[i].backgroundGameObject.transform.position += new Vector3(deltaMovement.x * backgrounds[i].parallaxFactors.x, deltaMovement.y * backgrounds[i].parallaxFactors.y, backgrounds[i].backgroundGameObject.transform.position.z);
        //// }

        //lastCameraPosition = this.transform.position;
    }
}
