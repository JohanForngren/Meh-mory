using UnityEngine;

public class CardController : MonoBehaviour
{
    [SerializeField] private GameController gameController;

    [SerializeField] private SpriteRenderer frontSpriteRenderer;

    [SerializeField] private float wrongRotationSpeed = 20f;
    [SerializeField] private float wrongRotationRadius = 0.1f;
    [SerializeField] private float flipSpeed = 200f;
    [SerializeField] private float matchTransformSpeed = 5f;

    [SerializeField] private MeshRenderer cardMeshRenderer;
    [SerializeField] private Material materialIdle;
    [SerializeField] private Material materialWrong;
    [SerializeField] private Material materialMatch;

    private CardState _cardState = CardState.Back;
    private Vector3 _startPosition;
    private float _wrongRotationAngle;

    public Sprite Sprite
    {
        get => frontSpriteRenderer.sprite;
        set => frontSpriteRenderer.sprite = value;
    }

    private void Start()
    {
        _startPosition = transform.position;
    }

    private void Update()
    {
        switch (_cardState)
        {
            case CardState.Back:
                break;
            case CardState.FlippingToFront:
                if (transform.eulerAngles.y + flipSpeed * Time.deltaTime >= 180f)
                {
                    _cardState = CardState.Front;
                    gameController.OnCardRevealed();
                    transform.Rotate(0, 180f - transform.eulerAngles.y, 0);
                }
                else
                {
                    transform.Rotate(0, flipSpeed * Time.deltaTime, 0);
                }

                break;
            case CardState.Front:
                break;
            case CardState.FlippingToBack:
                if (transform.eulerAngles.y + flipSpeed * Time.deltaTime >= 360)
                {
                    _cardState = CardState.Back;
                    transform.Rotate(0, 360 - transform.eulerAngles.y, 0);
                }
                else
                {
                    transform.Rotate(0, flipSpeed * Time.deltaTime, 0);
                }

                break;
            case CardState.Wrong:
                _wrongRotationAngle += wrongRotationSpeed * Time.deltaTime;

                var offset = new Vector3(Mathf.Sin(_wrongRotationAngle), Mathf.Cos(_wrongRotationAngle), 0) *
                             wrongRotationRadius;
                transform.position = _startPosition + offset;
                break;
            case CardState.Match:
                transform.localScale *= 1 + matchTransformSpeed * Time.deltaTime;
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