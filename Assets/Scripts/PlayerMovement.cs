using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private GameObject _inkEffect;
    [SerializeField] private GameObject _starEffect;
    [SerializeField] private bool _detectSwipeAfterRelease = true;
    [SerializeField] private float _swipeThreshold = 20f;

    private Animator _animator;
    private bool _isMoving;
    private Vector2 _fingerDownPosition;
    private Vector2 _fingerUpPosition;
    private LevelGenerator _levelGenerator;
    private Vector2 _levelDimensions;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _levelGenerator = FindObjectOfType<LevelGenerator>();
        _levelDimensions = _levelGenerator.GetLevelDimensions();
    }

    private void Update()
    {
        if (_isMoving) return;
        HandleInput();
    }

    private void HandleInput()
    {
#if UNITY_ANDROID
        HandleTouchInput();
#else
        HandleKeyboardInput();
#endif
    }

    private void HandleTouchInput()
    {
        foreach (Touch touch in Input.touches)
        {
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _fingerUpPosition = touch.position;
                    _fingerDownPosition = touch.position;
                    break;

                case TouchPhase.Moved:
                    if (!_detectSwipeAfterRelease)
                    {
                        _fingerDownPosition = touch.position;
                        DetectSwipe();
                    }
                    break;

                case TouchPhase.Ended:
                    _fingerDownPosition = touch.position;
                    DetectSwipe();
                    break;
            }
        }
    }

    private void HandleKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.D)) OnSwipeRight();
        else if (Input.GetKeyDown(KeyCode.A)) OnSwipeLeft();
        else if (Input.GetKeyDown(KeyCode.W)) OnSwipeUp();
        else if (Input.GetKeyDown(KeyCode.S)) OnSwipeDown();
    }

    private void DetectSwipe()
    {
        if (VerticalMovement() > _swipeThreshold && VerticalMovement() > HorizontalMovement())
        {
            if (_fingerDownPosition.y > _fingerUpPosition.y)
                OnSwipeUp();
            else
                OnSwipeDown();
        }
        else if (HorizontalMovement() > _swipeThreshold && HorizontalMovement() > VerticalMovement())
        {
            if (_fingerDownPosition.x > _fingerUpPosition.x)
                OnSwipeRight();
            else
                OnSwipeLeft();
        }

        _fingerUpPosition = _fingerDownPosition;
    }

    private float VerticalMovement() => Mathf.Abs(_fingerDownPosition.y - _fingerUpPosition.y);
    private float HorizontalMovement() => Mathf.Abs(_fingerDownPosition.x - _fingerUpPosition.x);

    private void OnSwipeUp() => Move(Vector3.forward);
    private void OnSwipeDown() => Move(Vector3.back);
    private void OnSwipeLeft() => Move(Vector3.left);
    private void OnSwipeRight() => Move(Vector3.right);

    private void Move(Vector3 direction)
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.Move);
        StartCoroutine(RotatePlayer(direction));
    }

    private IEnumerator RotatePlayer(Vector3 direction)
    {
        _isMoving = true;
        Vector3 targetPosition = CalculateTargetPosition(direction);

        if (IsPositionValid(targetPosition))
        {
            yield return PerformRotation(direction, targetPosition);
        }

        _isMoving = false;
    }

    private Vector3 CalculateTargetPosition(Vector3 direction)
    {
        return new Vector3(
            Mathf.Round(transform.position.x + direction.x),
            0,
            Mathf.Round(transform.position.z + direction.z)
        );
    }

    private bool IsPositionValid(Vector3 position)
    {
        return position.x >= 0 && position.z >= 0 &&
               position.x < _levelDimensions.x && position.z < _levelDimensions.y;
    }

    private IEnumerator PerformRotation(Vector3 direction, Vector3 targetPosition)
    {
        Vector3 rotationAxis = Vector3.Cross(Vector3.up, direction);
        Vector3 pivotPoint = transform.position + direction / 2 + Vector3.down / 2;
        float remainingAngle = 90f;

        while (remainingAngle > 0)
        {
            float rotationAngle = Mathf.Min(Time.deltaTime * _speed, remainingAngle);
            transform.RotateAround(pivotPoint, rotationAxis, rotationAngle);
            remainingAngle -= rotationAngle;
            yield return null;
        }

        transform.position = targetPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.FinishLineTagName))
        {
            HandleFinish(other);
        }
        else if (other.CompareTag(Constants.CoinTagName))
        {
            HandleCoinCollection(other);
        }
        else if (other.CompareTag(Constants.EnemyTagName))
        {
            HandleEnemyCollision();
        }
    }

    private void HandleFinish(Collider finishLine)
    {
        finishLine.GetComponent<Collider>().enabled = false;
        SoundManager.Instance.PlaySound(SoundManager.Instance.Rewarded);
        StartCoroutine(PlayJumpAnimation());
    }

    private void HandleCoinCollection(Collider coin)
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.Tick);
        Instantiate(_inkEffect, coin.transform.position, Quaternion.identity);
        Destroy(coin.gameObject);
    }

    private void HandleEnemyCollision()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.GameOver);
        UIManager.Instance.ShowGameOver();
    }

    private IEnumerator PlayJumpAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        _animator.SetTrigger("Jump");
    }

    public void OnLevelCompleteAnimation()
    {
        _starEffect.SetActive(true);
        StartCoroutine(CompleteLevel());
    }

    private IEnumerator CompleteLevel()
    {
        yield return new WaitForSeconds(1f);
        UIManager.Instance.ShowLevelComplete();
    }
}