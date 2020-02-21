using UnityEngine;
using System.Collections;
using DG.Tweening;

public class NumberManager : MonoBehaviour
{

    public Transform m_leftPos;
    public Transform m_rightPos;
    private float m_gridDis;


    //container
    private int[][] m_numArray;
    private Number[][] m_numIconArray;

    //gameObject;
    public GameObject m_numIconPrefab;

    //operate
    MoveDir m_dir;
    private Vector3 m_firstTouPoint;
    private bool m_isTou;


    //AudioSource 
    AudioSource m_audioSource;
    void Awake()
    {
        m_gridDis = m_rightPos.position.x - m_leftPos.position.x;


        m_numArray = new int[4][]{  
                                    new int[]{0,0,0,0},
                                    new int[]{0,0,0,0},
                                    new int[]{0,0,0,0},
                                    new int[]{0,0,0,0}
                                    };

        m_numIconArray = new Number[4][]{
                                            new Number[4]{null, null, null, null},
                                            new Number[4]{null, null, null, null},
                                            new Number[4]{null, null, null, null},
                                            new Number[4]{null, null, null, null}

                                         };


        m_isTou = false;

        m_audioSource = transform.GetComponent<AudioSource>();

       
        //Debug.Log("Length:" + m_numArray.Length);
    }

    void Start()
    {
//         m_leftPos.gameObject.SetActive(false);
//         m_rightPos.gameObject.SetActive(false);
        initStartNum();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.G))
        {
            initStartNum();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            m_audioSource.Play();

        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            printNumArray();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            clearAllNum();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            m_dir = MoveDir.U;
            combine(m_dir);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            m_dir = MoveDir.D;
            combine(m_dir);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            m_dir = MoveDir.L;
            combine(m_dir);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            m_dir = MoveDir.R;
            combine(m_dir);
        }

    }


    void initStartNum()
    {

        //clearAllNum();

        int[] addArray = new int[16];
        for (int index = 0; index < 16; index++)
        {
            addArray[index] = index;
        }
        int addLen = addArray.Length;

        int randomLen = Random.Range(6, 10);
        //int randomLen = 7;
        int[] randomArray = new int[randomLen];
        for (int index = 0; index < randomLen; index++)
        {
            int addIndex = Random.Range(0, addLen--);
            randomArray[index] = addArray[addIndex];

            int temp = addArray[addIndex];
            addArray[addIndex] = addArray[addLen];
            addArray[addLen] = temp;
        }

        GameObject go;
        for (int index = 0; index < randomLen; index++)
        {
            int x = (randomArray[index]) % 4;
            int y = (randomArray[index]) / 4;

            go = NGUITools.AddChild(this.gameObject, m_numIconPrefab);

            go.transform.position = new Vector3(m_leftPos.position.x + m_gridDis * x, m_leftPos.position.y - m_gridDis * y, m_leftPos.position.z);
            go.GetComponent<Number>().setNum(2);
            go.gameObject.name = "number" + go.GetComponent<Number>().getNum();
            m_numArray[x][y] = 1;
            m_numIconArray[x][y] = go.GetComponent<Number>();

        }
        //gernerate random index;

    }


    public void combine(MoveDir dir)
    {
        m_dir = dir;
        switch (m_dir)
        {
            case MoveDir.U:
                checkCol(m_dir);
                break;

            case MoveDir.D:
                checkCol(m_dir);
                break;

            case MoveDir.L:
                checkRow(m_dir);
                break;

            case MoveDir.R:
                checkRow(m_dir);
                break;
        }

        addBasicNum();
    }
    /*void combine()
    {
        bool isEqual = false;
        //row check;
        for (int y = 0; y < m_numArray.Length; y++)
        {
            int canMoveToX = 0;
            for (int x = 0; x < m_numArray.Length; x++)
            {
                if (m_numArray[x][y] != 0)
                {
                    int numValue = m_numIconArray[x][y].getNum();
                    int rightX;
                    for (rightX = x + 1; rightX < m_numArray.Length; rightX++)
                    {
                        if (m_numArray[rightX][y] == 0) continue;
                        else if (m_numIconArray[rightX][y].getNum() != numValue) break;//if have the same value,clear x and rightX ,next  move;
                        else if (m_numIconArray[rightX][y].getNum() == numValue) { isEqual = true; break; }

                    }
                    if (isEqual)
                    {
                        //Debug.Log("find " + y + " row " + "col: " + x + " col: " + rightX);
                        //StartCoroutine(addNewNum(canMoveToX, y, x, y, rightX, y));  

                        StartCoroutine(addNewNum(canMoveToX, y, x, y, rightX, y));
                        clearNum(x, y);
                        clearNum(rightX, y);
                    }
                    else
                    {

                        StartCoroutine(addNewNum(canMoveToX, y, x, y));
                        clearNum(x, y);
                    }
                    isEqual = false;
                    canMoveToX++;
                }
            }
        }
        //addNewNum();
    }*/

    private void checkRow(MoveDir dir)
    {
        int fromX = 0;
        int icr = 1;
        bool dirSymbol = false;
        if (dir == MoveDir.R)
        {
            fromX = m_numArray.Length - 1;
            icr = -1;
            dirSymbol = true;
        }
        else if (dir == MoveDir.L)
        {
            fromX = 0;
            icr = 1;
            dirSymbol = false;
        }



        bool isEqual = false;
        //row check;
        for (int y = 0; y < m_numArray.Length; y++)
        {
            int canMoveToX = fromX;
            for (int x = canMoveToX; (dirSymbol ? x > -1 : x < m_numArray.Length); x += icr)
            {
                if (m_numArray[x][y] != 0)
                {
                    int numValue = m_numIconArray[x][y].getNum();
                    int nearX;
                    for (nearX = x + icr; (dirSymbol ? nearX > -1 : nearX < m_numArray.Length); nearX += icr)
                    {
                        //Debug.Log("near x " + nearX + " y" + y);
                        if (m_numArray[nearX][y] == 0) { continue; }
                        else if (m_numIconArray[nearX][y].getNum() != numValue) break;//if have the same value,clear x and nearX ,next  move;
                        else if (m_numIconArray[nearX][y].getNum() == numValue) { isEqual = true; break; }

                    }
                    if (isEqual)
                    {
                        //Debug.Log("find " + y + " row " + "col: " + x + " col: " + nearX);
                        //StartCoroutine(addNewNum(canMoveToX, y, x, y, nearX, y));  

                        StartCoroutine(addNewNum(canMoveToX, y, x, y, nearX, y));
                        clearNum(x, y);
                        clearNum(nearX, y);
                    }
                    else
                    {

                        StartCoroutine(addNewNum(canMoveToX, y, x, y));
                        clearNum(x, y);
                    }

                    isEqual = false;
                    canMoveToX += icr;
                }

            }
        }
        //addNewNum();


    }

    private void checkCol(MoveDir dir)
    {
        int fromY = 0;
        int icr = 1;
        bool dirSymbol = false;
        if (dir == MoveDir.D)
        {
            fromY = m_numArray.Length - 1;
            icr = -1;
            dirSymbol = true;
        }
        else if (dir == MoveDir.U)
        {
            fromY = 0;
            icr = 1;
            dirSymbol = false;
        }



        bool isEqual = false;
        //row check;
        for (int x = 0; x < m_numArray.Length; x++)
        {
            int canMoveToY = fromY;
            for (int y = canMoveToY; (dirSymbol ? y > -1 : y < m_numArray.Length); y += icr)
            {
                if (m_numArray[x][y] != 0)
                {
                    int numValue = m_numIconArray[x][y].getNum();
                    int nearY;
                    for (nearY = y + icr; (dirSymbol ? nearY > -1 : nearY < m_numArray.Length); nearY += icr)
                    {
                        //Debug.Log("x " + x + " near y" + nearY);
                        if (m_numArray[x][nearY] == 0) { continue; }
                        else if (m_numIconArray[x][nearY].getNum() != numValue) break;//if have the same value,clear x and nearY ,next  move;
                        else if (m_numIconArray[x][nearY].getNum() == numValue) { isEqual = true; break; }

                    }
                    if (isEqual)
                    {
                        //Debug.Log("find " + y + " row " + "col: " + x + " col: " + nearY);
                        //StartCoroutine(addNewNum(canMoveToY, y, x, y, nearY, y));  

                        StartCoroutine(addNewNum(x, canMoveToY, x, y, x, nearY));
                        clearNum(x, y);
                        clearNum(x, nearY);
                    }
                    else
                    {
                        StartCoroutine(addNewNum(x, canMoveToY, x, y));
                        clearNum(x, y);
                    }
                    isEqual = false;
                    canMoveToY += icr;
                }

            }
        }
        //addNewNum();


    }

    private void clearAllNum()
    {
        for (int x = 0; x < m_numArray.Length; x++)
        {
            for (int y = 0; y < m_numArray.Length; y++)
            {
                m_numArray[x][y] = 0;

                if (m_numIconArray[x][y] != null)
                {
                    Destroy(m_numIconArray[x][y].gameObject);
                }

                m_numIconArray[x][y] = null;

            }

        }
    }

    private void clearNum(int x, int y)
    {
        m_numArray[x][y]--;
        if (m_numArray[x][y] == 2)
        {
            return;
        }
        if (m_numArray[x][y] == 0)
        {
            //Debug.Log("remove x " + x + " y " + y);
            m_numIconArray[x][y] = null;
        }
    }

    private IEnumerator addNewNum(int targetX, int targetY, int x, int y, int x1 = -1, int y1 = -1)
    {
        Sequence seq = DOTween.Sequence();
        Vector3 targetPos = m_numIconArray[x][y].transform.position + new Vector3((targetX - x) * m_gridDis, (y - targetY) * m_gridDis, 0);
        if (x1 == -1)//if dont have the same number,+1;
        {

            m_numArray[targetX][targetY]++;
            if (targetX != x || targetY != y)
            {
                m_numIconArray[targetX][targetY] = m_numIconArray[x][y];
                seq.Append(m_numIconArray[x][y].transform.DOMove(targetPos, 0.2f));
                seq.Play();
                yield return new WaitForSeconds(0.25f);
            }

        }
        else//if have the same number,move to here;
        {
            m_numArray[targetX][targetY] += 2;
            Number num;
            Number num1;
            if (targetX == x && targetY == y)
            {
                seq.Append(m_numIconArray[x1][y1].transform.DOMove(targetPos, 0.2f));

            }
            else
            {
                seq.Append(m_numIconArray[x][y].transform.DOMove(targetPos, 0.2f));
                seq.Join(m_numIconArray[x1][y1].transform.DOMove(targetPos, 0.2f));
            }
            seq.Play();
            num1 = m_numIconArray[x1][y1];
            num = m_numIconArray[x][y];
            yield return new WaitForSeconds(0.25f);

            //back to here,begin born a new num;

            //AudioSource
            m_audioSource.Play();

            //generate new number
            GameObject go = NGUITools.AddChild(this.gameObject, m_numIconPrefab);
            Number newNum = go.GetComponent<Number>();
            //set numArray and numIconArray;
            int value = num.getNum();//previous value
            m_numArray[targetX][targetY] = 1;
            m_numIconArray[targetX][targetY] = newNum;

            newNum.setNum(value * 2);
            go.gameObject.name = "number" + newNum.getNum();
            go.transform.position = targetPos;

            Destroy(num.gameObject);
            Destroy(num1.gameObject);

            //update grade
            Counter._instance.addGrade(value);

        }
    }
    private void printNumArray()
    {
        for (int y = 0; y < m_numArray.Length; y++)
        {
            Debug.Log("" + m_numArray[0][y] + " " + m_numArray[1][y] + " " + m_numArray[2][y] + " " + m_numArray[3][y]);
        }
        int[][] icon = new int[4][]{                                            
                                        new int[]{0,0,0,0},
                                        new int[]{0,0,0,0},
                                        new int[]{0,0,0,0},
                                        new int[]{0,0,0,0}
                                        };

        Debug.Log("===================================================");
        for (int x = 0; x < icon.Length; x++)
        {
            for (int y = 0; y < icon.Length; y++)
            {
                if (m_numIconArray[x][y] == null)
                {
                    icon[x][y] = 0;
                }
                else icon[x][y] = m_numIconArray[x][y].getNum();

            }
        }

        for (int y = 0; y < icon.Length; y++)
        {
            Debug.Log("" + icon[0][y] + " " + icon[1][y] + " " + icon[2][y] + " " + icon[3][y]);
        }
    }


    private void addBasicNum()
    {
        int[] empIndex = new int[15];
        int empNumCount = 0;

        for (int x = 0; x < m_numArray.Length; x++)
        {
            for (int y = 0; y < m_numArray.Length; y++)
            {

                if (m_numArray[x][y] == 0)
                {
                    empIndex[empNumCount] = y * 4 + x;
                    empNumCount++;
                }

            }
        }

        if (empNumCount < 8 && empNumCount > 0)//if empty count is larger than 0 and less than 8 ,gernerate new number
        {
            int value = empIndex[Random.Range(0, empNumCount)];
            int x = value % 4;
            int y = value / 4;
            GameObject go = NGUITools.AddChild(this.gameObject, m_numIconPrefab);
            go.transform.position = new Vector3(m_leftPos.position.x + m_gridDis * x, m_leftPos.position.y - m_gridDis * y, m_leftPos.position.z);
            go.GetComponent<Number>().setNum(2);
            go.gameObject.name = "number" + go.GetComponent<Number>().getNum();
            m_numArray[x][y] = 1;
            m_numIconArray[x][y] = go.GetComponent<Number>();
        }
        else if(empNumCount >= 8 && empNumCount <= 15 )
        {
            int i = 0;
            while (i<2)
            {
                int index = Random.Range(0, empNumCount-i-1);
                int value = empIndex[index];
                
                int x = value % 4;
                int y = value / 4;
                GameObject go = NGUITools.AddChild(this.gameObject, m_numIconPrefab);
                go.transform.position = new Vector3(m_leftPos.position.x + m_gridDis * x, m_leftPos.position.y - m_gridDis * y, m_leftPos.position.z);
                go.GetComponent<Number>().setNum(2);
                go.gameObject.name = "number" + go.GetComponent<Number>().getNum();
                m_numArray[x][y] = 1;
                m_numIconArray[x][y] = go.GetComponent<Number>();

             
                empIndex[index] = empIndex[empNumCount - i-1];
                empIndex[empNumCount - i] = value;
                i++;
            }

        }
    }
}
