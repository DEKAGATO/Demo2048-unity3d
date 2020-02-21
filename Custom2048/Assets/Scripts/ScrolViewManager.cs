using UnityEngine;
using System.Collections;

public class ScrolViewManager : MonoBehaviour {

    public enum MoveDir
    {
        Vertical,
        Horizontal
    }
    public MoveDir m_moveDir = MoveDir.Vertical;
    public float m_speed;
    private Vector3 m_speedVector;
    
    public GameObject[] m_items;
    private float m_itemWidth;
    private float m_itemHeight;

    public float m_itemVerticalInterval;
    public float m_itemHorizonInterval;
    private Vector3 m_intervalVector;

    private float m_maxDistance;
 
    public float m_scrolViewHeight;
    public float m_scrolViewWidth;

    


    private float m_curMoveDis;

    private Vector3 m_originPos;
    


    void Awake()
    {
        m_itemWidth = m_items[0].GetComponent<UISprite>().width;
        m_itemHeight = m_items[0].GetComponent<UISprite>().height;

//         m_itemVerticalInterval = transform.GetComponent<UIGrid>().cellHeight;
//         m_itemHorizonInterval = transform.GetComponent<UIGrid>().cellWidth;

        m_scrolViewHeight = transform.GetComponent<UIPanel>().height;
        m_scrolViewWidth = transform.GetComponent<UIPanel>().width;
     


        m_curMoveDis = 0;
    }
	// Use this for initialization
	void Start () {
        setProperty();
        initPos();
    }
	
	// Update is called once per frame
	void Update () {
        
        if (m_curMoveDis >=  m_maxDistance)
        {
            Vector3 newPos = Vector3.zero;
            for (int i = 0; i < m_items.Length; i++)
            {
                if (i == 0)
                {
                    newPos = m_originPos;
                    m_items[i].transform.localPosition = newPos;
                }
                else
                {
                    newPos = newPos - new Vector3(0, m_itemVerticalInterval, 0);
                    m_items[i].transform.localPosition = newPos;
                }

            }
            m_curMoveDis = 0;
        }
        else
        {
            for (int i = 0; i < m_items.Length; i++)
            {

                m_items[i].transform.localPosition += m_speedVector * Time.deltaTime;

            }
            m_curMoveDis += m_speed*Time.deltaTime;
       
        }

       
	}

    private void setProperty()
    {
        m_originPos = Vector3.zero;
        if (m_moveDir == MoveDir.Vertical)
        {
            //m_originPos = new Vector3(0, transform.localPosition.y, 0);
            m_maxDistance = (m_items.Length - 1) * m_itemVerticalInterval + m_itemHeight + m_scrolViewHeight;
            m_originPos.y = m_originPos.x - m_scrolViewHeight / 2- m_itemHeight/2;
            m_intervalVector = new Vector3(0, m_itemVerticalInterval, 0);
            m_speedVector = new Vector3(0, m_speed, 0);
        }
        else
        {
            //m_originPos = new Vector3(transform.localPosition.x, 0, 0);
            m_maxDistance = (m_items.Length - 1) * m_itemHorizonInterval + m_itemWidth + m_scrolViewWidth;
            m_originPos.x = m_originPos.x - m_scrolViewWidth / 2 - m_itemWidth/2;
            m_intervalVector = new Vector3(m_itemHorizonInterval, 0, 0);
            m_speedVector = new Vector3(m_speed, 0, 0);
        }
    }

    private void initPos()
    {
        Vector3 StartPos = Vector3.zero;
        for (int i = 0; i < m_items.Length; i++ )
        {
            
            if (i == 0 )
            {
                StartPos = m_originPos;
                m_items[i].transform.localPosition = StartPos; 
            }
            else
            {
                StartPos = StartPos - m_intervalVector; 
                m_items[i].transform.localPosition = StartPos; 
            }
        }


    }

}
