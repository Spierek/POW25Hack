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
    public delegate void PaintingLoadEvent(Painting p);
    public event PaintingLoadEvent OnPaintingLoaded;
    #endregion

    #region Variables
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
    #endregion

    #region Methods
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