using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private int gridRows = 4;
    [SerializeField] private int gridCols = 4;
    [SerializeField] private float gridOffsetX = 1.9f;
    [SerializeField] private float gridOffsetY = 1.9f;

    [SerializeField] private CardController firstCard;
    [SerializeField] private Sprite[] cardSprites;
    [SerializeField] private Text scoreLabel;
    [SerializeField] private SoundController soundController;

    private int _cardsStillInPlay;
    private CardController _firstRevealedCard;
    private int _numberOfFLippedCards;
    private int _score;
    private CardController _secondRevealedCard;

    public bool UserIsAllowedToRevealCard => _secondRevealedCard == null;

    private void Start()
    {
        _cardsStillInPlay = gridCols * gridRows;
        InstantiateAndPlaceCards();
    }

    private void InstantiateAndPlaceCards()
    {
        var shuffledCardDeck = CreateShuffledCardDeck(cardSprites);
        PlaceCardsInGrid(shuffledCardDeck);
    }

    private void PlaceCardsInGrid(IReadOnlyList<Sprite> shuffledCardDeck)
    {
        var firstCardPosition = firstCard.transform.position;

        for (var i = 0; i < gridCols; i++)
        for (var j = 0; j < gridRows; j++)
        {
            var cardController = i == 0 && j == 0 ? firstCard : Instantiate(firstCard);

            var index = j * gridCols + i;
            cardController.Sprite = shuffledCardDeck[index];

            var positionX = gridOffsetX * i + firstCardPosition.x;
            var positionY = gridOffsetY * j + firstCardPosition.y;
            cardController.transform.position = new Vector3(positionX, positionY, firstCardPosition.z);
        }
    }

    public void OnCardClicked(CardController cardController)
    {
        soundController.PlayClickSound();
        if (_firstRevealedCard == null)
            _firstRevealedCard = cardController;
        else
            _secondRevealedCard = cardController;
    }

    public void OnCardRevealed()
    {
        _numberOfFLippedCards++;
        if (_numberOfFLippedCards > 1)
            StartCoroutine(CheckIfRevealedCardsMatch());
    }

    private IEnumerator CheckIfRevealedCardsMatch()
    {
        if (_firstRevealedCard.Sprite == _secondRevealedCard.Sprite)
        {
            _firstRevealedCard.Match();
            _secondRevealedCard.Match();
            
            _score++;
            scoreLabel.text = "Score: " + _score;
            
            _cardsStillInPlay -= 2;
            if (_cardsStillInPlay < 1)
            {
                StartCoroutine(Victory());
            }
            else
            {
                soundController.PlayMatchSound();
                yield return new WaitForSeconds(2f);
            }
            
            _firstRevealedCard.Remove();
            _secondRevealedCard.Remove();
        }
        else
        {
            soundController.PlayWrongSound();
            _firstRevealedCard.WrongMatch();
            _secondRevealedCard.WrongMatch();
            _score--;
            scoreLabel.text = "Score: " + _score;

            yield return new WaitForSeconds(2f);

            _firstRevealedCard.Hide();
            _secondRevealedCard.Hide();
        }

        _firstRevealedCard = null;
        _secondRevealedCard = null;
        _numberOfFLippedCards = 0;
    }

    private static T[] CreateShuffledCardDeck<T>(T[] array)
    {
        return Helpers.KnuthShuffle(array.Concat(array).ToArray());
    }

    private IEnumerator Victory()
    {
        scoreLabel.text = "Victory! Score: " + _score;
        soundController.PlayVictorySound();
        yield return new WaitForSeconds(6.5f); // Length of audio clip 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}