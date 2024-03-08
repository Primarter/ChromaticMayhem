using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class HideInFrontOfCamera : MonoBehaviour
{
    MeshRenderer meshRenderer;
    Collider objCollider;
    Camera mainCamera;
    Transform playerTransform;

    Plane[] planes;

    [SerializeField] LayerMask layer;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        objCollider = GetComponent<Collider>();
        mainCamera = Camera.main;
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    void Start()
    {
    }

    void Update()
    {
        // float distanceCameraPlayer = Vector3.Distance(mainCamera.transform.position, playerTransform.position);
        // RaycastHit hit;
        // bool didHit = Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, distanceCameraPlayer, layer);
        // Debug.DrawRay(mainCamera.transform.position, mainCamera.transform.forward * distanceCameraPlayer, Color.red, 2f);
        // if (didHit)
        // {
        //     meshRenderer.enabled = hit.transform.gameObject == this;
        // }
        // else
        // {
        //     meshRenderer.enabled = true;
        // }

        Plane playerCameraPlane = new Plane(mainCamera.transform.forward, playerTransform.position);
        Plane playerZPlane = new Plane(new Vector3(0f, 0f, 1f), playerTransform.position);
        Plane playerXPlane = new Plane(new Vector3(1f, 0f, 0f), playerTransform.position);

        bool sideX = playerXPlane.GetSide(transform.position - new Vector3(8f, 0f, -8f));
        // bool sideZ = playerZPlane.GetSide(transform.position - new Vector3(8f, 0f, -8f));
        bool sideZ = playerCameraPlane.GetSide(transform.position - new Vector3(8f, 0f, -8f));

        // meshRenderer.enabled = !(!sideX & !sideZ);
        meshRenderer.shadowCastingMode = (!sideX & !sideZ) ? ShadowCastingMode.ShadowsOnly : ShadowCastingMode.On;

        // planes = GeometryUtility.CalculateFrustumPlanes(mainCamera);
        // if (GeometryUtility.TestPlanesAABB(planes, objCollider.bounds))
        // {
        //     meshRenderer.enabled = false;
        // }
        // else
        // {
        //     meshRenderer.enabled = true;
        // }
    }

    void OnDrawGizmos()
    {
        // Gizmos.DrawSphere(mainCamera.transform.position, 2f);
        // Gizmos.DrawSphere(playerTransform.position, 2f);
        // Gizmos.DrawSphere(transform.position + new Vector3(8f, 0f, -8f), 4f);
    }
}
