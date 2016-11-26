using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PaintingSelectionPanel : MonoBehaviour {
    private Texture2D _sourceImage;

    private Sprite _sourceSprite;

    public Image ImageControl;
    public Button ButtonControl;
    public PaintingManager PaintingManager;

    public void Setup(Painting painting)
    {
        _sourceImage = painting.Texture;

        ImageControl.sprite = Sprite.Create(_sourceImage, new Rect(0, 0, _sourceImage.width, _sourceImage.height), new Vector2(0, 0));
        ButtonControl.onClick.AddListener(new UnityEngine.Events.UnityAction(() =>
        {
            PaintingManager.SelectPainting(painting);
        }));
    }

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
