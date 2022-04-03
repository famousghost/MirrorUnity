using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CreateReflectionCamera : MonoBehaviour
{
    #region Inspector Variables
    [SerializeField] private Camera _MainCamera;
    [SerializeField] private Camera _ReflCamera;
    [SerializeField] private Transform _Plane;
    [SerializeField] private RenderTexture _ReflectionTexture;
    [SerializeField] private Texture2D _MirrorTexture;
    [SerializeField] private Material _MirrorMaterial;

    #endregion Inspector Variables

    #region Unity Mehtods
    private void Start()
    {
        _ReflCamera = ReflectionCamera(_MainCamera);

        _ReflectionTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGBFloat)
        {
            enableRandomWrite = true
        };

    }

    private void Update()
    {
        
    }

    private void OnPostRender()
    {
        SetupReflectionCamera();
    }
    #endregion Unity Methods

    #region Private Variables
    private static readonly int _RelfectionCameraTextureId = Shader.PropertyToID("_MainTex");
    private static readonly int _MirrorTextureId = Shader.PropertyToID("_MirrorTexture");
    #endregion Private Variables

    #region Private Methods
    private Camera ReflectionCamera(Camera main)
    {
        var go = new GameObject();

        go.AddComponent<Camera>();

        go.name = "ReflCamera";

        Camera reflCamera = go.GetComponent<Camera>();

        reflCamera.enabled = false;

        return reflCamera;
    }

    private void SetupReflectionCamera()
    {
        _ReflCamera.CopyFrom(_MainCamera);

        _ReflCamera.cullingMask = 1 << 0;
        var pos = _ReflCamera.transform.position;
        var dir = _ReflCamera.transform.forward;
        var up = _ReflCamera.transform.up;

        Vector3 lpos = _Plane.transform.InverseTransformPoint(pos);
        Vector3 ldir = _Plane.transform.InverseTransformDirection(dir);
        Vector3 lup = _Plane.transform.InverseTransformDirection(up);

        lpos = Vector3.Reflect(lpos, Vector3.up);

        ldir = Vector3.Reflect(ldir, Vector3.up);
        //ldir = Vector3.Reflect(ldir, Vector3.right);

        lup = Vector3.Reflect(lup, Vector3.up);
        //lup = Vector3.Reflect(lup, Vector3.right);

        pos = _Plane.transform.TransformPoint(lpos);
        dir = _Plane.transform.TransformDirection(ldir);
        lup = _Plane.transform.TransformDirection(lup);

        _ReflCamera.transform.position = pos;
        _ReflCamera.transform.LookAt(pos + dir, lup);

        _ReflCamera.projectionMatrix = _ReflCamera.projectionMatrix * Matrix4x4.Scale(new Vector3(-1, 1, 1));
        GL.SetRevertBackfacing(true);
        //_ReflCamera.transform.right = Vector3.Cross(-dir, lup);


        //_ReflCamera.transform.parent = _MainCamera.transform;

        _ReflCamera.targetTexture = _ReflectionTexture;

        _ReflCamera.Render();

        GL.SetRevertBackfacing(false);

        _MirrorMaterial.SetTexture(_RelfectionCameraTextureId, _ReflectionTexture);
        _MirrorMaterial.SetTexture(_MirrorTextureId, _MirrorTexture);
    }

    #endregion Private Methods
}
