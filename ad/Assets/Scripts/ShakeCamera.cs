using UnityEngine;
using System.Collections;

public class ShakeCamera : MonoBehaviour
{

    public Transform camTransform;

    //持续抖动的时长
    public float shake = 0f;

    // 抖动幅度（振幅）
    //振幅越大抖动越厉害
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;

    Vector3 originalPos;
    //private void OnGUI()
    //{
    //    if (GUI.Button(new Rect(0,100,100,60),"Move Camera"))
    //    {
    //        shake = 0.1f;
    //    }
    //}
    public void BeginShake()
    {
        shake = 0.5f;
    }
    void Awake()
    {
        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    void OnEnable()
    {
        originalPos = camTransform.localPosition;
    }

    void Update()
    {
        if (shake > 0)
        {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

            shake -= Time.deltaTime * decreaseFactor;
        }
        
        //else
        //{
        //    shake = 0f;
        //    camTransform.localPosition = originalPos;
        //}

    }
}

 


