using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private const int GridRows = 4;
    private const int GridCols = 4;
    private const float OffsetX = 2f;
    private const float OffsetY = 2f;

    [SerializeField] private CardController firstCard;

    [SerializeField] private Sprite[] sprites;

    [SerializeField] private TextMeshProUGUI scoreLabel;

    private CardController _firstRevealedCard;

    private int _score;
    private CardController _secondRevealedCard;

    public bool UserIsAllowedToRevealCard => _secondRevealedCard == null;

    private void Start()
    {
        InstantiateAndPlaceCards();
    }

    private void InstantiateAndPlaceCards()
    {
        var shuffledCardDeck = CreateShuffledCardDeck(sprites);
        PlaceCardsInGrid(shuffledCardDeck);
    }

    private void PlaceCardsInGrid(IReadOnlyList<Sprite> shuffledCardDeck)
    {
        var firstCardPosition = firstCard.transform.position;

        for (var i = 0; i < GridCols; i++)
        for (var j = 0; j < GridRows; j++)
        {
            var cardController = i == 0 && j == 0 ? firstCard : Instantiate(firstCard);

            var index = j * GridCols + i;
            cardController.Sprite = shuffledCardDeck[index];

            var positionX = OffsetX * i + firstCardPosition.x;
            var positionY = OffsetY * j + firstCardPosition.y;
            cardController.transform.position = new Vector3(positionX, positionY, firstCardPosition.z);
        }
    }

    public void OnCardClicked(CardController cardController)
    {
        if (_firstRevealedCard == null)
            _firstRevealedCard = cardController;
        else
            _secondRevealedCard = cardController;
    }

    public void OnCardRevealed()
    {
        if (_secondRevealedCard != null)
            StartCoroutine(CheckIfRevealedCardsMatch());
    }

    private IEnumerator CheckIfRevealedCardsMatch()
    {
        if (_firstRevealedCard.Sprite == _secondRevealedCard.Sprite)
        {
            _score++;
            scoreLabel.text = "Score: " + _score;

            yield return new WaitForSeconds(2f);

            _firstRevealedCard.Remove();
            _secondRevealedCard.Remove();
        }
        else
        {
            _score--;
            scoreLabel.text = "Score: " + _score;

            yield return new WaitForSeconds(2f);

            _firstRevealedCard.Hide();
            _secondRevealedCard.Hide();
        }

        _firstRevealedCard = null;
        _secondRevealedCard = null;
    }

    private static T[] CreateShuffledCardDeck<T>(T[] array)
    {
        return Helpers.KnuthShuffle(array.Concat(array).ToArray());
    }
}