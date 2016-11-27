using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using MaterialUI;

public class PaintingSelectionPanel : MonoBehaviour {
    private Texture2D _sourceImage;
    private Sprite _sourceSprite;
    private Painting _painting;

    public Image ImageControl;
    public Button ButtonControl;
    public PaintingManager PaintingManager;
    public ScreenView ScreenViewManager;

    public void Setup(Painting painting, ScreenView screenViewManager)
    {
        _painting = painting;
        _sourceImage = painting.Texture;
        ScreenViewManager = screenViewManager;

        ImageControl.sprite = Sprite.Create(_sourceImage, new Rect(0, 0, _sourceImage.width, _sourceImage.height), new Vector2(0, 0));
        ButtonControl.onClick.AddListener(new UnityEngine.Events.UnityAction(() =>
        {
            PaintingManager.SelectPainting(_painting);
            ScreenViewManager.Back(ScreenView.Type.Out);
        }));
    }

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
