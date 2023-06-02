using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceOnClick : MonoBehaviour
{
	public static GameObject selectedPiece;
	public Piece piece;

	public void Initialize(Piece piece)
	{
		this.piece = piece;
	}

	public void OnMouseDown()
	{
		Debug.Log("clicked " + name + " at " + piece.cell.location);			
		if (selectedPiece != gameObject)
		{
			BoardManager.UnhighlightCells();
			BoardManager.HighlightCells(PieceManager.GetValidMoves(piece));
			selectedPiece = gameObject;
		}
		else
		{
			BoardManager.UnhighlightCells();
			selectedPiece = null;
		}
	}
}
