using UnityEngine;
using System.Collections;

public class ArmourSpritesRenderer : MonoBehaviour {
    private NewArmourSprites armourSprites;
    [SerializeField]
    private SpriteRenderer armourRenderer, helmetRenderer;
    [SerializeField]
    private Sprite _ArmourSprite, _HelmetSprite;

    void Start()
    {
        armourSprites = this.gameObject.GetComponent<NewArmourSprites>();
        armourSprites.GenerateNewSprites();
        SetSprites();
    }

    public void TransferSprites(NewArmourSprites other)
    {
        armourSprites.SetSpriteSheetName(other.GetSpriteSheetName());
        armourSprites.SetArmourSprite(other.GetArmourSprite());
        armourSprites.SetHelmetSprite(other.GetHelmetSprite());
        SetSprites();
    }

    private void SetSprites()
    {
        _ArmourSprite = armourSprites.GetArmourSprite();
        _HelmetSprite = armourSprites.GetHelmetSprite();
        RenderSprites();
    }

    private void RenderSprites()
    {
        armourRenderer.sprite = _ArmourSprite;
        helmetRenderer.sprite = _HelmetSprite;
    }
}
