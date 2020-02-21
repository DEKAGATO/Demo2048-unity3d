using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{


    private Vector3 m_begTouPos;
    //    private Vector3 m_endTouPos;
    private bool m_isTou;
    private MoveDir m_dir;

    private NumberManager m_numManager;

    void Awake()
    {
        m_begTouPos = Vector3.zero;
        m_isTou = false;

        m_numManager = GameObject.FindGameObjectWithTag("Numbers").GetComponent<NumberManager>();
    }

    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (m_isTou == false)
            {
                m_begTouPos = Input.mousePosition;
                m_isTou = true;
            }

        }
        else
        {
            if (m_isTou)
            {
                //m_endTouPos = Input.mousePosition;
                m_isTou = false;
                setDir(Input.mousePosition);
                m_numManager.combine(m_dir);
                
            }
        }
    }

    private void setDir(Vector3 endTouPos)
    {
        float dx = endTouPos.x - m_begTouPos.x;
        float dy = endTouPos.y - m_begTouPos.y;

        if (Mathf.Abs(dx) > Mathf.Abs(dy))
        {
            if (dx > 0)
            {
                m_dir = MoveDir.R;
            }
            else
            {
                m_dir = MoveDir.L;
            }
        }
        else
        {
            if (dy > 0)
            {
                m_dir = MoveDir.U;
            }
            else
            {
                m_dir = MoveDir.D;
            }
        }
    }
}
