using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Portal : MonoBehaviour
{
    public Portal TwinPortal;
    public List<Portal> visiblePortals = new List<Portal>();
    private MeshRenderer rend;
    private Material mat;
    private RenderTexture rt;
    private readonly Matrix4x4 cullingProjectionMatrix = Matrix4x4.Perspective(60, 1, 1.95f, 1000f);
    private Matrix4x4 cullingWorldToCameraMatrix;
    private BoxCollider pCollider;
    private List<Teleportable> ableToTeleport = new List<Teleportable>();
    void Start()
    {
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
        if(rend.isVisible)
        {
            RenderTexture(GameController.Instance.mainCamera.transform.position, GameController.Instance.mainCamera.transform.rotation,GameController.Instance.mainCamera, PortalManager.Instance.recursionCount);
            for (int i = ableToTeleport.Count - 1; i >= 0; i--)
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

        //In the RecalculateWorldToCameraMatrix function:
        //Matrix4x4.TRS(
        //TwinPortal.transform.position + (), //FLIP THE Y AXIS BECAUSE UNITY HAS Y POSITIVE IS UP BUT OPENGL HAS Y POSITIVE IS DOWN
        //  Quaternion.identity,
        //  new Vector3(1, 1, -1)
        //)
        #endregion
    }

    private void LateUpdate()
    {
        PortalManager.Instance.ReleaseAllTextures();
    }

    public void RenderTexture(Vector3 campos, Quaternion camrot, PortalCamera pcam, int depth)
    {
        if (depth <= 0)
            return;
        
        //Render others
        Camera cam = PortalManager.Instance.portalRenderCamera.cam;
        cam.enabled = true;
        Vector3 relativePosition = transform.InverseTransformPoint(campos);
        relativePosition = Vector3.Scale(relativePosition, new Vector3(-1, 1, -1));
        cam.transform.position = TwinPortal.transform.TransformPoint(relativePosition);
        cam.transform.rotation = (Quaternion.Euler(0, 180, 0) * TwinPortal.transform.rotation) * Quaternion.Inverse(transform.rotation) * camrot;
        foreach (Portal p in TwinPortal.visiblePortals)
        {
            p.RenderTexture(cam.transform.position,cam.transform.rotation,pcam, depth - 1);
        }

        //Render self
        cam.enabled = true;
        cam.projectionMatrix = pcam.cam.projectionMatrix;
        relativePosition = transform.InverseTransformPoint(campos);
        relativePosition = Vector3.Scale(relativePosition, new Vector3(-1, 1, -1));
        cam.transform.position = TwinPortal.transform.TransformPoint(relativePosition);
        cam.transform.rotation = (Quaternion.Euler(0, 180, 0) * TwinPortal.transform.rotation) * Quaternion.Inverse(transform.rotation) * camrot;
        RecalculateWorldToCameraMatrix();
        cam.cullingMatrix = cullingProjectionMatrix * cullingWorldToCameraMatrix;
        PoolItem texture = PortalManager.Instance.GetTexture();
        cam.targetTexture = texture.Texture;
        cam.Render();
        mat.SetTexture("_MainTex", texture.Texture);
        cam.targetTexture = null;   
        cam.enabled = false;
    }

    private void OnDisable()
    {
        rt?.Release();
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
        Matrix4x4 mat = TwinPortal.transform.localToWorldMatrix;
        Vector3 offset = TwinPortal.transform.rotation * new Vector3(0, -1.82f, 2.5f);
        mat.m03 += offset.x;
        mat.m13 += offset.y;
        mat.m23 += offset.z;
        cullingWorldToCameraMatrix = Matrix4x4.Inverse(mat);
    }
}
