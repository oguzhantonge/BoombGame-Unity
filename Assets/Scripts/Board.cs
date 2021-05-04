using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public GameController gameController;
    public Spawner spawner;

    public Transform CellSprite;
    public int height = 30;
    public int width = 8;

    int emptyCellsHeader = 17;

    Transform[,] m_grid;
    public static List<Transform> childs = new List<Transform>();
  
    public Transform white;
    public Transform pink;
    public Transform yellow;
    public Transform red;
    public Transform blue;
    public Transform bomb;

    public Transform temp;
    string option = "";


    public ParticleSystem ClearFX;
    SoundManager soundManager;
    ScoreManager scoreManager;


    void Awake()
    {
        m_grid = new Transform[width, height];
    }
    void Start()
    {
        soundManager = GameObject.FindObjectOfType<SoundManager>();
        scoreManager = GameObject.FindObjectOfType<ScoreManager>();
      
        DrowingCells();
    }

    IEnumerator CreateAnimationForDestroyShapes(GameObject targetGameObject, Vector2 startPosition, Vector2 targetPosition, float delay, float timetoDestroy, bool isHorizontal)
    {
        yield return new WaitForSeconds(delay);

        if (isHorizontal)
        {
            if (targetGameObject)
            {
                float currentPosition = targetGameObject.gameObject.transform.position.x;
                float m_startPosition = startPosition.x;
                float m_targetPosition = targetPosition.x;
                float m_inc = ((m_targetPosition - m_startPosition) / timetoDestroy) * Time.deltaTime;
                while (Mathf.Abs(m_targetPosition - currentPosition) > 0.01f)
                {
                    yield return new WaitForEndOfFrame();
                    currentPosition = currentPosition + m_inc;
                    if (targetGameObject)
                    {
                        targetGameObject.gameObject.transform.position = new Vector3(currentPosition, targetGameObject.gameObject.transform.position.y, targetGameObject.gameObject.transform.position.z);
                    }
                }
            }

        }
        else
        {
            if (targetGameObject)
            {
                float currentPosition = targetGameObject.gameObject.transform.position.y;
                float m_startPosition = startPosition.y;
                float m_targetPosition = targetPosition.y;
                float m_inc = ((m_targetPosition - m_startPosition) / timetoDestroy) * Time.deltaTime;
                while (Mathf.Abs(m_targetPosition - currentPosition) > 0.01f)
                {
                    yield return new WaitForEndOfFrame();
                    currentPosition = currentPosition + m_inc;
                    if (targetGameObject)
                    {
                        targetGameObject.gameObject.transform.position = new Vector3(targetGameObject.gameObject.transform.position.x, currentPosition, targetGameObject.gameObject.transform.position.z);
                    }


                }
            }

        }

    }
    public bool IsOverLimit(Shape shape)
    {
        foreach (Transform child in shape.transform)
        {
            if (child.transform.position.y >= (height - emptyCellsHeader - 1))
            {
                return true;
            }
        }

        return false;
    }

    void CreateClearFX(int x, int y)
    {
        List<ParticleSystem> myFX = new List<ParticleSystem>();

        for (int i = 0; i < width; i++)
        {
            ParticleSystem temp = Instantiate(ClearFX, new Vector3(i, y, 0), Quaternion.identity);
            myFX.Add(temp);

        }
        for (int i = 0; i < height - emptyCellsHeader; i++)
        {
            ParticleSystem temp = Instantiate(ClearFX, new Vector3(x, i, 0), Quaternion.identity);
            myFX.Add(temp);
        }

        foreach (ParticleSystem i in myFX)
        {
            i.Play();
        }
    }

    Transform CheckForChangeSprite(string option)
    {

        switch (option)
        {
            case "white":
                temp = pink;
                break;
            case "pink":
                temp = yellow;
                break;

            case "yellow":
                temp = red;
                break;
            case "red":
                temp = blue;
                break;
            case "blue":
                temp = bomb;
                break;

            default:
                break;
        }

        return temp;
    }

  
    void Update()
    {

        
    }
    
    public void CorrectingPosition()
    {
        if (childs != null)
        {
            for (int i = 0; i < childs.Count - 1; i++)
            {
                for (int j = i+1; j < childs.Count; j++)
                {
                    if (childs[i].position.y > childs[j].position.y)
                    {

                        Transform temp = childs[i];
                        childs[i] = childs[j];
                        childs[j] = temp;
                    }
                }

            }

           
        }
    }
    void ClearRow(int y)
    {
        for (int x = 0; x < width; ++x)
        {
            if (m_grid[x, y] != null)
            {
                Destroy(m_grid[x, y].gameObject);
            }
            m_grid[x, y] = null;

        }
    }
    void ClearColumn(int x)
    {
        for (int y = 0; y < height; ++y)
        {
            if (m_grid[x, y] != null)
            {
                Destroy(m_grid[x, y].gameObject);
            }
            m_grid[x, y] = null;

        }
    }

    void ShiftOneRowDown(int y)
    {
        for (int x = 0; x < width; ++x)
        {
            if (m_grid[x, y] != null)
            {
                m_grid[x, y - 1] = m_grid[x, y];
                m_grid[x, y] = null;
                m_grid[x, y - 1].position += new Vector3(0, -1, 0);
            }

        }
    }
    void ShiftRowsDown(int StartY)
    {
        for (int i = StartY; i < height; ++i)
        {
            ShiftOneRowDown(i);
        }
 //       StartCoroutine(ClearSquare(false));
    }

    public IEnumerator ClearSquare(bool canGenerateNewShape)
    {
        yield return new WaitForSeconds(0.1f);
        bool devam = true;
        while (devam)
        {

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height - 1; y++)
                {
                    if (m_grid[x, y] != null && m_grid[x, y + 1] != null && m_grid[x, y].gameObject.tag == m_grid[x, y + 1].gameObject.tag)
                    {
                        StartCoroutine(CreateAnimationForDestroyShapes(m_grid[x, y + 1].gameObject, m_grid[x, y + 1].gameObject.transform.position, m_grid[x, y].gameObject.transform.position, 0f, 0.6f, false));
                        if (m_grid[x, y].gameObject.tag == "bomb" && m_grid[x, y + 1].gameObject.tag == "bomb")
                        {

                            yield return new WaitForSeconds(0.6f);

                            CreateClearFX(x, y);
                            gameController.PlaySound(soundManager.clearRowSound, 1f);
                            scoreManager.Score(100);
                            //   yield return new WaitForSeconds(0.41f);
                            ClearRow(y);
                            ClearColumn(x);
                            yield return new WaitForSeconds(1f);
                          //  StopCoroutine("ClearSquare");
                            ShiftRowsDown(y + 1);
                            

                        }
                        else
                        {
                            yield return new WaitForSeconds(0.6f);
                         //   Debug.Log("addasdsa");
                            Destroy(m_grid[x, y + 1].gameObject);
                            m_grid[x, y + 1] = null;
                            scoreManager.Score(25);

                            CheckForChangeSprite(m_grid[x, y].gameObject.tag);
                            Destroy(m_grid[x, y].gameObject);

                            //   if(m_grid[x,y].gameObject.tag != "bomb") {
                            m_grid[x, y] = null;
                            m_grid[x, y] = Instantiate(temp, new Vector3(x, y, -1), Quaternion.identity);
                            // }
                            //   else
                            //    {
                            //      m_grid[x, y] = null;
                            //    }

                            sqareDownFromUpToDown(y + 2, x);
                            y = -1;
                        }

                    }
                }
            }
            devam = false;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width - 1; x++)
                {
                    if (m_grid[x, y] != null && m_grid[x + 1, y] != null && m_grid[x, y].gameObject.tag == m_grid[x + 1, y].gameObject.tag)
                    {
                        StartCoroutine(CreateAnimationForDestroyShapes(m_grid[x+1, y].gameObject, m_grid[x+1, y].gameObject.transform.position, m_grid[x, y].gameObject.transform.position, 0f, 0.6f, true));
                        if (m_grid[x, y].gameObject.tag == "bomb" && m_grid[x + 1, y ].gameObject.tag == "bomb")
                        {
                            yield return new WaitForSeconds(0.6f);

                            CreateClearFX(x, y);
                            gameController.PlaySound(soundManager.clearRowSound, 1f);
                            scoreManager.Score(100);

                            // yield return new WaitForSeconds(0.41f);
                            ClearRow(y);
                            ClearColumn(x);
                            yield return new WaitForSeconds(1f);
                        //    StopCoroutine("ClearSquare");
                            ShiftRowsDown(y + 1);
                           
                        }
                        else
                        {
                            yield return new WaitForSeconds(0.6f);

                           // Debug.Log("1232");
                            Destroy(m_grid[x + 1, y].gameObject);
                            m_grid[x + 1, y] = null;
                            scoreManager.Score(25);


                            CheckForChangeSprite(m_grid[x, y].gameObject.tag);
                            Destroy(m_grid[x, y].gameObject);

                            m_grid[x, y] = null;
                            m_grid[x, y] = Instantiate(temp, new Vector3(x, y, -1), Quaternion.identity);

                            sqareDownFromUpToDown(y + 1, x + 1);
                            x = -1;
                            devam = true;
                        }
                        
                    }
                }
            }
        }
        if (canGenerateNewShape)
        {
            yield return new WaitForSeconds(0.5f);

            gameController.currentShape = spawner.SpawnShape();
            gameController.movingDown = true;
            gameController.IsLanding = false;
        }
     

    }


    void sqareDownFromUpToDown(int yPos, int x)
    {
        for (int y = yPos; y < height; y++)
        {
            if (m_grid[x, y] != null)
            {
                m_grid[x, y - 1] = m_grid[x, y];
                m_grid[x, y] = null;
                m_grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
       
    }

    bool isInTheBoard(int x, int y)
    {
        return (x >= 0 && x < width && y >= 0);
    }

    bool IsOccupied(int x, int y, Shape shape)
    {
        return (m_grid[x, y] != null && m_grid[x, y].parent != shape.transform);

    }
    public bool IsValidHorizontalPosition(Shape shape)
    {
        foreach (Transform child in shape.transform)
        {
            Vector2 pos = Vector2Int.RoundToInt(child.position);

            if (!isInTheBoard((int)pos.x, (int)pos.y))
            {
                return false;
            }

            if (IsOccupied((int)pos.x, (int)pos.y, shape))
            {
                return false;
            }


        }
        return true;
    }

    public bool isProperPositionShape(Shape shape)
    {
       
        childs.Clear();
        foreach (Transform child in shape.transform)
        {
            childs.Add(child as Transform);
        }
        CorrectingPosition();
        for (int i = 0; i < childs.Count; i++)
        {
            Vector2 pos = Vector2Int.RoundToInt(childs[i].position);
            

            if (!isInTheBoard((int)pos.x, (int)pos.y))
            {
                //StoreChildInGrid(childs[i]);
               // childs.RemoveAt(i);
            
                return false;
            }

            if (IsOccupied((int)pos.x, (int)pos.y, shape))
            {
               // StoreChildInGrid(childs[i]);
                //childs.RemoveAt(i);
                return false;
            }

        }
        return true;
        /*
        foreach (Transform child in shape.transform)
        {
            Vector2 pos = Vector2Int.RoundToInt(child.position);

            if (!isInTheBoard((int)pos.x, (int)pos.y))
            {
                return false;
            }

            if (IsOccupied((int)pos.x, (int)pos.y, shape))
            {
                return false;
            }


        }
        return true;
        */

    }
    bool otherIsOccupied(int x, int y)
    {
        return (m_grid[x, y] != null);
    }

    public void CheckForOtherChildObjects(List<Transform>childss)
    {
        //print(childss.Count);
        for (int i = 0; i < childss.Count; i++)
        {

            bool m_hitBottom = false;
            while (!m_hitBottom)
            {
                childss[i].position += new Vector3(0f, -1f, 0f);
                Vector2 pos = Vector2Int.RoundToInt(childss[i].position);
                if (!isInTheBoard((int)pos.x, (int)pos.y))
                {
                    childss[i].position += new Vector3(0f, 1f, 0f);
                    pos = Vector2Int.RoundToInt(childss[i].position);
                    if (!otherIsOccupied((int)pos.x, (int)pos.y))
                    {
                        StoreChildInGrid(childss[i]);
                        m_hitBottom = true;
                        break;
                    }
                   
                }
                if (otherIsOccupied((int)pos.x, (int)pos.y))
                {
                    
                    bool cannotStore = false;
                    while (!cannotStore)
                    {
                        childss[i].position += new Vector3(0f, 1f, 0f);
                        pos = Vector2Int.RoundToInt(childss[i].position);
                        if (!otherIsOccupied((int)pos.x, (int)pos.y))
                        {
                            StoreChildInGrid(childss[i]);
                            m_hitBottom = true;
                            cannotStore = true;
                        }
                       
                    }
                    


                }
            }
        }
    }
    public void StoreChildInGrid(Transform child)
    {
        if (child == null)
        {
            return;
        }
        Vector2 pos = Vector2Int.RoundToInt(child.position);
        m_grid[(int)pos.x, (int)pos.y] = child;
    }

    void DrowingCells()
    {
        if (CellSprite != null) // Eğer emptySprite prefabımız yoksa eror vermek yerine error mesajı çıkarttık.
        {
            for (int y = 0; y < height - emptyCellsHeader; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Transform clone;
                    clone = Instantiate(CellSprite, new Vector3(x, y, 0), Quaternion.identity) as Transform; //Instantiate is inherited from the Object class.Bu sebeple Transform type casting',i yaptık
                    clone.name = "Board Space ( x = " + x.ToString() + " , y =" + y.ToString() + " )";
                    clone.transform.parent = transform;
                }
            }
        }
        else
        {
            Debug.Log("WARNING! Please assign the emptySprite object!");
        }

    }


    public void StoreShapeInGrid(Shape shape)
    {
        if (shape == null)
        {
            return;
        }

        foreach (Transform child in shape.transform)
        {
            Vector2 pos = Vector2Int.RoundToInt(child.position);
            m_grid[(int)pos.x, (int)pos.y] = child;

        }
    }

    
   
    
}
