using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawLineEX : MonoBehaviour {
	
	float offsetScaleH = 0.0f;
	float lineSize = 0.15f;

	public void DrawLinkLineZ(RealLinkDot from, RealLinkDot toDot, SimRealWorld.ArrageDIR dir, Transform parentTrans) {
		Vector3[] wayPoint;

		if (from.transform.position.x == toDot.transform.position.x || from.transform.position.z == toDot.transform.position.z) {
			// directly link
			wayPoint = new Vector3[2];
			wayPoint[0] = from.transform.position;
			wayPoint[1] = toDot.transform.position;
		} else {
			// correr link
			wayPoint = new Vector3[4];
			wayPoint[0] = from.transform.position;

			if (dir == SimRealWorld.ArrageDIR.top || dir == SimRealWorld.ArrageDIR.bottom) {
				wayPoint[1] = new Vector3(from.transform.position.x, 0, (toDot.transform.position.z - from.transform.position.z) * 0.9f);
				wayPoint[2] = new Vector3(toDot.transform.position.x, 0, (toDot.transform.position.z - from.transform.position.z) * 0.9f);
			} else {
                wayPoint[1] = new Vector3((toDot.transform.position.x - from.transform.position.x) * 0.6f, 0, from.transform.position.z);
                wayPoint[2] = new Vector3((toDot.transform.position.x - from.transform.position.x) * 0.6f, 0, toDot.transform.position.z);
            }

			wayPoint[3] = toDot.transform.position;
		}

		DrawLine (from, toDot, wayPoint, parentTrans);
	}

    public void DrawLinkLineToLeftConner(RealLinkDot from, RealLinkDot toDot, float offset, Transform parentTrans)
    {
        Vector3[] wayPoint;

        if (from.transform.position.x == toDot.transform.position.x || from.transform.position.z == toDot.transform.position.z)
        {
            // directly link
            wayPoint = new Vector3[2];
            wayPoint[0] = from.transform.position;
            wayPoint[1] = toDot.transform.position;
        }
        else
        {
            // correr link
            wayPoint = new Vector3[4];
            wayPoint[0] = from.transform.position;

            wayPoint[1] = new Vector3(from.transform.position.x, 0, (toDot.transform.position.z + offset));
            wayPoint[2] = new Vector3(toDot.transform.position.x, 0, (toDot.transform.position.z + offset));

            wayPoint[3] = toDot.transform.position;
        }

        DrawLine(from, toDot, wayPoint, parentTrans);
    }

    public RealLinkDot CreateRootLineWithDot(RealLinkDot from, Vector3 toPos, Transform parentTrans) {
        GameObject toDotGo = (GameObject)Instantiate(GameManager.Instance.real_corner, parentTrans);
        RealLinkDot toLinkDot = toDotGo.gameObject.AddComponent<RealLinkDot>();
        toDotGo.name = "RootLineEndCap";
        toDotGo.transform.localScale = new Vector3(lineSize, lineSize, lineSize);
        toDotGo.transform.position = toPos;

        Vector3[] wayPoint;
		if (from.transform.position.x == toPos.x || from.transform.position.z == toPos.z) {
			// directly link
			wayPoint = new Vector3[2];
			wayPoint[0] = from.transform.position;
			wayPoint[1] = toPos;
		} else {
			// Z link
			wayPoint = new Vector3[4];
			wayPoint[0] = from.transform.position;
			wayPoint[1] = new Vector3(from.transform.position.x, 0, (toPos.z - from.transform.position.z) * 0.5f);
			wayPoint[2] = new Vector3(toPos.x, 0, (toPos.z - from.transform.position.z) * 0.5f);
			wayPoint[3] = toPos;
		}

		DrawLine (from, toLinkDot, wayPoint, parentTrans);

		return toLinkDot;
	}

	public void DrawLinkLine(RealLinkDot fromDot, RealLinkDot toDot, bool directLink, Transform parentTrans) {
		Vector3[] wayPoints;

		if (directLink) {
			wayPoints = new Vector3[2];
			wayPoints[0] = fromDot.transform.position;
			wayPoints[1] = toDot.transform.position;
		} else {
			wayPoints = new Vector3[3];
			wayPoints[0] = fromDot.transform.position;
			wayPoints[2] = toDot.transform.position;

			if (wayPoints[0].z == wayPoints[2].z) {
				wayPoints[1] = new Vector3(wayPoints[0].x + (wayPoints[2].x - wayPoints[0].x) * 0.5f, 0, wayPoints[0].z);
			} else if (wayPoints[0].x == wayPoints[2].x) {
				wayPoints[1] = new Vector3(wayPoints[0].x, 0, wayPoints[0].z + (wayPoints[2].z - wayPoints[0].z) * 0.5f);
			} else {
				wayPoints[1] = new Vector3(wayPoints[0].x, 0, wayPoints[2].z);
			}

		}

		DrawLine (fromDot, toDot, wayPoints, parentTrans);
	}

	public void DrawLine(RealLinkDot fromDot, RealLinkDot toDot, Vector3[] wayPoint, Transform parentTrans) {
		int j;
		GameObject go;
		// draw line
		Vector3 centerPos = Vector3.zero;
		for (j = 0; j < wayPoint.Length - 1; j++) {
			centerPos = (wayPoint[j] + wayPoint[j + 1]) / 2;
			go = (GameObject)Instantiate(GameManager.Instance.real_line, centerPos, transform.localRotation, parentTrans);
			go.name = "Line" + j + 1;
			go.transform.up = (go.transform.localPosition - wayPoint[j]).normalized;
			float distance = Vector3.Distance(wayPoint[j], wayPoint[j + 1]);
			go.transform.localScale = new Vector3(lineSize, distance / 2 + offsetScaleH, lineSize);

		}

		RealLinkDot prevDot = fromDot;
		// draw port
		for (int i = 1; i < wayPoint.Length - 1; i++) {
			centerPos = wayPoint[i];
			go = (GameObject)Instantiate(GameManager.Instance.real_corner, centerPos, transform.localRotation, parentTrans);
			go.name = "Port" + i;
			go.transform.localScale = new Vector3(lineSize, lineSize, lineSize);
			RealLinkDot dot = go.AddComponent<RealLinkDot> ();
			dot.prev = prevDot;
			prevDot = dot;
		}

		if (toDot != null) {
			toDot.prev = prevDot;
		}

	}

}