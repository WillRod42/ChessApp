using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Promotion : MonoBehaviour
{
	[SerializeField] private GameObject promotionUI;
	private static GameObject sPromotionUI;

	private static Piece promotingPawn;

	private void Awake()
	{
		sPromotionUI = promotionUI;
	}

	public static void ToggleUI(Piece pawn)
	{
		promotingPawn = pawn;
		sPromotionUI.SetActive(true);
	}

	public static void SelectPromotion(string pieceType)
	{
		switch (pieceType)
		{
			case "rook": PieceManager.PromotePiece(promotingPawn, PieceManager.PieceType.rook); break;
			case "knight": PieceManager.PromotePiece(promotingPawn, PieceManager.PieceType.knight); break;
			case "bishop": PieceManager.PromotePiece(promotingPawn, PieceManager.PieceType.bishop); break;
			default: PieceManager.PromotePiece(promotingPawn, PieceManager.PieceType.queen); break;
		}

		sPromotionUI.SetActive(false);
		promotingPawn = null;
	}
}
