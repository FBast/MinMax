using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class AIBrain {

    public Board Board;
    public PlayerColor Player;
    public int DepthSearch;
    
    public PlayerColor OtherPlayer => Player == PlayerColor.Black ? PlayerColor.White : PlayerColor.Black;
    
    private List<Tuple<int, Node>> Nodes = new List<Tuple<int, Node>>();

    public AIBrain(Board board, PlayerColor player, int depthSearch) {
        Board = board;
        Player = player;
        DepthSearch = depthSearch;
    }

    public void Think() {
        Nodes.Clear();
        float startingTime = Time.realtimeSinceStartup;
        Debug.Log("Thinking...");
        foreach (Piece availablePiece in Board.AvailablePieces(Player)) {
            foreach (Coordinate availableMove in availablePiece.AvailableMoves(Board)) {
                Node node = new Node(Board, Player, availablePiece.CurrentCoordinate, availableMove);
//                int value = MinMax(node, DepthSearch, true);
//                int value = MinMaxAlphaBeta(node, DepthSearch, int.MinValue, int.MaxValue, true);
//                int value = NegaMax(node, DepthSearch, 1);
                int value = NegaMaxAlphaBeta(node, DepthSearch, int.MinValue, int.MaxValue, 1);
                Nodes.Add(new Tuple<int, Node>(value, node));
            }
        }
        Debug.Log("Reflexion took about : " + (Time.realtimeSinceStartup - startingTime) + " seconds");
        for (int i = 0; i < Nodes.Count; i++) {
            Debug.Log(i + " - Points : " + Nodes[i].Item1 + " From " + Nodes[i].Item2.MoveOrigin + " To " + Nodes[i].Item2.MoveDestination);
        }
    }
    
    public void Act() {
        if (Nodes.Count == 0) throw new Exception("MinMax results is empty");
        int bestValue = Nodes.Max(node => node.Item1);
        Nodes.RemoveAll(node => node.Item1 < bestValue);
        Tuple<int, Node> selectedTuple = Nodes[Random.Range(0, Nodes.Count)];
        Debug.Log("Choose with Points : " + selectedTuple.Item1 + " From " + selectedTuple.Item2.MoveOrigin + " To " + selectedTuple.Item2.MoveDestination);
        Board.GetPiece(selectedTuple.Item2.MoveOrigin).ExecuteMove(Board, selectedTuple.Item2.MoveDestination);
    }
        
    private int MinMax(Node node, int depth, bool isMax) {
        if (depth == 0 || node.IsTerminal)
            return node.HeuristicValue;
        int value;
        if (isMax) {
            value = int.MinValue;
            foreach (Node child in node.Children) {
                value = Mathf.Max(value, MinMax(child, depth - 1, false));
            }
        }
        else {
            value = int.MaxValue;
            foreach (Node child in node.Children) {
                value = Mathf.Min(value, MinMax(child, depth - 1, true));
            }
        }
        return value + node.HeuristicValue * (isMax ? 1 : -1);
    }

    // int result = AlphaBetaMinMax(node, Depth, int.MinValue, int.MaxValue, false);
    private int MinMaxAlphaBeta(Node node, int depth, int alpha, int beta, bool isMax) {
        if (depth == 0 || node.IsTerminal)
            return node.HeuristicValue;
        int value;
        if (isMax) {
            value = int.MinValue;
            foreach (Node child in node.Children) {
                value = Mathf.Max(value, MinMaxAlphaBeta(child, depth - 1, alpha, beta, false));
                if (value >= beta) return value + node.HeuristicValue;
                alpha = Mathf.Max(alpha, value);
            }
        }
        else {
            value = int.MaxValue;
            foreach (Node child in node.Children) {
                value = Mathf.Min(value, MinMaxAlphaBeta(child, depth - 1, alpha, beta, true));
                if (alpha >= value) return value + node.HeuristicValue;
                beta = Mathf.Min(beta, value);
            }
        }
        return value + node.HeuristicValue * (isMax ? 1 : -1);
    }
    
    private int NegaMax(Node node, int depth, int color) {
        if (depth == 0 || node.IsTerminal)
            return color * node.HeuristicValue;
        int value = int.MinValue;
        foreach (Node child in node.Children) {
            int childValue = -NegaMax(child, depth - 1, -color);
            value = Mathf.Max(value, childValue);
        }
        return color * node.HeuristicValue + value;
    } 
    
    private int NegaMaxAlphaBeta(Node node, int depth, int alpha, int beta, int color) {
        if (depth == 0 || node.IsTerminal)
            return color * node.HeuristicValue;
        int value = int.MinValue;
        foreach (Node child in node.Children) {
            value = Mathf.Max(value, -NegaMaxAlphaBeta(child, depth - 1, -beta, -alpha, -color));
            alpha = Mathf.Max(alpha, value);
            if (alpha >= beta) break;
        }
        return color * node.HeuristicValue + value;
    }
    
}