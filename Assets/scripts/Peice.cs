using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Peice : MonoBehaviour
{
   public Board board { get; private set; }
   public Vector3Int position { get; private set; }
   public TetrominoData data { get; private set; }
   public Vector3Int[] cells { get; private set; }
   public int rotationIndex { get; private set; }

   public float stepDelay = 1f;
   public float lockDelay = 0.5f;

   private float stepTime;
   private float lockTime;
   public void Initialize(Board board, Vector3Int position,TetrominoData data)
   {
      this.board = board;
      this.position = position;
      this.data = data;
      this.rotationIndex = 0;
      this.stepTime = Time.time + stepDelay;
      this.lockTime = 0f;
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
      this.lockTime += Time.deltaTime;
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
      else if (Input.GetKeyDown(KeyCode.Q))
      {
         rotate(-1);
      }
      else if (Input.GetKeyDown(KeyCode.E))
      {
         rotate(1);
      }

      if (Time.time > this.stepTime)
      {
         step();
      }

      this.board.set(this);
   }

   private void step()
   {
      this.stepTime = Time.time + this.stepDelay;
      move(Vector2Int.down);
      if (this.lockTime >= this.lockDelay)
      {
         Lock();
      }
   }

   private void Lock()
   {
      this.board.set(this);
      this.board.clearLines();
      this.board.spawn();
   }

   private void hardDrop()
   {
      while (move(Vector2Int.down))
      {
         continue;
      }
      Lock();
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
         this.lockTime = 0f;
      }

      return valid;
   }

   private void rotate(int direction)
   {
      int ogRotationIndex = this.rotationIndex;
      this.rotationIndex = wrap(this.rotationIndex + direction, 0, 4);
     applyRotationMatrix(direction);
     if (!testWallKicks(this.rotationIndex, direction))
     {
        this.rotationIndex = ogRotationIndex;
        applyRotationMatrix(-direction);
     }
   }
   
   private void applyRotationMatrix(int direction)
   {
      for(int i=0;i<this.cells.Length;i++)
   {
      Vector3 cell = this.cells[i];
      int x, y;
      switch (this.data.tetromino){
         case Tetromino.I :
         case Tetromino.O:
            cell.x-=0.5f;
            cell.y -= 0.5f;
            x = Mathf.RoundToInt(cell.x * Data.RotationMatrix[0] * direction + cell.y * Data.RotationMatrix[1] * direction);
            y =Mathf.RoundToInt( cell.x * Data.RotationMatrix[2] * direction + cell.y * Data.RotationMatrix[3] * direction);
            break;
                  
         default:
            x = Mathf.RoundToInt(cell.x * Data.RotationMatrix[0] * direction + cell.y * Data.RotationMatrix[1] * direction);
            y =Mathf.RoundToInt( cell.x * Data.RotationMatrix[2] * direction + cell.y * Data.RotationMatrix[3] * direction);
            break;
      }

      this.cells[i] = new Vector3Int(x, y, 0);
   }}

   private bool testWallKicks(int rotationindex, int rotationDirection)
   {
      int wallKicksIndex = getWallKicksIndex(rotationindex, rotationDirection);
      for (int i = 0; i < this.data.wallKicks.GetLength(1);i++)
      {
         Vector2Int translation = this.data.wallKicks[wallKicksIndex, i];
         if (move(translation))
         {
            return true;
         }
      }

      return false;
   }

   private int getWallKicksIndex(int rotationIndex, int rotationDirection)
   {
      int wallKicksIndex = rotationIndex * 2;
      if (rotationDirection < 0)
      {
         rotationIndex--;
      }

      return wrap(wallKicksIndex, 0, this.data.wallKicks.GetLength(0));
   }
   private int wrap(int input, int min, int max)
   {
      if (input < min)
      {
         return max - (min - input) % (max - min);
      }
      else
      {
         return max + (input - min) % (max - min);
      }
   }
   
}
