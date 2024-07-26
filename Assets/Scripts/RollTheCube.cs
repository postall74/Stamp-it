using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamePolygon;

public class RollTheCube : MonoBehaviour
{

    bool isMoving;
    public float speed;
    int TotalCoin, coins;
    public GameObject Ink, StarPuff;
    Animator anim;

    private Vector2 fingerDown;
    private Vector2 fingerUp;
    public bool detectSwipeOnlyAfterRelease = false;

    public float SWIPE_THRESHOLD = 20f;

    LevelGenerate levelGenerate;

    Vector2 Area, oldPos, newPos;
    // Start is called before the first frame update
    void Start()

    {
        levelGenerate = GameObject.Find("GameManager").GetComponent<LevelGenerate>();
        anim = GetComponent<Animator>();
        isMoving = false;
        TotalCoin = GameObject.FindGameObjectsWithTag("Coin").Length;
        coins = 0;
        MovableArea();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMoving)
        {
#if UNITY_ANDROID
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    fingerUp = touch.position;
                    fingerDown = touch.position;
                }

                //Detects Swipe while finger is still moving
                if (touch.phase == TouchPhase.Moved)
                {
                    if (!detectSwipeOnlyAfterRelease)
                    {
                        fingerDown = touch.position;
                        // checkSwipe();
                    }
                }

                //Detects swipe after finger is released
                if (touch.phase == TouchPhase.Ended)
                {
                    fingerDown = touch.position;
                    checkSwipe();
                }
            }
#endif

            if (Input.GetKeyDown(KeyCode.D))
            {
                OnSwipeRight();

            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                OnSwipeLeft();

            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                OnSwipeUp();

            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                OnSwipeDown();

            }
            //    transform.Translate(direction * Time.deltaTime * speed);
        }



       // Vector3 clampedPosition = transform.position;
        // Now we can manipulte it to clamp the y element
     //   clampedPosition.x = Mathf.Clamp(clampedPosition.x, 1f, Area.y-1);
        // re-assigning the transform's position will clamp it
       // transform.position = clampedPosition;

    }




    IEnumerator RotateCube(Vector3 direction) {
        MovableArea();
        Debug.Log("POSITION :"+ transform.position.x +" "+transform.position.z);
        newPos = new Vector2(Mathf.RoundToInt(transform.position.x+direction.x), Mathf.RoundToInt(transform.position.z+direction.z));
        Debug.Log("x : "+direction.x + " Z: " + direction.z);
        if (newPos.x < 1 || newPos.y < 0 || newPos.x == Area.x || newPos.y == Area.y) 
        {
            newPos = new Vector2(transform.position.x, transform.position.z);
            yield return null;
        }
        else
        {

            GameManager.instance.TotalMove();
            Vector3 roatatin_axis;
            Vector3 roatation_point;
            isMoving = true;
            float remainingangle = 90;
            roatation_point = transform.position + direction / 2 + Vector3.down / 2;
            roatatin_axis = Vector3.Cross(Vector3.up, direction);
            while (remainingangle > 0)
            {
                float rotationangle = Mathf.Min(Time.deltaTime * speed, remainingangle);
                transform.RotateAround(roatation_point, roatatin_axis, rotationangle);
                remainingangle -= rotationangle;
                yield return null;
            }
        }
        isMoving = false;
        transform.position = new Vector3(newPos.x,transform.position.y,newPos.y);
        
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("FinishLine")) {
            other.gameObject.transform.GetComponent<BoxCollider>().enabled = false;
            SoundManager.Instance.PlaySound(SoundManager.Instance.rewarded);

            StartCoroutine(StampJUmp());
           
            GameManager.instance.GameEndCam();
        }
        if (other.gameObject.CompareTag("Coin")) {
            SoundManager.Instance.PlaySound(SoundManager.Instance.Tick);
            coins = coins+1;
            Instantiate(Ink,other.gameObject.transform.position, Quaternion.identity);
            Destroy(other.gameObject);

        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.gameOver);
            //Instantiate(Ink, other.gameObject.transform.position, Quaternion.identity);
            GameManager.instance.ShowGameOver();
            //Destroy(other.gameObject);
            Debug.Log("---------------------GameOver");

        }
    }

    IEnumerator StampJUmp() {
        yield return new WaitForSeconds(0.5f);
        anim.SetTrigger("Jump");
    }

    void checkSwipe()
    {
        //Check if Vertical swipe
        if (verticalMove() > SWIPE_THRESHOLD && verticalMove() > horizontalValMove())
        {
            if (fingerDown.y - fingerUp.y > 0)//up swipe
            {
                OnSwipeUp();
            }
            else if (fingerDown.y - fingerUp.y < 0)//Down swipe
            {
                OnSwipeDown();
            }
            fingerUp = fingerDown;
        }

        //Check if Horizontal swipe
        else if (horizontalValMove() > SWIPE_THRESHOLD && horizontalValMove() > verticalMove())
        {
            if (fingerDown.x - fingerUp.x > 0)//Right swipe
            {
                OnSwipeRight();
            }
            else if (fingerDown.x - fingerUp.x < 0)//Left swipe
            {
                OnSwipeLeft();
            }
            fingerUp = fingerDown;
        }

        //No Movement at-all
        else
        {
         
        }
    }

    float verticalMove()
    {
        return Mathf.Abs(fingerDown.y - fingerUp.y);
    }

    float horizontalValMove()
    {
        return Mathf.Abs(fingerDown.x - fingerUp.x);
    }

    //////////////////////////////////CALLBACK FUNCTIONS/////////////////////////////
    void OnSwipeUp()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.Move);

        StartCoroutine(RotateCube(Vector3.forward));
    }

    void OnSwipeDown()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.Move);

        StartCoroutine(RotateCube(Vector3.back));
    }

    void OnSwipeLeft()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.Move);

        StartCoroutine(RotateCube(Vector3.left));
    }

    void OnSwipeRight()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.Move);

        StartCoroutine(RotateCube(Vector3.right));
    }

    public void OnAnimationEnded() {
        StarPuff.SetActive(true);
        StartCoroutine(DelayLevelComplete());
    }


    IEnumerator DelayLevelComplete() {
        yield return new WaitForSeconds(1);
        GameManager.instance.LevelCompleted();
    }


    void MovableArea() {
        Area = levelGenerate.MoveArea();

    }

    void CancelLastMovement() {


    }
}
