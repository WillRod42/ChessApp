using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceManager : MonoBehaviour
{
	[SerializeField] private GameObject wPawn, wBishop, wKnight, wRook, wQueen, wKing, bPawn, bBishop, bKnight, bRook, bQueen, bKing;
	[SerializeField] private GameObject board;

	public static bool whitesTurn;
	public static bool playerIsWhitePieces;
	public static Piece enPassant;
	public static Cell enPassantCell;
	public static List<Cell> castleCells;
	public static List<Piece> castleRooks;

	private static List<Piece> whitePieces;
	private static List<Piece> blackPieces;
	private static GameObject pro_wBishop, pro_wKnight, pro_wRook, pro_wQueen, pro_bBishop, pro_bKnight, pro_bRook, pro_bQueen;

	public enum PieceType
	{
		pawn,
		bishop,
		knight,
		rook,
		queen,
		king
	}

	public void Initialize()
	{
		whitePieces = new List<Piece>();
		blackPieces = new List<Piece>();
		castleCells = new List<Cell>();
		castleRooks = new List<Piece>();
		whitesTurn = true;

		pro_wBishop = wBishop;
		pro_wKnight = wKnight;
		pro_wRook = wRook;
		pro_wQueen = wQueen;
		pro_bBishop = bBishop;
		pro_bKnight = bKnight;
		pro_bRook = bRook;
		pro_bQueen = bQueen;
		
		for (int i = 0; i < BoardManager.BOARD_WIDTH; i++)
		{
			if (playerIsWhitePieces)
			{
				whitePieces.Add(new Piece(Instantiate(wPawn), PieceType.pawn, BoardManager.GetCell(1 + i, 2), true));
				blackPieces.Add(new Piece(Instantiate(bPawn), PieceType.pawn, BoardManager.GetCell(1 + i, 7), false));
				
			}
			else
			{
				whitePieces.Add(new Piece(Instantiate(wPawn), PieceType.pawn, BoardManager.GetCell(1 + i, 7), true));
				blackPieces.Add(new Piece(Instantiate(bPawn), PieceType.pawn, BoardManager.GetCell(1 + i, 2), false));
			}

			blackPieces[i].pieceObj.GetComponent<BoxCollider2D>().enabled = false;
		}
		int whitePiecesLocation;
		int blackPiecesLocation;
		if (playerIsWhitePieces)
		{
			whitePiecesLocation = 11;
			blackPiecesLocation = 18;
		}
		else
		{
			whitePiecesLocation = 18;
			blackPiecesLocation = 11;
		}

		whitePieces.Add(new Piece(Instantiate(wRook), PieceType.rook, BoardManager.GetCell(whitePiecesLocation), true));
		whitePieces.Add(new Piece(Instantiate(wKnight), PieceType.knight, BoardManager.GetCell(whitePiecesLocation + 10), true));
		whitePieces.Add(new Piece(Instantiate(wBishop), PieceType.bishop, BoardManager.GetCell(whitePiecesLocation + 20), true));
		whitePieces.Add(new Piece(Instantiate(wQueen), PieceType.queen, BoardManager.GetCell(whitePiecesLocation + 30), true));
		whitePieces.Add(new Piece(Instantiate(wKing), PieceType.king, BoardManager.GetCell(whitePiecesLocation + 40), true));
		whitePieces.Add(new Piece(Instantiate(wBishop), PieceType.bishop, BoardManager.GetCell(whitePiecesLocation + 50), true));
		whitePieces.Add(new Piece(Instantiate(wKnight), PieceType.knight, BoardManager.GetCell(whitePiecesLocation + 60), true));
		whitePieces.Add(new Piece(Instantiate(wRook), PieceType.rook, BoardManager.GetCell(whitePiecesLocation + 70), true));

		blackPieces.Add(new Piece(Instantiate(bRook), PieceType.rook, BoardManager.GetCell(blackPiecesLocation), false));
		blackPieces.Add(new Piece(Instantiate(bKnight), PieceType.knight, BoardManager.GetCell(blackPiecesLocation + 10), false));
		blackPieces.Add(new Piece(Instantiate(bBishop), PieceType.bishop, BoardManager.GetCell(blackPiecesLocation + 20), false));
		blackPieces.Add(new Piece(Instantiate(bQueen), PieceType.queen, BoardManager.GetCell(blackPiecesLocation + 30), false));
		blackPieces.Add(new Piece(Instantiate(bKing), PieceType.king, BoardManager.GetCell(blackPiecesLocation + 40), false));
		blackPieces.Add(new Piece(Instantiate(bBishop), PieceType.bishop, BoardManager.GetCell(blackPiecesLocation + 50), false));
		blackPieces.Add(new Piece(Instantiate(bKnight), PieceType.knight, BoardManager.GetCell(blackPiecesLocation + 60), false));
		blackPieces.Add(new Piece(Instantiate(bRook), PieceType.rook, BoardManager.GetCell(blackPiecesLocation + 70), false));

		if (!playerIsWhitePieces)
		{
			Piece temp = blackPieces[11];
			blackPieces[11] = blackPieces[12];
			blackPieces[12] = temp;

			Piece temp2 = whitePieces[11];
			whitePieces[11] = whitePieces[12];
			whitePieces[12] = temp;
		}

		foreach(Piece piece in whitePieces)
		{
			piece.pieceObj.AddComponent<PieceOnClick>();
			piece.pieceObj.GetComponent<PieceOnClick>().Initialize(piece);
		}
		foreach(Piece piece in blackPieces)
		{
			piece.pieceObj.AddComponent<PieceOnClick>();
			piece.pieceObj.GetComponent<PieceOnClick>().Initialize(piece);
		}
	}

	public List<Piece> GetWhitePieces()
	{
		return whitePieces;
	}

	public List<Piece> GetBlackPieces()
	{
		return blackPieces;
	}

	public static Piece GetPiece(string name)
	{
		return null;
	}

	public static void PromotePiece(Piece pawn, PieceType newType)
	{
		pawn.pieceType = newType;
		Vector3 pieceObjPos = pawn.pieceObj.transform.position;
		Destroy(pawn.pieceObj);

		if (pawn.isWhitePiece)
		{
			switch (newType)
			{
				case PieceType.rook: pawn.pieceObj = Instantiate(pro_wRook, pieceObjPos, Quaternion.identity); break;
				case PieceType.knight: pawn.pieceObj = Instantiate(pro_wKnight, pieceObjPos, Quaternion.identity); break;
				case PieceType.bishop: pawn.pieceObj = Instantiate(pro_wBishop, pieceObjPos, Quaternion.identity); break;
				default: pawn.pieceObj = Instantiate(pro_wQueen, pieceObjPos, Quaternion.identity); break;
			}
		}
		else
		{
			switch (newType)
			{
				case PieceType.rook: pawn.pieceObj = Instantiate(pro_bRook, pieceObjPos, Quaternion.identity); break;
				case PieceType.knight: pawn.pieceObj = Instantiate(pro_bKnight, pieceObjPos, Quaternion.identity); break;
				case PieceType.bishop: pawn.pieceObj = Instantiate(pro_bBishop, pieceObjPos, Quaternion.identity); break;
				default: pawn.pieceObj = Instantiate(pro_bQueen, pieceObjPos, Quaternion.identity); break;
			}
		}

		pawn.pieceObj.AddComponent<PieceOnClick>();
		pawn.pieceObj.GetComponent<PieceOnClick>().piece = pawn;
	}

	public static void CapturePiece(Piece capturing, Piece captured)
	{
		if (capturing.isWhitePiece != captured.isWhitePiece)
		{
			captured.pieceObj.SetActive(false);
			Destroy(captured.pieceObj);
			if (captured.isWhitePiece)
			{
				whitePieces.Remove(captured);
			}
			else
			{
				blackPieces.Remove(captured);
			}

			captured.cell.piece = null;
			captured.cell = null;
		}
	}

	public static void TogglePieces()
	{
		if (whitesTurn)
		{
			foreach (Piece piece in whitePieces)
			{
				piece.pieceObj.GetComponent<BoxCollider2D>().enabled = false;
			}
			foreach (Piece piece in blackPieces)
			{
				piece.pieceObj.GetComponent<BoxCollider2D>().enabled = true;
			}
		}
		else
		{
			foreach (Piece piece in whitePieces)
			{
				piece.pieceObj.GetComponent<BoxCollider2D>().enabled = true;
			}
			foreach (Piece piece in blackPieces)
			{
				piece.pieceObj.GetComponent<BoxCollider2D>().enabled = false;
			}
		}
		whitesTurn = !whitesTurn;
	}

	public static List<Cell> GetValidMoves(Piece piece)
	{
		List<Cell> moves = new List<Cell>();
		switch (piece.pieceType)
		{
			case PieceType.pawn: moves = PawnMoves(piece.cell.location, piece.isWhitePiece, piece.hasMoved); break;
			case PieceType.knight: moves = KnightMoves(piece.cell.location, piece.isWhitePiece); break;
			case PieceType.bishop: moves = BoardManager.GetDiagonalCells(piece.cell.location); break;
			case PieceType.rook: moves = BoardManager.GetCellsPlus(piece.cell.location); break;
			case PieceType.queen: moves = QueenMoves(piece.cell.location); break;
			default: moves = KingMoves(piece.cell.location); break;
		}

		return RemoveBlockedCells(RemoveNullFromMoveList(moves), piece.isWhitePiece);
	}

	// Add in checks for moves putting king in check
	private static List<Cell> PawnMoves(int location, bool isWhitePiece, bool hasMoved)
	{
		List<Cell> moves = new List<Cell>();
		int direction = (isWhitePiece ? 1 : -1) * (playerIsWhitePieces ? 1 : -1);
		Cell moveOne = BoardManager.GetCell(location + direction);
		if (moveOne.piece == null)
		{
			moves.Add(moveOne);
			if (!hasMoved)
			{
				Cell moveTwo = BoardManager.GetCell(location + (direction * 2));
				if (moveTwo.piece == null)
					moves.Add(moveTwo);
			}
		}

		Cell captureLeft = BoardManager.GetCell(location - 10 + direction);
		Cell captureRight = BoardManager.GetCell(location + 10 + direction);
		if (captureLeft != null && captureLeft.piece != null && captureLeft.piece.isWhitePiece != isWhitePiece)
		{
			moves.Add(captureLeft);
		}
		else if (enPassant != null && enPassant.cell == BoardManager.GetCell(location - 10))
		{
			moves.Add(captureLeft);
		}

		if (captureRight != null && captureRight.piece != null && captureRight.piece.isWhitePiece != isWhitePiece)
		{
			moves.Add(captureRight);
		}
		else if (enPassant != null && enPassant.cell == BoardManager.GetCell(location + 10))
		{
			moves.Add(captureRight);
		}		
		return moves;
	}

	private static List<Cell> KnightMoves(int location, bool isWhitePiece)
	{
		List<Cell> moves = new List<Cell>();
		moves.Add(BoardManager.GetCell(location - 8));
		moves.Add(BoardManager.GetCell(location + 12));
		moves.Add(BoardManager.GetCell(location + 21));
		moves.Add(BoardManager.GetCell(location + 19));
		moves.Add(BoardManager.GetCell(location + 8));
		moves.Add(BoardManager.GetCell(location - 12));
		moves.Add(BoardManager.GetCell(location - 21));
		moves.Add(BoardManager.GetCell(location - 19));

		return moves;
	}

	private static List<Cell> QueenMoves(int location)
	{
		List<Cell> moves = BoardManager.GetDiagonalCells(location);
		moves.AddRange(BoardManager.GetCellsPlus(location));
		return moves;
	}

	private static List<Cell> KingMoves(int location)
	{
		List<Cell> moves = new List<Cell>();
		moves.AddRange(BoardManager.GetAdjacentCells(location));

		Cell kingCell = BoardManager.GetCell(location);
		Piece king = kingCell.piece;
		castleCells.Clear();
		castleRooks.Clear();

		if (!king.hasMoved)
		{
			List<Cell> cells = new List<Cell>();

			Piece leftRook = BoardManager.GetCell((BoardManager.GetRow(location)) + 10).piece;
			List<Cell> cellsBetween = BoardManager.GetRange(leftRook.cell.location + 10, location - 10);
			if (CheckCastleBetweenCells(leftRook, cellsBetween))
			{
				Cell castleCell = BoardManager.GetCell(location - 20);
				moves.Add(castleCell);
				castleCells.Add(BoardManager.GetCell(castleCell.location + 10));
				castleRooks.Add(leftRook);
			}

			Piece rightRook = BoardManager.GetCell((BoardManager.GetRow(location)) + 80).piece;
			cellsBetween = BoardManager.GetRange(location + 10, rightRook.cell.location - 10);
			if (CheckCastleBetweenCells(rightRook, cellsBetween))
			{
				Cell castleCell = BoardManager.GetCell(location + 20);
				moves.Add(castleCell);
				castleCells.Add(BoardManager.GetCell(castleCell.location - 10));
				castleRooks.Add(rightRook);
			}
		}

		return moves;
	}

	/// <summary>
	/// Returns false if cell is occupied by friendly piece
	/// </summary>
	private static List<Cell> RemoveBlockedCells(List<Cell> moves, bool isWhitePiece)
	{
		List<Cell> filteredMoves = new List<Cell>();
		foreach (Cell cell in moves)
		{
			if (cell.piece == null || cell.piece.isWhitePiece != isWhitePiece)
			{
				filteredMoves.Add(cell);
			}
		}

		return filteredMoves;
	}

	private static List<Cell> RemoveNullFromMoveList(List<Cell> moves)
	{
		List<Cell> filteredMoves = new List<Cell>();
		foreach (Cell cell in moves)
		{
			if (cell != null)
			{
				filteredMoves.Add(cell);
			}
		}

		return filteredMoves;
	}

	private static bool CheckCastleBetweenCells(Piece rook, List<Cell> cellsBetween)
	{
		if (rook == null || rook.hasMoved)
		{
			return false;
		}
		foreach (Cell cell in cellsBetween)
		{
			if (cell.piece != null)
			{
				return false;
			}
		}
		return true;
	}
}
