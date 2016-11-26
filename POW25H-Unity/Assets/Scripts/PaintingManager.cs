using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Collections;

public class PaintingManager : MonoBehaviour
{
    #region Variables
    private List<Painting> m_Paintings = new List<Painting>();
    public List<Painting> Paintings { get { return m_Paintings; } }
    #endregion

    #region Monobehaviour
    private void Awake()
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
            LoadPainting(paths[i]);
        }
    }

    private void LoadPainting(string path)
    {
        Painting p = new Painting();
        string directPath = "file://" + path;

        StartCoroutine(LoadPaintingTexture(directPath, p));
        StartCoroutine(LoadPaintingData(directPath, p));

        m_Paintings.Add(p);
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