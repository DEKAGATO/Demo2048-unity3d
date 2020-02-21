using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Number : MonoBehaviour
{

    //position argument;
    private Vector3 m_curPos;

    //texture argument;
    public int m_value;
    private UITexture m_texture;
    public int m_maxTexX;
    public int m_maxTexY;
    private int m_texX;
    private int m_texY;


    private float m_texX_Offset;
    private float m_texY_Offset;

    private float m_texX_Interval;
    private float m_texY_Interval;



    void Awake()
    {
        m_texture = transform.GetComponent<UITexture>();

        m_texX = 0;
        m_texY = 0;

        m_texX_Offset = -0.008f;
        m_texY_Offset = -0.006f;

        m_texX_Interval = (float)1.0 / m_maxTexX;
        m_texY_Interval = (float)1.0 / m_maxTexY;


    }

    void Start()
    {
        m_texture.uvRect = new Rect((m_texX - 1) * m_texX_Interval + m_texX_Offset, (m_texY - 1) * m_texY_Interval + m_texY_Offset, m_texX_Interval, m_texY_Interval);
        this.transform.localScale = new Vector3(0, 0, 1);
        this.transform.DOScale(new Vector3(1f, 1f, 1f), 0.08f);
      
        //m_texture.uvRect = new Rect((m_texX_Offset - 1) * m_xTextureOffsetTextureInterval, (m_texY_Offset - 1) * m_yTextureOffsetTextureInterval, m_xTextureOffsetTextureInterval, m_yTextureOffsetTextureInterval);
    }

    public void setNum(int num)
    {
        //Debug.Log("log2 :" + Mathf.Log(num, 2));
        if (Mathf.Log(num, 2) * 2.0f % 2 != 0)
        {
            return;
        }

        int y = 0;
        int x = 0;
        switch (num)
        {
            case 2:
                x = 2;
                y = 3;
                break;

            case 4:
                x = 2;
                y = 4;

                break;

            case 8:

                x = 3;
                y = 5;
                break;


            case 16:
                x = 3;
                y = 4;

                break;

            case 32:

                x = 2;
                y = 2;
                break;

            case 64:
                x = 1;
                y = 2;
                break;

            case 128://need to rotate the gameObject ,then can display normally
                x = 3;
                y = 1;
                transform.Rotate(0, 0, 90);
                break;

            case 256:
                x = 3;
                y = 3;
                break;


            case 512:
                x = 1;
                y = 4;
                break;

            case 1024:
                x = 3;
                y = 2;
                break;

            case 2048:
                x = 1;
                y = 3;
                break;

            case 4096:
                x = 2;
                y = 5;
                break;

            case 8192:
                x = 1;
                y = 5;
                break;


        }
        m_value = num;
        m_texX = x;
        m_texY = y;
        m_texture.uvRect = new Rect((m_texX - 1) * m_texX_Interval + m_texX_Offset, (m_texY - 1) * m_texY_Interval + m_texY_Offset, m_texX_Interval, m_texY_Interval);
    }

    public int getNum()
    {
        return m_value;
    }

    public Sequence buildMoveTo(Vector3 targetPos , bool isDesCallback)
    {
        Sequence seq = DOTween.Sequence();
        if (isDesCallback)
        {
            
            seq.Append(transform.DOMove(targetPos, 0.2f));
            seq.AppendCallback(AnimationCallback);
            //seq.Play();
        }
        else
        {
            seq.Append(transform.DOMove(targetPos, 0.2f));
        }
        return seq;
    }

    private void AnimationCallback()
    {
        Destroy(this.gameObject);
    }

    public void setPos(Vector3 pos)
    {
        

    }

}
