using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal TwinPortal;
    public List<Portal> visiblePortals = new List<Portal>();
    private MeshRenderer rend;
    private Material mat;
    private RenderTexture rt;
    private readonly Matrix4x4 cullingProjectionMatrix = Matrix4x4.Perspective(60, 1, 1.95f, 1000f);
    private Matrix4x4 cullingWorldToCameraMatrix;
    private List<Teleportable> ableToTeleport = new List<Teleportable>();
    
    void Start()
    {
        rend = GetComponentInChildren<MeshRenderer>();
        mat = new Material(rend.material);
        rt = new RenderTexture(Screen.height,Screen.width,1,RenderTextureFormat.ARGB32);
        mat.SetTexture("_MainTex", rt); //Causes the screen melt effect on the other end of the portal, i think this is cooler than having it be pure black
        rend.material = mat;
        RecalculateWorldToCameraMatrix();
    }

    //All the code in the update is related to rendering the portals
    void Update()
    {
        if(rend.isVisible)
        {
            RenderTexture(PortalManager.Instance.mainCamera.transform.position, PortalManager.Instance.mainCamera.transform.rotation, PortalManager.Instance.mainCamera, PortalManager.Instance.recursionCount);
        }
    }


    //All the code in the fixedupdate is related to teleporting the objects inside of the portal
    private void FixedUpdate()
    {
        if(rend.isVisible) //You can't teleport in portals you can't see, if this becomes an issue, remove this line
        {
            for (int i = ableToTeleport.Count - 1; i >= 0; i--)
            {
                Teleportable teleportable = ableToTeleport[i];
                if (Vector3.Dot(transform.forward, (teleportable.transform.position - transform.position).normalized) > Mathf.Epsilon) //The object has moved through the portal
                {
                    Vector3 altLocation = TransformPositionBetweenPortals(this, TwinPortal, teleportable.transform.position);
                    Quaternion altRotation = TransformRotationBetweenPortals(this, TwinPortal, teleportable.transform.rotation);
                    teleportable.transform.position = altLocation;
                    teleportable.transform.rotation = altRotation;
                    teleportable.OnTeleport(this, TwinPortal, TransformPositionBetweenPortals(this, TwinPortal, Vector3.zero), TransformRotationBetweenPortals(this, TwinPortal, Quaternion.identity));
                    ITeleportListener[] bois = teleportable.GetComponentsInChildren<ITeleportListener>();
                    for (int j = 0; j < bois.Length; j++)
                    {
                        bois[j].OnTeleport(this, TwinPortal);
                    }
                    ableToTeleport.Remove(teleportable);
                }
            }
        }   
    }

    /// <summary>
    /// This function renders the texture on to the portal from a specific perspective
    /// </summary>
    /// <param name="campos">The camera position</param>
    /// <param name="camrot">The camera rotation</param>
    /// <param name="pcam">The portal camera instance off the camera</param>
    /// <param name="depth">the depth you want to render with</param>
    public void RenderTexture(Vector3 campos, Quaternion camrot, PortalCamera pcam, int depth)
    {
        if (depth <= 0) //So we don't get stuck in an infinite loop when two portals face eachother
            return;
        
        //Render the other portal visible inside of me
        Camera cam = PortalManager.Instance.portalRenderCamera.cam;
        cam.enabled = true;
        //Calculate the camera position of our camera in the cordinate reference frame of the other portal
        Vector3 relativePosition = transform.InverseTransformPoint(campos);
        relativePosition = Vector3.Scale(relativePosition, new Vector3(-1, 1, -1));
        cam.transform.position = TwinPortal.transform.TransformPoint(relativePosition);
        cam.transform.rotation = (Quaternion.Euler(0, 180, 0) * TwinPortal.transform.rotation) * Quaternion.Inverse(transform.rotation) * camrot;
        foreach (Portal p in TwinPortal.visiblePortals)
        {
            p.RenderTexture(cam.transform.position,cam.transform.rotation,pcam, depth - 1);
        }

        //Render my own texture
        cam.enabled = true;
        cam.projectionMatrix = pcam.cam.projectionMatrix; //Makes the perspective match
        //Calculate where the camera is supposed to be on the other end of the portal
        relativePosition = transform.InverseTransformPoint(campos);
        relativePosition = Vector3.Scale(relativePosition, new Vector3(-1, 1, -1));
        cam.transform.position = TwinPortal.transform.TransformPoint(relativePosition);
        cam.transform.rotation = (Quaternion.Euler(0, 180, 0) * TwinPortal.transform.rotation) * Quaternion.Inverse(transform.rotation) * camrot;
        RecalculateWorldToCameraMatrix(); //Since we moved are render camera we have to recalculate our culling matrix and apply it
        cam.cullingMatrix = cullingProjectionMatrix * cullingWorldToCameraMatrix;
        PoolItem texture = PortalManager.Instance.GetTexture(); //Get an unused texture from the pool (this is faster than creating a new one and deleting it every frame)
        cam.targetTexture = texture.Texture;
        cam.Render(); //Actually render the camera on to a texture
        mat.SetTexture("_MainTex", texture.Texture); //Apply the texture to the portal
        cam.targetTexture = null;   
        cam.enabled = false;
    }

    private void OnDisable()
    {
        rt?.Release(); //We don't want to keep the render texture locked to this object if its gone
    }

    //Detect if something is in the portal///////////

    private void OnTriggerEnter(Collider other)
    {
        Teleportable tel = other.GetComponent<Teleportable>();
        if(tel != null)
            ableToTeleport.Add(tel);
    }
    
    private void OnTriggerExit(Collider other)
    {
        Teleportable tel = other.GetComponent<Teleportable>();
        if (tel != null && ableToTeleport.Contains(tel))
        {
            ableToTeleport.Remove(tel);
        }
    }

    /////////////////////////////////////////////////

    /// <summary>
    /// This function is called to recalculate the culling matrix when the camera is moved
    /// </summary>
    void RecalculateWorldToCameraMatrix()
    {
        Matrix4x4 mat = TwinPortal.transform.localToWorldMatrix;
        Vector3 offset = TwinPortal.transform.rotation * new Vector3(0, -1.82f, 2.5f);
        mat.m03 += offset.x;
        mat.m13 += offset.y;
        mat.m23 += offset.z;
        cullingWorldToCameraMatrix = Matrix4x4.Inverse(mat);
    }

    /// <summary>
    /// Calculates what a position would be like on the other end of a portal
    /// </summary>
    /// <param name="sender">the entry portal</param>
    /// <param name="target">the exit portal</param>
    /// <param name="position">the point you would like to translate</param>
    /// <returns>the translated point</returns>
    public static Vector3 TransformPositionBetweenPortals(Portal sender, Portal target, Vector3 position)
    {
        return target.transform.position + target.transform.rotation * (Quaternion.Inverse(sender.transform.rotation) * (position - sender.transform.position)); //position - sender.transform.position
    }

    /// <summary>
    /// Calculates what a rotation would be like on the other end of a portal
    /// </summary>
    /// <param name="sender">the entry portal</param>
    /// <param name="target">the exit portal</param>
    /// <param name="rotation">the rotation you would like to translate</param>
    /// <returns>the translated rotation</returns>
    public static Quaternion TransformRotationBetweenPortals(Portal sender, Portal target, Quaternion rotation)
    {
        return
            Quaternion.Euler(0,180,0) * (target.transform.rotation *
            (Quaternion.Inverse(sender.transform.rotation) *
            rotation));
    }
}
