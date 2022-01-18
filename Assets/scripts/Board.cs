using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.VFX;
using  UnityEngine;
using Random = UnityEngine.Random;

public class Board : MonoBehaviour
{
  public Tilemap tilemap { get; private set; }
  public TetrominoData[] tetrominoes;
  public Peice activePeice { get; private set; }
  public Vector3Int activePosition;
  public Vector2Int boardSize = new Vector2Int(10, 20);

  public RectInt bounds
  {
    get
    {
      Vector2Int position = new Vector2Int(-this.boardSize.x / 2, -this.boardSize.y / 2);
      return new RectInt(position, this.boardSize);
    }
  }
  
  private void Awake()
  {
    this.tilemap = GetComponentInChildren<Tilemap>();    //to get the tilemap which is a children of go.
    this.activePeice = GetComponentInChildren<Peice>();
    for (int i = 0; i < tetrominoes.Length; i++)
    {
      this.tetrominoes[i].Initialize();
    }
  }

  private void Start()
  {
    spawn();
  }

  public void spawn()
  {
    int random = Random.Range(0, this.tetrominoes.Length);
    TetrominoData data = this.tetrominoes[random];
    this.activePeice.Initialize(this,activePosition ,data);
    if (isValidPosition(this.activePeice, this.activePosition))
    {
      set(this.activePeice);
    }
    else
    {
      gameover();
    }
  }

  private void gameover()
  {
    this.tilemap.ClearAllTiles();
  }

  public void set(Peice peice)
  {
    for (int i = 0; i < peice.cells.Length; i++)
    {
      Vector3Int tilePosition = peice.cells[i]+peice.position;
      this.tilemap.SetTile(tilePosition,peice.data.tile);
    }
    
  }
  public void clear(Peice peice)
  {
    for (int i = 0; i < peice.cells.Length; i++)
    {
      Vector3Int tilePosition = peice.cells[i]+peice.position;
      this.tilemap.SetTile(tilePosition,null);
    }
    
  }

  public bool isValidPosition(Peice peice, Vector3Int position)
  {
    RectInt bounds = this.bounds;
    for (int i = 0; i < peice.cells.Length; i++)
    {
      Vector3Int tilePosition = peice.cells[i] + position;
      if (!bounds.Contains((Vector2Int)tilePosition))
      {
        return false;
      }
      if (this.tilemap.HasTile(tilePosition))
      {
        return false;
      }
      
    }

    return true;
  }

  public void clearLines()
  {
    RectInt bounds = this.bounds;
    int row = bounds.yMin;

    while (row < bounds.yMax)
    {
      if (isLineFull(row))
      {
        lineClear(row);
      }
      else
      {
        row++;
      }
    }

  }

  private bool isLineFull(int row)
  {
    RectInt bounds = this.bounds;
    for (int col = bounds.xMin; col < bounds.xMax; col++)
    {
      Vector3Int position = new Vector3Int(col, row, 0);
      if (!this.tilemap.HasTile(position))
      {
        return false;
      }
    }

    return true;
  }

  private void lineClear(int row)
  {
    RectInt bounds = this.bounds;
    for (int col = bounds.xMin; col < bounds.xMax; col++)
    {
      Vector3Int position = new Vector3Int(col, row, 0);
      this.tilemap.SetTile(position, null);
    }

    while (row < bounds.yMax)
    {
      for (int col = bounds.xMin; col < bounds.xMax; col++)
      {
        Vector3Int position = new Vector3Int(col, row+1, 0);
        TileBase aboveTile = this.tilemap.GetTile(position);
        position = new Vector3Int(col, row, 0);
        this.tilemap.SetTile(position,aboveTile);
      }

      row++;
    }
  }
}
