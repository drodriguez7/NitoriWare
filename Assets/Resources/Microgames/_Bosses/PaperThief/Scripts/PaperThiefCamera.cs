﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperThiefCamera : MonoBehaviour
{
	public static PaperThiefCamera instance;

	[SerializeField]
	private Transform follow;
	[SerializeField]
	private float shiftSpeed;

	private BoxCollider2D boxCollider;
	private Vector3 goalPosition;
	private float goalSize, lerpSize, lerpSizeDistance;
	private bool mustShift;

	void Awake()
	{
		instance = this;
		boxCollider = GetComponent<BoxCollider2D>();
		goalSize = Camera.main.orthographicSize;
		goalPosition = transform.localPosition;
	}

	public void setGoalPosition(Vector3 goalPosition)
	{
		this.goalPosition = goalPosition;
		mustShift = true;
	}

	public void setGoalSize(float goalSize)
	{
		this.goalSize = goalSize;
		lerpSize = Camera.main.orthographicSize;
		lerpSizeDistance = ((Vector2)(goalPosition - transform.localPosition)).magnitude;
		mustShift = true;
	}

	public void setFollow(Transform follow)
	{
		this.follow = follow;
	}
	
	void Update()
	{
		if (mustShift)
			updateShift();
		else if (follow != null)
			updateFollow();
	}

	void updateShift()
	{
		if (transform.moveTowardsLocal((Vector2)goalPosition, shiftSpeed))
		{
			Camera.main.orthographicSize = goalSize;
			mustShift = false;
		}
		else
			Camera.main.orthographicSize = Mathf.Lerp(goalSize, lerpSize, ((Vector2)(goalPosition - transform.localPosition)).magnitude / lerpSizeDistance);
		//Debug.Log(lerpSizeDistance + " and " + ((Vector2)(goalPosition - transform.localPosition)).magnitude);
		//Debug.Log("also " + lerpSize + " and " + goalSize);
	}

	void updateFollow()
	{
		boxCollider.enabled = true;

		Vector3 bounds = boxCollider.bounds.extents;
		transform.position = new Vector3(
			Mathf.Clamp(transform.position.x, follow.position.x - bounds.x, follow.position.x + bounds.x),
			Mathf.Clamp(transform.position.y, follow.position.y - bounds.y, follow.position.y + bounds.y),
			transform.position.z);

		boxCollider.enabled = false;
	}
}
