using UnityEngine;
using UnityEngine.UI;

public class UIPaintingInfo : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private Text m_NameText;
    [SerializeField]
    private Text m_ArtistDateText;
    [SerializeField]
    private Text m_TechniqueSizeText;
    [SerializeField]
    private Text m_PriceText;
    #endregion

    #region Monobehaviour
    private void Start()
    {
        PaintingManager.Instance.OnPaintingLoaded += SetPaintingInfo;
    }
    #endregion

    #region Methods
    private void SetPaintingInfo(Painting p)
    {
        PaintingManager.Instance.OnPaintingLoaded -= SetPaintingInfo;

        m_NameText.text = p.Data.name;
        m_ArtistDateText.text = string.Format("{0}, {1}", p.Data.artist, p.Data.date);
        m_TechniqueSizeText.text = string.Format("{0}, {1}x{2}cm", p.Data.technique, p.Data.sizeX, p.Data.sizeY);

        string price = string.Format("{0}PLN", p.Data.price);

        if (p.Data.subscription)
        {
            price = string.Format("{0} / mc", price);
        }

        m_PriceText.text = price;
    }
    #endregion
}