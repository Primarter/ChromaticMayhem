using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ObjectInfiniteScroll : MonoBehaviour
{
    [SerializeField] Vector3 scrollVelocity;
    [SerializeField] Vector3 maxPosition;

    void Update()
    {
        transform.position += scrollVelocity * Time.deltaTime;
        transform.position = new(
            transform.position.x % maxPosition.x,
            transform.position.y % maxPosition.y,
            transform.position.z % maxPosition.z
        );
    }
}
