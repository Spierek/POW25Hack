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
    public event Action OnFinishedLoading;     // #LS db prepared
    #endregion

    #region Variables
    private int m_CurrentIndex = -1;
    private bool m_FinishedLoading = false;
    private int m_PrevSlideValue = 0;

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
        if (m_FinishedLoading && m_Paintings.Count!=0)
        {
            var newSlideValue = SwipeControl.currentValue;
            if (Input.GetKeyDown(KeyCode.Return)) newSlideValue = m_PrevSlideValue + 1;

            if (m_PrevSlideValue != newSlideValue)
            {
                Debug.Log("Switching due to swipe, value " + SwipeControl.currentValue + " - " + SwipeControl.smoothValue);
                SelectPainting(newSlideValue > m_PrevSlideValue ? m_CurrentIndex + 1 : m_CurrentIndex - 1);
                m_PrevSlideValue = newSlideValue;
            }
        }
    }
    #endregion

    #region Methods
    public void SelectPainting(Painting p)
    {
        SelectPainting(m_Paintings.IndexOf(p));
    }

    public void SelectPainting(int index)
    {
        Debug.Log("Selecting painting " + index);
        m_CurrentIndex = index;
        m_CurrentIndex = (m_CurrentIndex % m_Paintings.Count);

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

        //Not really - should check if all images are loaded in a coroutine instead
        m_FinishedLoading = true;
        if (OnFinishedLoading != null)
        {
            OnFinishedLoading.Invoke();
        }
    }

    private string GetAssetPath()
    {
        string path = string.Empty;
        //#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
        //        path = "file://" + Application.streamingAssetsPath;
        //#elif UNITY_ANDROID
        //        path = "jar:file://" + Application.dataPath + "!/assets/";
        //#endif
        //path = Path.Combine(path, "Paintings");

        path = "http://bluebrick.pl/static_files/PrimeArtEye/";

        return path;
    }

    private IEnumerator LoadPainting(string path)
    {
        Painting p = new Painting();
        Debug.Log("Loading painting from " + path);

        yield return StartCoroutine(LoadPaintingTexture(path, p));
        yield return StartCoroutine(LoadPaintingData(path, p));

        m_Paintings.Add(p);

        if (OnPaintingLoaded != null)
        {
            OnPaintingLoaded.Invoke(p);
            Debug.Log("Painting loaded from " + path);
        }
    }

    private IEnumerator LoadPaintingTexture(string path, Painting painting)
    {
        string imgPath = path + "/img.jpg";
        Debug.Log("Loading image from " + imgPath);

        WWW loader = new WWW(imgPath);

        yield return loader;

        painting.Texture = loader.texture;
    }

    private IEnumerator LoadPaintingData(string path, Painting painting)
    {
        string dataPath = path + "/data.json";
        WWW loader = new WWW(dataPath);

        yield return loader;

        painting.Data = JsonUtility.FromJson<PaintingData>(loader.text);
    }
#endregion
}