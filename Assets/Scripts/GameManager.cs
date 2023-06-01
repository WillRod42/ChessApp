using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	private bool isPlayerTurn;
	[SerializeField] private bool isPlayerWhitePieces;

	private PieceManager pieceManager;

	public void Initialize(bool playerAsWhitePieces)
	{
		isPlayerWhitePieces = playerAsWhitePieces;
		isPlayerTurn = playerAsWhitePieces;
	}

  private void Start()
  {
    pieceManager = GetComponent<PieceManager>();
		pieceManager.Initialize(isPlayerWhitePieces);
  }

  private void Update()
  {
    
  }
}
