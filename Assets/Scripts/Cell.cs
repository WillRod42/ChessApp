using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
	public int location;
	public Piece piece;
	public GameObject cellObj;

	private Color defaultColor;
	private Color captureColor;

	public Cell(int location)
	{
		this.location = location;
		defaultColor = new Color(255, 225, 58);
		captureColor = Color.red;
	}

	public void SetColor(bool isCapture)
	{
		cellObj.GetComponentInChildren<SpriteRenderer>().color = isCapture ? captureColor : defaultColor;
	}
}
