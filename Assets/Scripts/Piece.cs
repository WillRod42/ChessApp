using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece
{
	public GameObject pieceObj;
	public PieceManager.PieceType pieceType;
	public Cell cell;
	public bool isWhitePiece;

	public Piece(GameObject pieceObj, PieceManager.PieceType pieceType, Cell cell, bool isWhitePiece)
	{
		this.pieceObj = pieceObj;
		this.pieceType = pieceType;
		this.cell = cell;
		this.isWhitePiece = isWhitePiece;

		CenterOnCell();
	}

	public void Move(Cell toMove)
	{
		cell.piece = null;
		cell = toMove;
		toMove.piece = this;

		CenterOnCell();
	}

	private void CenterOnCell()
	{
		pieceObj.transform.position = cell.cellObj.transform.position;
	}
}
