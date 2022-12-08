using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 0.1f;
    [SerializeField] private float rotationRadius = 5f;
    private float _rotationAngle;

    private Vector3 _startPosition;

    private void Start()
    {
        _startPosition = transform.position;
    }

    private void Update()
    {
        _rotationAngle += rotationSpeed * Time.deltaTime;

        var offset = new Vector3(Mathf.Sin(_rotationAngle), Mathf.Cos(_rotationAngle), 0) *
                     rotationRadius;
        transform.position = _startPosition + offset;
    }
}