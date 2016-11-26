using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using System;

public class PaintingManager : MonoBehaviour
{
    #region Singleton
    private static PaintingManager m_Instance;
    public static PaintingManager Instance { get { return m_Instance; } }
    #endregion

    #region Events
    public delegate void PaintingChangeEvent(Painting p);
    public event PaintingChangeEvent OnPaintingLoaded;      // #LS first load
    public event PaintingChangeEvent OnPaintingChanged;     // #LS change request
    #endregion

    #region Variables
    private int m_CurrentIndex = -1;

    private List<Painting> m_Paintings = new List<Painting>();
    public List<Painting> Paintings { get { return m_Paintings; } }
    #endregion

    #region Monobehaviour
    private void Awake()
    {
        m_Instance = this;
    }

    private void Start()
    {
        LoadPaintingDatabase();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || (Input.touchCount > 0 && Input.GetTouch(0).tapCount > 0))
        {
            GetNewPainting();
        }
    }
    #endregion

    #region Methods
    private void GetNewPainting()
    {
        m_CurrentIndex++;
        if (m_CurrentIndex > m_Paintings.Count - 1)
        {
            m_CurrentIndex = 0;
        }

        if (OnPaintingChanged != null)
        {
            OnPaintingChanged.Invoke(m_Paintings[m_CurrentIndex]);
        }
    }

    private void LoadPaintingDatabase()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Paintings");
        string[] paths = Directory.GetDirectories(path);

        for (int i = 0; i < paths.Length; ++i)
        {
            StartCoroutine(LoadPainting(paths[i]));
        }
    }

    private IEnumerator LoadPainting(string path)
    {
        Painting p = new Painting();
        string directPath = "file://" + path;

        yield return StartCoroutine(LoadPaintingTexture(directPath, p));
        yield return StartCoroutine(LoadPaintingData(directPath, p));

        m_Paintings.Add(p);

        if (OnPaintingLoaded != null)
        {
            OnPaintingLoaded.Invoke(p);
        }
    }

    private IEnumerator LoadPaintingTexture(string path, Painting painting)
    {
        string imgPath = Path.Combine(path, "img.jpg");
        WWW loader = new WWW(imgPath);

        yield return loader;

        painting.Texture = loader.texture;
    }

    private IEnumerator LoadPaintingData(string path, Painting painting)
    {
        string dataPath = Path.Combine(path, "data.json");
        WWW loader = new WWW(dataPath);

        yield return loader;

        painting.Data = JsonUtility.FromJson<PaintingData>(loader.text);
    }
    #endregion
}