using UnityEngine;
using System.Collections.Generic;

public class PaintingDisplay : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private MeshRenderer m_Renderer;

    private Material m_Material;
    #endregion

    #region Monobehaviour
    private void Awake()
    {
        if (m_Renderer == null)
        {
            m_Renderer = GetComponent<MeshRenderer>();
        }

        m_Material = m_Renderer.material;
    }

    private void Start()
    {
        PaintingManager.Instance.OnPaintingLoaded += SetNewPainting;
        PaintingManager.Instance.OnPaintingChanged += SetNewPainting;
    }

    private void OnDestroy()
    {
        PaintingManager.Instance.OnPaintingLoaded -= SetNewPainting;
        PaintingManager.Instance.OnPaintingChanged -= SetNewPainting;
    }
    #endregion

    #region Methods
    public void SetNewPainting(Painting p)
    {
        PaintingManager.Instance.OnPaintingLoaded -= SetNewPainting;

        m_Material.mainTexture = p.Texture;
        transform.localScale = new Vector3(p.Data.sizeX / 50f, transform.localScale.y, p.Data.sizeY / 50f);
    }
    #endregion
}