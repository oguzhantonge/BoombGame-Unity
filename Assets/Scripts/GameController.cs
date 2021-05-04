using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    Board gameBoard;

    Spawner spawner;

    public Shape currentShape;

    public float dropTime = 0.9f;
    float dropTimeModded;

    float timeToDrop;

    float timeToNextKey;
    /*
    
    [Range(0.02f,1f)]
    public float m_keyRepeatRate= 0.25f;

    
    */

    float timeToNextKeyLeftRight;
    [Range(0.02f, 1f)]
    public float keyRepeatRateLeftRight = 0.25f;


    float timeToNextKeyDown;
    [Range(0.001f, 1f)]
    public float keyRepeatRateDown = 0.01f;


    float timeToNextKeyRotate;
    [Range(0.02f, 1f)]
    public float keyRepeatRateRotate = 0.25f;


    bool clockwise = true;

    public bool movingDown = true;
    public bool IsLanding = false;

    bool gameOver = false;
    public GameObject gameOverPanel;

    public bool m_isPaused = false;

    public GameObject pausePanel;

    SoundManager soundManager;

    public Text diagnosticText;

    enum Direction {none, left, right, up, down}

    Direction dragDirection = Direction.none;
    Direction swipeDirection = Direction.none;

    float timeToNextDrag;
    float timeToNextSwipe;

    [Range(0.05f, 1f)]
    public float minTimeToDrag = 0.15f;

    [Range(0.05f, 1f)]
    public float minTimeToSwipe = 0.3f;

    bool didTap = false;

    void OnEnable()
    {
        TouchController.DragEvent += DragHandler;
        TouchController.SwipeEvent += SwipeHandler;
        TouchController.TapEvent += TapHandler;

    }

    void OnDisable()
    {
        TouchController.DragEvent -= DragHandler;
        TouchController.SwipeEvent -= SwipeHandler;
        TouchController.TapEvent -= TapHandler;

    }

    void Start()
    {
      /*
        if (diagnosticText)
        {
            diagnosticText.text = "";
        }
       */

        timeToNextKey = Time.time;
        
        //gameBoard = GameObject.FindWithTag("Board").GetComponent<Board>();
        //spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();

        gameBoard = GameObject.FindObjectOfType<Board>();
        spawner = GameObject.FindObjectOfType<Spawner>();
        soundManager = GameObject.FindObjectOfType<SoundManager>();

        timeToNextKeyLeftRight = Time.time + keyRepeatRateLeftRight;
        timeToNextKeyDown = Time.time + keyRepeatRateDown;
        timeToNextKeyRotate = Time.time + keyRepeatRateRotate;

        if (!gameBoard)
        {
            Debug.Log("Warning! There is no game board defined!");
        }
      
        if (!spawner)
        {
            Debug.Log("Warning! There is no spawner defined!");
        }
       
        else
        {
           spawner.transform.position = Vector3Int.RoundToInt(spawner.transform.position);
            if (currentShape == null)
            {
              currentShape = spawner.SpawnShape();
            }
        }

        dropTimeModded = dropTime;

        if (gameOverPanel)
        {
            gameOverPanel.SetActive(false);
        }
        if (pausePanel)
        {
            pausePanel.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameBoard || !spawner || !currentShape || gameOver )
        {
            return;
        }
        
        PlayerInput();
    }
    void PlayerInput()
    {
        if(IsLanding == false && m_isPaused == false) {

                if ((Input.GetButton("MoveRight") && (Time.time > timeToNextKeyLeftRight)) || Input.GetButtonDown("MoveRight") && movingDown)
                {
                    MoveRight();

                }
                else if ((Input.GetButton("MoveLeft") && (Time.time > timeToNextKeyLeftRight)) || Input.GetButtonDown("MoveLeft") && movingDown)
                {
                    MoveLeft();
                }
                else if (Input.GetButtonDown("Rotate") && (Time.time > timeToNextKeyRotate) && movingDown)
                {
                    Rotate();

                }
                else if (((Input.GetButton("MoveDown") && (Time.time > timeToNextKeyDown)) || (Time.time > timeToDrop)) && movingDown )
                {
                    MoveDown();
                }
                
             else if ((swipeDirection == Direction.right && Time.time > timeToNextSwipe && movingDown) || ((dragDirection == Direction.right) && movingDown && (Time.time > timeToNextSwipe)))
              {
                MoveRight();

                // swipeDirection = Direction.none;
                //  swipeDirection = Direction.none;
                timeToNextDrag = Time.time + minTimeToDrag;
                timeToNextSwipe = Time.time + minTimeToSwipe;

             }

            else if ((swipeDirection == Direction.left && Time.time > timeToNextSwipe && movingDown) || ((dragDirection == Direction.left) && movingDown && (Time.time > timeToNextSwipe)))
            {
                MoveLeft();

                // dragDirection = Direction.none;
                // swipeDirection = Direction.none;
                timeToNextDrag = Time.time + minTimeToDrag;
                timeToNextSwipe = Time.time + minTimeToSwipe;
            }

            else if (((swipeDirection == Direction.up) && movingDown && (Time.time > timeToNextSwipe)) || (didTap && movingDown))
            {
                Rotate();
                timeToNextSwipe = Time.time + minTimeToSwipe;
                didTap = false;
            }
          
            else if ((dragDirection == Direction.down)  && movingDown)
            {
                MoveDown();

                //dragDirection = Direction.none;
               // swipeDirection = Direction.none;
            }

              

        }

        swipeDirection = Direction.none;
        dragDirection = Direction.none;
        didTap = false;
    }

    private void MoveDown()
    {
        timeToDrop = Time.time + dropTimeModded;
        timeToNextKeyDown = Time.time + keyRepeatRateDown;

        currentShape.MoveDown();
        if (!gameBoard.isProperPositionShape(currentShape))
        {

            if (gameBoard.IsOverLimit(currentShape))
            {
                GameOver();
            }
            else
            {
                movingDown = false;
                IsLanding = true;
                InsertShape();
            }

        }
    }

    private void Rotate()
    {
        //currentShape.RotatingRight();
        currentShape.RotateClockwise(clockwise);

        timeToNextKeyRotate = Time.time + keyRepeatRateRotate;

        if (!gameBoard.IsValidHorizontalPosition(currentShape))
        {
            //currentShape.RotatingLeft();
            currentShape.RotateClockwise(!clockwise);

        }
        else
        {
            PlaySound(soundManager.moveSound, 1f);
        }

        // gameBoard.CorrectingPosition();
    }

    private void MoveLeft()
    {
        currentShape.MoveLeft();
        timeToNextKeyLeftRight = Time.time + keyRepeatRateLeftRight;

        if (!gameBoard.IsValidHorizontalPosition(currentShape))
        {
            currentShape.MoveRight();

        }
        else
        {
            PlaySound(soundManager.moveSound, 1f);
        }
    }

    private void MoveRight()
    {
        currentShape.MoveRight();
        timeToNextKeyLeftRight = Time.time + keyRepeatRateLeftRight;


        if (!gameBoard.IsValidHorizontalPosition(currentShape))
        {
            currentShape.MoveLeft();

        }
        else
        {
            PlaySound(soundManager.moveSound, 1f);
        }
    }

    void InsertShape()
    {
        // shape lands here
        // timeToNextKey = Time.time;
        if (currentShape)
        {
            

            currentShape.MoveUp();

            // gameBoard.StoreShapeInGrid(currentShape);
             gameBoard.CheckForOtherChildObjects(Board.childs);
            Board.childs.Clear();
            //gameBoard.StoreShapeInGrid(currentShape);
            PlaySound(soundManager.landSound, 0.75f);
            StartCoroutine(gameBoard.ClearSquare(true));

            // currentShape = spawner.SpawnShape();
            // movingDown = true;

            timeToNextKeyLeftRight = Time.time;
            timeToNextKeyDown = Time.time;
            timeToNextKeyRotate = Time.time;

          //  gameBoard.StartCoroutine("ClearAllRows");

        }

    }

    void GameOver()
    {
        currentShape.MoveUp();
        gameOver = true;
       // Debug.LogWarning(currentShape.name + " is over the limit");
       
                if (gameOverPanel)
                {
                    gameOverPanel.SetActive(true);
                }
       
         
          PlaySound(soundManager.gameOverClip, 5f);
         
      
    }

    public void PlaySound(AudioClip clip, float volume)
    {
        if (clip && soundManager.isfxEnabled)
        {
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, Mathf.Clamp(soundManager.fxVolume * volume, 0.05f, 1f));
        }

    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void TogglePause()
    {
        if (gameOver)
        {
            return;
        }

        m_isPaused = !m_isPaused;

        if (pausePanel)
        {
            pausePanel.SetActive(m_isPaused);

            if (soundManager)
            {
               soundManager.musicSource.volume = (m_isPaused) ? 0.15f : 0.3f;
            }

            Time.timeScale = (m_isPaused) ? 0 : 1;

        }

    }

     
    void DragHandler(Vector2 dragMovement)
    {
       /*
        if (diagnosticText)
        {
            diagnosticText.text = "Swipe Event Detected";
        }
        */
        dragDirection = GetDirection(dragMovement);
    }

    void SwipeHandler(Vector2 swipeMovement)
    {
      /*
        if (diagnosticText)
        {
            diagnosticText.text = "";
        }
       */
        swipeDirection = GetDirection(swipeMovement);
    }
    void TapHandler(Vector2 tapMovement)
    {
        /*
          if (diagnosticText)
          {
              diagnosticText.text = "";
          }
         */
        //swipeDirection = GetDirection(swipeMovement);

        didTap = true;
    }

    Direction GetDirection(Vector2 swipeMovement)
    {
        Direction swipeDir = Direction.none;

        //horizontal
        if (Mathf.Abs(swipeMovement.x) > Mathf.Abs(swipeMovement.y))
        {
            swipeDir = (swipeMovement.x >= 0) ? Direction.right : Direction.left;
        }

        //vertical
        else
        {
            swipeDir = (swipeMovement.y >= 0) ? Direction.up : Direction.down;
        }
        
        return swipeDir;
    }





}
