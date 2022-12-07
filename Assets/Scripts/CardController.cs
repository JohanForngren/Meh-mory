using UnityEngine;

public class CardController : MonoBehaviour
{
    [SerializeField] private GameController gameController;

    [SerializeField] private GameObject backGameObject;

    [SerializeField] private SpriteRenderer frontSpriteRenderer;

    public Sprite Sprite
    {
        get => frontSpriteRenderer.sprite;
        set => frontSpriteRenderer.sprite = value;
    }

    public void OnMouseDown()
    {
        if (backGameObject.activeSelf && gameController.UserIsAllowedToRevealCard)
        {
            backGameObject.SetActive(false);
            gameController.OnCardRevealed(this);
        }
    }

    public void Hide()
    {
        backGameObject.SetActive(true);
    }

    public void Remove()
    {
        gameObject.SetActive(false);
    }
}