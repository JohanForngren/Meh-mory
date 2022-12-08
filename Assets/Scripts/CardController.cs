using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class CardController : MonoBehaviour
{
    [SerializeField] private GameController gameController;

    [SerializeField] private SpriteRenderer frontSpriteRenderer;

    [SerializeField] private float hoverRotationSpeed = 10f;
    [SerializeField] private float hoverRotationRadius = 0.1f;

    [SerializeField] private float flipSpeed = 5f;
    private float _angle;

    private CardState _cardState = CardState.Back;

    private Vector3 _centre;

    [SerializeField] private MeshRenderer cardMeshRenderer;
    
    [SerializeField] private Material materialIdle;
    [SerializeField] private Material materialWrong;
    [SerializeField] private Material materialMatch;

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
            case CardState.Wrong:
                _angle += hoverRotationSpeed * Time.deltaTime;

                var offset = new Vector3(Mathf.Sin(_angle), Mathf.Cos(_angle), 0) * hoverRotationRadius;
                transform.position = _centre + offset;
                break;
            case CardState.Match:
                transform.localScale *= 1.02f;
                break;
            case CardState.Removed:
            default: break;
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

    public void WrongMatch()
    {
        cardMeshRenderer.material = materialWrong;
        _cardState = CardState.Wrong;
    }

    public void Hide()
    {
        cardMeshRenderer.material = materialIdle;
        _cardState = CardState.FlippingToBack;
    }

    public void Match()
    {
        _cardState = CardState.Match;
        cardMeshRenderer.material = materialMatch;
    }
    public void Remove()
    {
        _cardState = CardState.Removed;
        gameObject.SetActive(false);
    }
}

public enum CardState
{
    Back,
    FlippingToFront,
    Front,
    FlippingToBack,
    Wrong,
    Match,
    Removed
}