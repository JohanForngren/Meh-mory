using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class CardController : MonoBehaviour
{
    [SerializeField] private GameController gameController;

    [SerializeField] private SpriteRenderer frontSpriteRenderer;

    [SerializeField] private float hoverRotationSpeed = 3f;
    [SerializeField] private float hoverRotationRadius = 0.1f;

    [SerializeField] private float flipSpeed = 5f;
    private float _angle;

    private CardState _cardState = CardState.Back;

    private Vector3 _centre;

    public Sprite Sprite
    {
        get => frontSpriteRenderer.sprite;
        set => frontSpriteRenderer.sprite = value;
    }

    private void Start()
    {
        _centre = transform.position;
        _angle = Random.Range(-1f, 1f);
    }

    private void Update()
    {
        switch (_cardState)
        {
            case CardState.Back:
                break;
            case CardState.FlippingToFront:
                if (transform.eulerAngles.y + flipSpeed >= 180f)
                {
                    _cardState = CardState.Front;
                    gameController.OnCardRevealed();
                    transform.Rotate(0, 180f - transform.eulerAngles.y, 0);
                }
                else
                {
                    transform.Rotate(0, flipSpeed, 0);
                }

                break;
            case CardState.Front:
                break;
            case CardState.FlippingToBack:
                if (transform.eulerAngles.y + flipSpeed >= 360)
                {
                    _cardState = CardState.Back;
                    transform.Rotate(0, 360 - transform.eulerAngles.y, 0);
                }
                else
                {
                    transform.Rotate(0, flipSpeed, 0);
                }

                break;
            case CardState.Removing:
                transform.localScale *= 0.92f;
                if (transform.localScale.y < 0.1f)
                    gameObject.SetActive(false);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void OnMouseDown()
    {
        if (_cardState == CardState.Back && gameController.UserIsAllowedToRevealCard)
        {
            _cardState = CardState.FlippingToFront;
            gameController.OnCardClicked(this);
        }
    }

    public void OnMouseOver()
    {
        _angle += hoverRotationSpeed * Time.deltaTime;

        var offset = new Vector3(Mathf.Sin(_angle), Mathf.Cos(_angle), 0) * hoverRotationRadius;
        transform.position = _centre + offset;
    }

    public void Hide()
    {
        _cardState = CardState.FlippingToBack;
    }

    public void Remove()
    {
        _cardState = CardState.Removing;
    }
}

public enum CardState
{
    Back,
    FlippingToFront,
    Front,
    FlippingToBack,
    Removing
}