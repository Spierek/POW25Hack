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
    private bool m_FinishedLoading = false;

    private List<Painting> m_Paintings = new List<Painting>();
    public List<Painting> Paintings { get { return m_Paintings; } }
    #endregion

    public SwipeControl SwipeControl;

    #region Monobehaviour
    private void Awake()
    {
        m_Instance = this;
    }

    private void Start()
    {
        StartCoroutine(LoadPaintingDatabase());
    }

    private void Update()
    {
        if (m_FinishedLoading)
        {
            var newIndex = (SwipeControl.currentValue % m_Paintings.Count);
            if (Input.GetKeyDown(KeyCode.Return)) newIndex = m_CurrentIndex+1;

            if(m_CurrentIndex != newIndex)
                GetNewPainting(newIndex);
        }
    }
    #endregion

    #region Methods
    private void GetNewPainting(int index)
    {
        m_CurrentIndex = index;

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

    private IEnumerator LoadPaintingDatabase()
    {
        string path = GetAssetPath();

        // get directories
        string dirPath = Path.Combine(path, "paintinglist.txt");
        WWW loader = new WWW(dirPath);

        yield return loader;

        string[] paths = loader.text.Split(';');

        for (int i = 0; i < paths.Length; ++i)
        {
            string paintingPath = Path.Combine(path, paths[i]);
            StartCoroutine(LoadPainting(paintingPath));
        }

        m_FinishedLoading = true;
    }

    private string GetAssetPath()
    {
        string path = string.Empty;
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
        path = "file://" + Application.streamingAssetsPath;
#elif UNITY_ANDROID
        path = "jar:file://" + Application.dataPath + "!/assets/";
#endif
        path = Path.Combine(path, "Paintings");

        return path;
    }

    private IEnumerator LoadPainting(string path)
    {
        Painting p = new Painting();

        yield return StartCoroutine(LoadPaintingTexture(path, p));
        yield return StartCoroutine(LoadPaintingData(path, p));

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