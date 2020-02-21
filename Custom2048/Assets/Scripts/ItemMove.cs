using UnityEngine;
using System.Collections;

public class ItemMove : MonoBehaviour {


    public float m_speed;
    
    void Update()
    {

        this.transform.position += new Vector3(0, -m_speed*Time.deltaTime, 0);
    
    }



}
