using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformUVScroller : MonoBehaviour
{
    [SerializeField] private Transform drivingTransform;
    [SerializeField] private Material targetMaterial;
    [SerializeField] private float scale = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        targetMaterial.mainTextureOffset = new Vector2(-drivingTransform.position.x * scale, -drivingTransform.position.z * scale);
    }
}
