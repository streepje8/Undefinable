using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal TwinPortal;
    public float headSpace = 0.2f;
    public Camera cullingCamera;
    private Camera cam;
    private MeshRenderer rend;
    private Material mat;
    private RenderTexture rt;
    void Start()
    {
        cam = GetComponentInChildren<Camera>();
        rend = GetComponentInChildren<MeshRenderer>();
        mat = new Material(rend.material);
        rt = new RenderTexture(Screen.height,Screen.width,1,RenderTextureFormat.ARGB32);
        mat.SetTexture("_MainTex", rt);
        rend.material = mat;
    }
    private bool setTexture = false;
    void Update()
    {
        if(!setTexture && cam != null)
        {
            cam.targetTexture?.Release();
            cam.targetTexture = rt;
            setTexture = true;
        }
        cam.gameObject.SetActive(rend.isVisible);


        /*
        Vector3 playerOffsetFromPortal = GameController.Instance.mainCamera.transform.position - transform.position;
        cam.transform.position = TwinPortal.transform.position + playerOffsetFromPortal; //TwinPortal.transform.rotation *   

        float angularDifferenceBetweenPortalRotations = Quaternion.Angle(transform.rotation, TwinPortal.transform.rotation);

        Quaternion portalRotationalDifference = Quaternion.AngleAxis(angularDifferenceBetweenPortalRotations, Vector3.up);
        Vector3 newCameraDirection = portalRotationalDifference * GameController.Instance.mainCamera.transform.forward;
        cam.transform.rotation = GameController.Instance.mainCamera.transform.rotation;
        */

        cam.projectionMatrix = GameController.Instance.mainCamera.cam.projectionMatrix;
        

        Vector3 relativePosition = transform.InverseTransformPoint(GameController.Instance.mainCamera.transform.position);
        relativePosition = Vector3.Scale(relativePosition, new Vector3(-1, 1, -1));
        cam.transform.position = TwinPortal.transform.TransformPoint(relativePosition);

        /*
        Vector3 relativeRotation = transform.InverseTransformDirection(GameController.Instance.mainCamera.transform.forward);
        relativeRotation = Vector3.Scale(relativeRotation, new Vector3(-1, 1, -1));
        cam.transform.forward = TwinPortal.transform.TransformDirection(relativeRotation);
        */
        cam.transform.rotation = GameController.Instance.mainCamera.transform.rotation;

        cam.cullingMatrix = cullingCamera.projectionMatrix * cullingCamera.worldToCameraMatrix;
        //cam.nearClipPlane = Mathf.Clamp(Vector3.Distance(cam.transform.position, TwinPortal.transform.position) - headSpace,0.000000001f,1000f);
    }
}
