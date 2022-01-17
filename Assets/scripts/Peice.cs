using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peice : MonoBehaviour
{
   public Board board { get; private set; }
   public Vector3Int position { get; private set; }
   public TetrominoData data { get; private set; }
   public Vector3Int[] cells { get; private set; }
   
   public void Initialize(Board board, Vector3Int position,TetrominoData data)
   {
      this.board = board;
      this.position = position;
      this.data = data;
      if (cells == null)
      {
         this.cells = new Vector3Int[4];
      }

      for (int i = 0; i < 4; i++)
      {
         this.cells[i] =(Vector3Int) data.cells[i];
      }
   }

   private void Update()
   {
      this.board.clear(this);
      if (Input.GetKeyDown(KeyCode.A))
      {
         move(Vector2Int.left);
         }
      else if (Input.GetKeyDown(KeyCode.D))
      {
         move(Vector2Int.right);
      }
      else if (Input.GetKeyDown(KeyCode.S))
      {
         move(Vector2Int.down);
      }
      else if (Input.GetKeyDown(KeyCode.W))
      {
         move(Vector2Int.up);
      }
      else if (Input.GetKeyDown(KeyCode.Space))
      {
            hardDrop();
      }
      this.board.set(this);
   }

   private void hardDrop()
   {
      while (move(Vector2Int.down))
      {
         continue;
      }
   }

   private bool move(Vector2Int translation)
   {
      Vector3Int newPosition = this.position;
      newPosition.x += translation.x;
      newPosition.y += translation.y;

      bool valid = this.board.isValidPosition(this,newPosition);
      if (valid)
      {
         this.position = newPosition;
      }

      return valid;
   }
}
