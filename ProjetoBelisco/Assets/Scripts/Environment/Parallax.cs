using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{

    private Vector3 lastCameraPosition;

    struct Background
    {
        public GameObject backgroundGameObject;
        public Vector2 parallaxFactors;
    }
    [SerializeField] private List<Background> backgrounds;

    void Start()
    {
        lastCameraPosition = this.transform.position;
    }

    private void LateUpdate() {
        Vector3 deltaMovement = this.transform.position - lastCameraPosition;

        foreach (var background in backgrounds)
        {
            background.backgroundGameObject.transform.position += new Vector3(deltaMovement.x * background.parallaxFactors.x, deltaMovement.y * background.parallaxFactors.y, background.backgroundGameObject.transform.position.z);
        }
        // for (int i = 0; i < backgrounds.Count; i++)
        // {
        //     backgrounds[i].backgroundGameObject.transform.position += new Vector3(deltaMovement.x * backgrounds[i].parallaxFactors.x, deltaMovement.y * backgrounds[i].parallaxFactors.y, backgrounds[i].backgroundGameObject.transform.position.z);
        // }

        lastCameraPosition = this.transform.position;
    }
}
