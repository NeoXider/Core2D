using UnityEngine;
using System.Collections;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
using System;
using System.Reflection;
#endif

public class TinyScreenCapture1 : MonoBehaviour
{
#if UNITY_EDITOR
    private static TinyScreenCapture1 Instance { get; set; }

    [Header("Prefix for saved file.")] [SerializeField]
    private string _basePath = "/../Assets/TinyScreenCapture/ScreenCapture/";

    [SerializeField] private string _fileName = "screenshoot";
    [SerializeField] private bool _vertical = true;
    [SerializeField] private KeyCode _captureKey = KeyCode.F1;

    [Space] [Header("Resolution settings")] [SerializeField]
    private Vector2Int[] _portraitResolutions =
    {
        new(1320, 2868),
        new(1290, 2786),
        new(2064, 2752),
        new(2048, 2732)
    };

    [SerializeField] private Vector2Int[] _landscapeResolutions =
    {
        new(2868, 1320),
        new(2786, 1290),
        new(2752, 2064),
        new(2732, 2048)
    };

    private Texture2D _texture;
    private Vector2Int _startSize;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        _startSize = Screen.width > Screen.height
            ? new Vector2Int(Screen.width, Screen.height)
            : new Vector2Int(Screen.height, Screen.width);
    }

    private void Update()
    {
        if (Input.GetKeyDown(_captureKey))
            StartCoroutine(TinyCapture());
    }

    private IEnumerator TinyCapture()
    {
        print("Capturing...");

        var canvases = FindObjectsByType<Canvas>(FindObjectsSortMode.None);
        Canvas canvas = null;

        foreach (var canv in canvases)
            if (canv.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                canvas = canv;
                break;
            }

        if (canvas == null)
        {
            Debug.LogError("Canvas not found!");
            yield break;
        }

        var originalCamera = canvas.worldCamera;
        canvas.sortingOrder = 15000;
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = Camera.main;
        var resolutions = _vertical ? _portraitResolutions : _landscapeResolutions;
        for (var i = 0; i < resolutions.Length; i++)
        {
            var width = resolutions[i].x;
            var height = resolutions[i].y;

            var rt = new RenderTexture(width, height, 24);
            Camera.main.targetTexture = rt;
            RenderTexture.active = rt;

            Camera.main.Render();

            _texture = new Texture2D(width, height, TextureFormat.RGB24, false);
            _texture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            _texture.Apply();

            Save(resolutions, i, _texture);

            Camera.main.targetTexture = null;
            RenderTexture.active = null;
            Destroy(rt);
            yield return null;
        }

        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.worldCamera = originalCamera;

        print("End Capturing");
    }

    private Texture2D GetTexture(int width, int height)
    {
        _texture = new Texture2D(width, height, TextureFormat.RGB24, false);
        _texture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        _texture.Apply();
        return _texture;
    }

    private void Save(Vector2Int[] resolutions, int i, Texture2D texture)
    {
        var bytes = texture.EncodeToPNG();
        var timestamp = DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss");
        var basePath = Application.dataPath + _basePath;
        var targetFolderPath = basePath + $"{resolutions[i].x}x{resolutions[i].y}/";
        if (!Directory.Exists(targetFolderPath)) Directory.CreateDirectory(targetFolderPath);
        File.WriteAllBytes(targetFolderPath + $"{_fileName}_{timestamp}.png", bytes);
    }

    // private int GetResolutionIndex(Vector2 resolution, bool vertical)
    // {
    //     Vector2[] resolutions = vertical ? portraitResolutions : landscapeResolutions;
    //     for (int i = 0; i < resolutions.Length; i++)
    //     {
    //         if (resolutions[i] == resolution)
    //             return i;
    //     }
    //     return -1;
    // }


#endif
}