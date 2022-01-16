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
    set(this.activePeice);
  }

  public void set(Peice peice)
  {
    for (int i = 0; i < peice.cells.Length; i++)
    {
      Vector3Int tilePosition = peice.cells[i]+peice.position;
      this.tilemap.SetTile(tilePosition,peice.data.tile);
    }
    
  }
}
