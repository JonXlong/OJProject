using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum OBJ_TYPE:int {
	down = 0,
	middle = 1,
	up = 2,
	outside=3
}
public class SmallManager : MonoBehaviour {
	
	public static SmallManager Instance;
	public GameObject objProfab;
	public GameObject Obj;
	private GameObject lineobj;
	private OBJ_TYPE objType;
	[Space (20)]
	public List  <Small> middlList=new List<Small>();
	public List <Small >doswnList=new List<Small>();
	void Awake()
	{
		Instance = this;
	}
	// Use this for initialization
	void Start () {
		Vector3[] pos = new Vector3[4];
		pos [0] = new Vector3 (3,0.5f,0);
		pos [1] = new Vector3 (0,0.5f,0);
		pos [2] = new Vector3 (-3,0.5f,0);
		pos [3] = new Vector3 (-5,0.5f,0);
		GetOBJ (4,pos,objType=OBJ_TYPE.down);
		Vector3[] pos1 = new Vector3[3];
		pos1.SetValue (new Vector3(3,0.5f,8),0);
		pos1.SetValue (new Vector3(-4,0.5f,8),1);
		pos1.SetValue (new Vector3(-7,0.5f,8),2);

	GetOBJ (3, pos1,objType=OBJ_TYPE.middle);
	}
	//横向的一条线段
	Vector3 []  lineVec;
	//向上偏移量
	float disDown;
	public void GetOBJ(int indexCount,Vector3[] pos,OBJ_TYPE objT ){
		lineVec = new Vector3[indexCount * 2];
		for (int i = 0; i < indexCount; i++) {
			GameObject obj =Instantiate (objProfab  );
			obj.transform.position = pos[i];
			obj.GetComponent<Small> ().objtype = objT;
			//最下层的物体统一向上画线
			switch (objT) {
			case OBJ_TYPE.down:
				obj.GetComponent<Small> ().SetEnable ();
				disDown = obj.GetComponent<Small> ().disDown;
				doswnList.Add (obj.GetComponent<Small>());
				break;
			case OBJ_TYPE.middle:
				//obj.GetComponent<Small> ().SetEnable ();
			//	disDown = obj.GetComponent<Small> ().disDown;
				middlList.Add(obj.GetComponent<Small>());
				break;
			case OBJ_TYPE.outside:
				break;
			case OBJ_TYPE.up:
				break;
			default:
				break;
			}
		
		}
		if (objT==OBJ_TYPE.down) {
			//画一条连接最下层物体的线
			lineVec.SetValue (new Vector3(pos [0].x,pos[0].y,pos[0].z+disDown), 0);lineVec.SetValue (new Vector3(pos[pos.Length-1].x,pos[pos.Length-1].y,pos[pos.Length-1].z+disDown),1);
			SetLineVect (2,lineVec);

		}

		if (objT==OBJ_TYPE.middle) {
			Vector3[] midV = new Vector3[2];
			midV.SetValue (middlList[0].transform.position,0);
			midV.SetValue (middlList[middlList.Count-1].transform.position,1);
			SetLineVect (2,midV);

			//计算出上面这条线的中间坐标 /当有上一层物体坐标时则采用上一层物体的坐标
			Vector3 posMidle=new Vector3((lineVec[0].x+lineVec[1].x)/2,pos[0].y,lineVec[0].z);
			Vector3[] middlPos = new Vector3[2];
			middlPos.SetValue (posMidle,0);middlPos.SetValue (new Vector3(posMidle.x,posMidle.y,middlList[0].transform.position.z),1);
			SetLineVect (2,middlPos);
		}

//		for (int j = 0; j < lineVec.Length; j++) {
//			if (j/1==1) {
//				lineVec [j] = new Vector3 (pos[0].x,pos[0].y,pos[0].z+5);
//			}
//			if (j/2==1) {
//				Debug.LogError (j);
//				lineVec [j] = new Vector3 (pos[1].x,pos[1].y,pos[1].z+5);
//			}
////			if (j%2==0&&j/2==1) {
////		//		Debug.LogError (j);
////				print(j);
////				lineVec [j] = new Vector3 (pos[pos.Length].x,pos[pos.Length].y,pos[pos.Length].z+4);
////			}
//		}
//		lineVec [0] = pos [0];
//
//		lineVec [lineVec.Length-1] = pos [pos.Length-1];
//		SetLineVect (lineVec.Length, lineVec);
	}


	public void SetLineVect(int posCount,Vector3 [] vc)
	{
		lineobj= Instantiate (Obj);

		DrawLineFree dlf = lineobj.GetComponent<DrawLineFree> ();

		dlf.posCount = posCount;
		dlf.SetLinePos (posCount, vc);
	}
	// Update is called once per frame
	void Update () {
		
	}
}
