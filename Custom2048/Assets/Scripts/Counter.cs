using UnityEngine;
using System.Collections;

public class Counter : MonoBehaviour
{

    public static Counter _instance;


    private UILabel m_lable;
    private int m_grade;
    private string m_text;


    public int m_count = 0;

    void Awake()
    {
        m_lable = transform.GetComponent<UILabel>();
        m_text = "Grade ";

        m_grade = 0;
        if (_instance == null)
        {
            _instance = this;
        }
    }

    void Start()
    {
        m_lable.text = m_text + m_grade;

    }

    public void addGrade(int num)
    {
        m_grade += num;
        //StartCoroutine(updateContent());
        updateContent();
    }

    void Update()
    {
       

    }

    void LateUpdate()
    {

  

    }


    public void updateContent()
    {

        m_lable.text = m_text + m_grade;
    }

}
