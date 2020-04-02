using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteAlways]
[InitializeOnLoad]
public class CameraObjectVisibilityField : MonoBehaviour
{
    private static Camera main;

    [SerializeField] [Vector2Slider(0,10)] private Vector2 _offsets;

    public static float Height { get; private set; } = 0;
    public static float Width { get; private set; } = 0;
    private static float h;
    private static float w;


    private void Awake()
    {
        main = Camera.main;

        Height = main.orthographicSize * 2f;
        Width = Height * main.aspect;

        h = (Height / 2) + _offsets.y;
        w = (Width / 2) + _offsets.x;
    }

    private void Update()
    {
        if (main == null)
        {
            main = Camera.main;
        }

        Height = main.orthographicSize * 2f;
        Width = Height * main.aspect;

        h = (Height / 2) + _offsets.y;
        w = (Width / 2) + _offsets.x;
    }

    public static bool IsWithinBounds(Vector2 position)
    {
#if UNITY_EDITOR
        main = Camera.main;

        Height = main.orthographicSize * 2f;
        Width = Height * main.aspect;
#endif
        return !((position.x >= (main.transform.position.x + w) ||
                    position.x <= (main.transform.position.x - w)) ||
                        (position.y >= (main.transform.position.y + h) ||
                            position.y <= (main.transform.position.y - h)));
    }


    private void OnDrawGizmos()
    {
        if (main == null)
        {
            main = Camera.main;
        }

        Height = main.orthographicSize * 2f;
        Width = Height * main.aspect;

        float h = (Height / 2) + _offsets.y;
        float w = (Width / 2) + _offsets.x;


        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(main.transform.position + new Vector3(w, h, 0), main.transform.position + new Vector3(-w, h, 0));
        Gizmos.DrawLine(main.transform.position + new Vector3(-w, h, 0), main.transform.position + new Vector3(-w, -h, 0));
        Gizmos.DrawLine(main.transform.position + new Vector3(-w, -h, 0), main.transform.position + new Vector3(w, -h, 0));
        Gizmos.DrawLine(main.transform.position + new Vector3(w, -h, 0), main.transform.position + new Vector3(w, h, 0));
    }
}
