using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum eOrientationMode { NODE = 0, TANGENT }

[AddComponentMenu("Splines/Spline Controller")]
[RequireComponent(typeof(SplineInterpolator))]
public class SplineController : MonoBehaviour
{
	public GameObject SplineRoot;
	public float Duration = 10;
	public eOrientationMode OrientationMode = eOrientationMode.NODE;
	public eWrapMode WrapMode = eWrapMode.ONCE;
	public bool AutoStart = false;
	public bool AutoClose = false;
	public bool HideOnExecute = false;


	SplineInterpolator mSplineInterp;
	public Transform[] mTransforms;

	void OnDrawGizmos()
	{

            Transform[] trans = GetTransforms();
            if (trans.Length < 2)
                return;

            SplineInterpolator interp = GetComponent(typeof(SplineInterpolator)) as SplineInterpolator;
            SetupSplineInterpolator(interp, trans);
            interp.StartInterpolation(null, false, WrapMode);


            Vector3 prevPos = trans[0].position;
            for (int c = 1; c <= 100; c++)
            {
                float currTime = c * Duration / 100;
                Vector3 currPos = interp.GetHermiteAtTime(currTime);
                float mag = (currPos - prevPos).magnitude * 2;
                Gizmos.color = new Color(mag, 0, 0, 1);
                Gizmos.DrawLine(prevPos, currPos);
                prevPos = currPos;
            }

	}


	void Start()
	{
		mSplineInterp = GetComponent(typeof(SplineInterpolator)) as SplineInterpolator;


		    mTransforms = GetTransforms();

		if (HideOnExecute)
			DisableTransforms();

		if (AutoStart)
			FollowSpline();
	}



	public void SetupSplineInterpolator(SplineInterpolator interp, Transform[] trans)
	{
		interp.Reset();

		float step = (AutoClose) ? Duration / trans.Length :
			Duration / (trans.Length - 1);

		int c;
		for (c = 0; c < trans.Length; c++)
		{
            if (OrientationMode == eOrientationMode.NODE)
            {
                interp.AddPoint(trans[c].position, trans[c].rotation, step * c, new Vector2(0, 1));
            }
            else if (OrientationMode == eOrientationMode.TANGENT)
            {
                Quaternion rot;
                if (c != trans.Length - 1)
                {
                    Vector3 dir = trans[c + 1].position - trans[c].position;
                    Debug.Log("Angle:" + Vector3.Angle(dir, Vector3.left));
                    //dir = new Vector3(dir.x, 0, 0);
                    rot = Quaternion.LookRotation(dir, trans[c].up);
                }
                else if (AutoClose)
                {
                    Vector3 dir = trans[0].position - trans[c].position;
                    Debug.Log("Angle:" + Vector3.Angle(dir, Vector3.left));
                    //dir = new Vector3(dir.x, 0, 0);
                    rot = Quaternion.LookRotation(dir, trans[c].up);
                }
                else
                    rot = trans[c].rotation;

				interp.AddPoint(trans[c].position, rot, step * c, new Vector2(0, 1));
			}
		}

		if (AutoClose)
			interp.SetAutoCloseMode(step * c);
	}


	/// <summary>
	/// Returns children transforms, sorted by name.
	/// </summary>
	public Transform[] GetTransforms()
	{
		if (SplineRoot != null)
		{
			List<Component> components = new List<Component>(SplineRoot.GetComponentsInChildren(typeof(Transform)));
			List<Transform> transforms = components.ConvertAll(c => (Transform)c);

			transforms.Remove(SplineRoot.transform);
			transforms.Sort(delegate(Transform a, Transform b)
			{
				return a.name.CompareTo(b.name);
			});

			return transforms.ToArray();
		}

		return null;
	}

	/// <summary>
	/// Disables the spline objects, we don't need them outside design-time.
	/// </summary>
	void DisableTransforms()
	{
		if (SplineRoot != null)
		{
			SplineRoot.SetActiveRecursively(false);
		}
	}


	/// <summary>
	/// Starts the interpolation
	/// </summary>
	public void FollowSpline()
	{
		if (mTransforms.Length > 0)
		{
			SetupSplineInterpolator(mSplineInterp, mTransforms);
			mSplineInterp.StartInterpolation(null, true, WrapMode);
		}
	}
}