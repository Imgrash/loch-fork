using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PortalVisuals : MonoBehaviour
{
    public PortalVisuals linkedPortal;
    public MeshRenderer screen;
    public Camera playerCam;
    public Camera portalCam;
    RenderTexture viewTexture;
    public Transform emptyKid;

    private void Awake()
    {
        //screen = GetComponent<MeshRenderer>();
        //portalCam = GetComponentInChildren<Camera>();
        /*portalCam.enabled = false;
        Render();
        portalCam.enabled = true;*/
    }


    void CreateViewTexture()
    {
        if(viewTexture == null || viewTexture.width != Screen.width || viewTexture.height != Screen.height)
        {
            if(viewTexture != null)
            {
                viewTexture.Release();
            }
            viewTexture = new RenderTexture(Screen.width, Screen.height, 24);
            portalCam.targetTexture = viewTexture;
            linkedPortal.screen.material.SetTexture("_MainTex", viewTexture);
        }
    }

    static bool VisibleFromCamera(Renderer renderer, Camera camera)
    {
        Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(camera);
        return GeometryUtility.TestPlanesAABB(frustumPlanes, renderer.bounds);
    }

    public int recursionLimit = 10;
    public float maxRenderDistance = 150f;
    public void Render2()
    {
        if (!VisibleFromCamera(linkedPortal.screen, playerCam))
            return;
        CreateViewTexture();

        Matrix4x4 localToWorldMatrix = playerCam.transform.localToWorldMatrix;
        Matrix4x4[] matrices = new Matrix4x4[recursionLimit];
        for (int i = 0; i < recursionLimit; i++)
        {
            localToWorldMatrix = transform.localToWorldMatrix * linkedPortal.emptyKid.transform.worldToLocalMatrix * localToWorldMatrix;
            matrices[recursionLimit - i - 1] = localToWorldMatrix;
        }

        screen.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;

        for (int i = 0; i < recursionLimit; i++)
        {
            try
            {
                if (Vector3.Distance(matrices[i].GetColumn(3), linkedPortal.emptyKid.transform.position) > maxRenderDistance)
                {
                    Debug.Log($"portalCam pos: {portalCam.transform.position}");
                    continue;
                }
                portalCam.transform.SetPositionAndRotation(matrices[i].GetColumn(3), matrices[i].rotation);
                SetNearClipPlane();
                portalCam.Render();
                //добавить проверку - виден ли рендерящийся кусок, может, поможет исправить screen position out of view frustum
            }
            catch//(Exception e)
            {
                //Debug.Log(e.Message);
            }
        }
        screen.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
    }

    public void Render()
    {
        if(!VisibleFromCamera(linkedPortal.screen, playerCam))
            return;
        screen.enabled = false;
        //linkedPortal.screen.enabled = false;
        CreateViewTexture();
        var m = transform.localToWorldMatrix * linkedPortal.emptyKid.transform.worldToLocalMatrix * playerCam.transform.localToWorldMatrix;
        portalCam.transform.SetPositionAndRotation(m.GetColumn(3), m.rotation);
        //portalCam.transform.Rotate(linkedPortal.transform.up * 180);
        SetNearClipPlane();
        portalCam.Render();

        //screen.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        //linkedPortal.screen.enabled = true;
        screen.enabled = true;
        //linkedPortal.screen.enabled = true;
    }
    void SetNearClipPlane()
    {
        Transform clipPlane = emptyKid;
        int dot = System.Math.Sign(Vector3.Dot(clipPlane.forward, transform.position - portalCam.transform.position));

        Vector3 camSpacePos = portalCam.worldToCameraMatrix.MultiplyPoint(clipPlane.position);
        Vector3 camSpaceNormal = portalCam.worldToCameraMatrix.MultiplyVector(clipPlane.forward) * dot;
        float camSpaceDst = -Vector3.Dot(camSpacePos, camSpaceNormal);
        Vector4 clipPlaneCameraSpace = new Vector4(camSpaceNormal.x, camSpaceNormal.y, camSpaceNormal.z, camSpaceDst);

        portalCam.projectionMatrix = playerCam.CalculateObliqueMatrix(clipPlaneCameraSpace);

        Debug.DrawLine(transform.position, transform.position + camSpaceNormal * 20);

        //DrawPlane(transform.position, camSpaceNormal);

    }

    public void DrawPlane(Vector3 position, Vector3 normal)
    {
        Vector3 v3;

        if (normal.normalized != Vector3.forward)
            v3 = Vector3.Cross(normal, Vector3.forward).normalized * normal.magnitude;
        else
            v3 = Vector3.Cross(normal, Vector3.up).normalized * normal.magnitude;

        var corner0 = position + v3;
        var corner2 = position - v3;
        var q = Quaternion.AngleAxis(90.0f, normal);
        v3 = q * v3;
        var corner1 = position + v3;
        var corner3 = position - v3;

        Debug.DrawLine(corner0, corner2, Color.green);
        Debug.DrawLine(corner1, corner3, Color.green);
        Debug.DrawLine(corner0, corner1, Color.green);
        Debug.DrawLine(corner1, corner2, Color.green);
        Debug.DrawLine(corner2, corner3, Color.green);
        Debug.DrawLine(corner3, corner0, Color.green);
        Debug.DrawRay(position, normal, Color.red);
    }
    private void Update()
    {
        Render2();
        Debug.DrawLine(this.transform.position, this.transform.position + this.transform.forward * 10, Color.green);
    }
}
