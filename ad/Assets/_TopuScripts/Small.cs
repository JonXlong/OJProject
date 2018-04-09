
using UnityEngine;

public class Small : MonoBehaviour {
	

	// Use this for initialization
	public GameObject Obj;
	public int Level;
	public Vector3[] lineVect;
	private Vector3 pos;
	GameObject obj ;
	public OBJ_TYPE  objtype=OBJ_TYPE.down;
	public float disHorizontal;
	public float disVertical;
	public float disDown=4;
	public Vector3 disH;
	void Start () {
		
	
	


	}
public 	void SetEnable()
	{
		print ("............");
		Vector3[] pos = new Vector3[2];
		Vector3 vec = transform.position;
		pos.SetValue (transform.position, 0);
		switch (objtype) {
		case  OBJ_TYPE.down:
			pos.SetValue (new Vector3(vec.x, vec.y, vec.z+4),1);
			break;
		case OBJ_TYPE.middle:
			pos.SetValue(disH,1);
			break;
		case OBJ_TYPE.outside:
			
			break;
		case OBJ_TYPE.up:
			pos.SetValue (new Vector3( vec.x, vec.y, vec.z+ disVertical),1);
			break;
		default:
		break;
		}

	
		SetLineVect (2,pos);
	}
	public void SetLineVect(int posCount,Vector3 [] vc)
	{
		obj= Instantiate (Obj, transform);
		pos = transform.position;
		DrawLineFree dlf = obj.GetComponent<DrawLineFree> ();

		dlf.posCount = posCount;
		dlf.SetLinePos (posCount, vc);
	}
	// Update is called once per frame
	void Update () {
		
	}
}
