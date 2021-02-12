using System;
using System.Collections.Generic;
using UnityEngine;

namespace MinMax.Scripts {
    public class GameManager : MonoBehaviour {

        public GameObject WhiteMenPrefab;
        public GameObject WhiteKingPrefab;
        public GameObject BlackMenPrefab;
        public GameObject BlackKingPrefab;
        public List<Transform> PositionList;
        public Transform[,] PhysicalMatrix;
        public Board Board = new Board();
        public Transform PiecesContent;

        private AI _whiteAI;
        private AI _blackAI;
        private bool _isBlackTurn;

        private void Awake() { 
            GeneratePositionMatrix();
            Board.SetupBoard();
            CreateAI();
            UpdatePhysicalBoard();
        }

        private void Update() {
            if (Input.GetButtonUp("Jump")) {
                if (_isBlackTurn) {
                    _blackAI.Think();
                    _blackAI.Act();
                    UpdatePhysicalBoard();
                }
                else {
                    _whiteAI.Think();
                    _whiteAI.Act();
                    _isBlackTurn = true;
                    UpdatePhysicalBoard();
                }
            }
        }

        private void GeneratePositionMatrix() {
            PhysicalMatrix = new Transform[(int) Mathf.Sqrt(PositionList.Count),(int) Mathf.Sqrt(PositionList.Count)];
            foreach (Transform cellTransform in PositionList) {
                int row = Convert.ToInt32(cellTransform.name.Split('.')[0]);
                int column = Convert.ToInt32(cellTransform.name.Split('.')[1]);
                PhysicalMatrix[row, column] = cellTransform;
            }
        }

        public void CreateAI() {
            _whiteAI = new AI(Board, PlayerColor.White);
            _blackAI = new AI(Board, PlayerColor.Black);
        }
        
        private void UpdatePhysicalBoard() {
            // Clear previous pieces
            foreach (Transform child in PiecesContent) {
                Destroy(child.gameObject);
            }
            // Rebuild all pieces
            for (int i = 0; i < Board.Matrix.GetLength(0); i++) {
                for (int j = 0; j < Board.Matrix.GetLength(1); j++) {
                    if (Board.Matrix[i, j] == null) continue;
                    if (Board.Matrix[i, j].Owner == PlayerColor.White && Board.Matrix[i, j].GetType() == typeof(Men))
                        Instantiate(WhiteMenPrefab, PhysicalMatrix[i, j].position, Quaternion.identity, PiecesContent);
                    if (Board.Matrix[i, j].Owner == PlayerColor.White && Board.Matrix[i, j].GetType() == typeof(King))
                        Instantiate(WhiteKingPrefab, PhysicalMatrix[i, j].position, Quaternion.identity, PiecesContent);
                    if (Board.Matrix[i, j].Owner == PlayerColor.Black && Board.Matrix[i, j].GetType() == typeof(Men))
                        Instantiate(BlackMenPrefab, PhysicalMatrix[i, j].position, Quaternion.identity, PiecesContent);
                    if (Board.Matrix[i, j].Owner == PlayerColor.Black && Board.Matrix[i, j].GetType() == typeof(King))
                        Instantiate(BlackKingPrefab, PhysicalMatrix[i, j].position, Quaternion.identity, PiecesContent);
                }
            }
        }
        
    }
}