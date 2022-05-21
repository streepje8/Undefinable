using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Portal : MonoBehaviour
{
    public Portal TwinPortal;
    public Camera temp;
    
    private Camera cam;
    private MeshRenderer rend;
    private Material mat;
    private RenderTexture rt;
    private readonly Matrix4x4 cullingProjectionMatrix = Matrix4x4.Perspective(60, 1, 1.95f, 1000f);
    private Matrix4x4 culllingWorldToCameraMatrix;
    private BoxCollider pCollider;
    private List<Teleportable> ableToTeleport = new List<Teleportable>();
    void Start()
    {
        cam = GetComponentInChildren<Camera>();
        rend = GetComponentInChildren<MeshRenderer>();
        mat = new Material(rend.material);
        rt = new RenderTexture(Screen.height,Screen.width,1,RenderTextureFormat.ARGB32);
        mat.SetTexture("_MainTex", rt);
        rend.material = mat;
        RecalculateWorldToCameraMatrix();
        pCollider = GetComponent<BoxCollider>();
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
        cam.projectionMatrix = GameController.Instance.mainCamera.cam.projectionMatrix;
        Vector3 relativePosition = transform.InverseTransformPoint(GameController.Instance.mainCamera.transform.position);
        relativePosition = Vector3.Scale(relativePosition, new Vector3(-1, 1, -1));
        cam.transform.position = TwinPortal.transform.TransformPoint(relativePosition);
        cam.transform.rotation = (Quaternion.Euler(0,180,0) * TwinPortal.transform.rotation) * Quaternion.Inverse(transform.rotation) * GameController.Instance.mainCamera.transform.rotation;
        RecalculateWorldToCameraMatrix();
        cam.cullingMatrix = cullingProjectionMatrix * culllingWorldToCameraMatrix;
        if(rend.isVisible)
        {
            for(int i = 0; i < ableToTeleport.Count; i++)
            {
                Teleportable teleportable = ableToTeleport[i];
                Vector3 altLocation = TwinPortal.transform.position + (TwinPortal.transform.rotation * (teleportable.transform.position - transform.position));
                if(Vector3.Dot(teleportable.direction.normalized, -(teleportable.transform.position - transform.position).normalized) > 0f) //Player moving towards this portal
                {
                    CharacterController cont = teleportable.GetComponent<CharacterController>();
                    if (cont != null)
                    {
                        cont.enabled = false;
                        teleportable.transform.position = altLocation;
                        cont.enabled = true;
                    }
                    else 
                    { 
                        teleportable.transform.position = altLocation;
                    }
                    ableToTeleport.Remove(teleportable);
                }
            }
        }
        Debug.DrawLine(transform.position, transform.position + PortalManager.Instance.teleportables[0].direction.normalized * 3, Color.green);
        Debug.DrawLine(transform.position, transform.position + -(PortalManager.Instance.teleportables[0].transform.position - transform.position).normalized * 3, Color.blue);
        #region COMMENTS
        //Failed attempts
        /*
        Vector3 playerOffsetFromPortal = GameController.Instance.mainCamera.transform.position - transform.position;
        cam.transform.position = TwinPortal.transform.position + playerOffsetFromPortal; //TwinPortal.transform.rotation *   

        float angularDifferenceBetweenPortalRotations = Quaternion.Angle(transform.rotation, TwinPortal.transform.rotation);

        Quaternion portalRotationalDifference = Quaternion.AngleAxis(angularDifferenceBetweenPortalRotations, Vector3.up);
        Vector3 newCameraDirection = portalRotationalDifference * GameController.Instance.mainCamera.transform.forward;
        cam.transform.rotation = GameController.Instance.mainCamera.transform.rotation;
        */
        /*
        Vector3 relativeRotation = transform.InverseTransformDirection(GameController.Instance.mainCamera.transform.forward);
        relativeRotation = Vector3.Scale(relativeRotation, new Vector3(-1, 1, -1));
        cam.transform.forward = TwinPortal.transform.TransformDirection(relativeRotation);
        */
        //cam.nearClipPlane = Mathf.Clamp(Vector3.Distance(cam.transform.position, TwinPortal.transform.position) - headSpace,0.000000001f,1000f);
        #endregion
    }

    private void OnTriggerEnter(Collider other)
    {
        Teleportable tel = other.GetComponent<Teleportable>();
        if(tel != null)
        {
            ableToTeleport.Add(tel);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Teleportable tel = other.GetComponent<Teleportable>();
        if (tel != null)
        {
            if(ableToTeleport.Contains(tel))
                ableToTeleport.Remove(tel);
        }
    }

    void RecalculateWorldToCameraMatrix()
    {
        culllingWorldToCameraMatrix = Matrix4x4.Inverse(Matrix4x4.TRS(
          TwinPortal.transform.position + (TwinPortal.transform.rotation * new Vector3(0, -1.82f, 1.93f)), //FLIP THE Y AXIS BECAUSE UNITY HAS Y POSITIVE IS UP BUT OPENGL HAS Y POSITIVE IS DOWN
          Quaternion.identity,
          new Vector3(1, 1, -1)
        ));
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.75f, 0.0f, 0.0f, 0.75f);

        // Convert the local coordinate values into world
        // coordinates for the matrix transformation.
        Gizmos.matrix = transform.localToWorldMatrix * culllingWorldToCameraMatrix;
        Gizmos.DrawCube(Vector3.forward * 2, Vector3.one);
    }

}
