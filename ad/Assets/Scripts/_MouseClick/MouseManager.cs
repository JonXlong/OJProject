using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 鼠标点击模块 ，主要用来点击场景中需要说明的物体。
/// </summary>
public enum MouseState {
    FREE,CLICK,BACK
}
public class MouseManager : MonoBehaviour {


    public static MouseManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    // Update is called once per frame
    private bool isRotate = false;
    public MouseState mouseState;
    private Transform targetTrans;
    private Vector3 refVec;
    public  Vector3 oldPos;
    public  Quaternion oldRate;
    public bool isClick = false;
    void Update () {
        if (Input.GetMouseButton (0)&&mouseState==MouseState.FREE)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray,out hit))
            {
                SimWorldManager.Instance.currentWorld.StopRotation();
                Debug.Log(hit.transform.gameObject.name);
                targetTrans = hit.transform;
                oldPos = transform.position; oldRate = transform.rotation;
                //isRotate = true; mouseState = MouseState.CLICK;
            }
        }
        if (isRotate)
        {
            Vector3 pos = targetTrans.position - transform.position;
            Quaternion targetQuate = Quaternion.LookRotation(pos,Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetQuate, 6 * Time.deltaTime);

            //Move to OBJ
            transform.position = new Vector3(Mathf.SmoothDamp(transform.position.x, targetTrans.position.x, ref refVec.x, 1), Mathf.SmoothDamp(transform.position.y, targetTrans.position.y, ref refVec.y, 1), Mathf.SmoothDamp(transform.position.z, targetTrans.position.z, ref refVec.z, 1)
                );
            float  distancePos = Vector3.Distance(transform.position,targetTrans.position);
            isRotate = distancePos <= 20 ? false : isRotate;
            if (isRotate==false )
            {
                isClick = true;
                if (targetTrans.GetComponent<SimUnitVirtual>())
                {
                    SimUnitVirtual suv = targetTrans.GetComponent<SimUnitVirtual>();

                    if (suv.model != null)
                    {
                        GameManager.Instance.QuestId = suv.team.teamData.team;
                        //strName = suv.team.teamData.name;
                        // strMessage = suv.team.teamData.explain;                       
                        Debug.Log("............." + GameManager.Instance.QuestId +"name"+suv.team.teamData.name);
                        GameManager.Instance.requestTeamData = true;
                        UIPannelManager.Instance.isTitle = false ;
                        //  strScore = GameConfigs.Instance.score;
                        //  strSolvedCount = GameConfigs.Instance.solvedCount;
                    }
                    else
                    {
                        UIPannelManager.Instance.isTitle = true;
                        GameManager.Instance.QuestId = suv.puzzleData.id;
                        //strName = suv.titleName;
                        GameManager.Instance.requestTitleData = true;
                        //strMessage = GameConfigs.Instance.explian_date;//"题目说明";
                        //strMessage = suv.puzzleData.explain;
                        Debug.Log(".........."+"name"+suv.puzzleData.name +".........id"+GameManager.Instance.QuestId);

                    }
                }
                Debug.Log("show pannel");
              
                UIPannelManager.Instance.ShowUIPannel();
            }
        }
        if (mouseState==MouseState.CLICK)
        {
            Vector3 pos = targetTrans.position - transform.position;
            Quaternion targetQuate = Quaternion.LookRotation(pos, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetQuate, 6 * Time.deltaTime);
        }
       // mouseState = Input.GetMouseButton(1) ? MouseState.BACK : mouseState;
        if (mouseState==MouseState.BACK)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, oldRate, 6 * Time.deltaTime);
            transform.position = new Vector3(Mathf.SmoothDamp(transform.position.x, oldPos.x, ref refVec.x, 1), Mathf.SmoothDamp(transform.position.y, oldPos.y, ref refVec.y, 1), Mathf.SmoothDamp(transform.position.z, oldPos.z, ref refVec.z, 1)
               );
            float distancePos = Vector3.Distance(transform.position, oldPos);
            mouseState = distancePos < 2.4f ? MouseState.FREE : mouseState;
        }
        //Debug.Log(mouseState);
	}
}
