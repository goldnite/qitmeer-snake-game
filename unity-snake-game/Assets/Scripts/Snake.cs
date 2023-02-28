﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D))]
public class Snake : MonoBehaviour
{
  private List<Transform> segments = new List<Transform>();
  public Transform segmentPrefab;
  public Vector2 direction = Vector2.right;
  private Vector2 input;
  public int initialSize = 4;
  public int counter = 0;
  public int startTime;

  public bool isPlaying;
  public const int speedStep = 20;
  public const int speedIncreaseInterval = 10;
  private void StartGame()
  {
    ResetState();
    isPlaying = true;
  }

  private void Update()
  {
    if (!isPlaying) return;
    // Only allow turning up or down while moving in the x-axis
    if (direction.x != 0f)
    {
      if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
      {
        input = Vector2.up;
      }
      else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
      {
        input = Vector2.down;
      }
    }
    // Only allow turning left or right while moving in the y-axis
    else if (direction.y != 0f)
    {
      if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
      {
        input = Vector2.right;
      }
      else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
      {
        input = Vector2.left;
      }
    }
  }

  private void FixedUpdate()
  {
    if (!isPlaying) return;

    int delta = (int)(Time.time - startTime) / speedIncreaseInterval;
    if (++counter < ((speedStep - delta) >= 1 ? (speedStep - delta) : 1)) return;
    counter = 0;

    // Set the new direction based on the input
    if (input != Vector2.zero)
    {
      direction = input;
    }

    // Set each segment's position to be the same as the one it follows. We
    // must do this in reverse order so the position is set to the previous
    // position, otherwise they will all be stacked on top of each other.
    for (int i = segments.Count - 1; i > 0; i--)
    {
      segments[i].position = segments[i - 1].position;
    }

    // Move the snake in the direction it is facing
    // Round the values to ensure it aligns to the grid
    float x = Mathf.Round(transform.position.x) + direction.x;
    float y = Mathf.Round(transform.position.y) + direction.y;

    transform.position = new Vector2(x, y);
  }

  public void Grow()
  {
    Transform segment = Instantiate(segmentPrefab);
    segment.position = segments[segments.Count - 1].position;
    segments.Add(segment);
  }

  public void ResetState()
  {
    direction = Vector2.right;
    transform.position = Vector3.zero;

    // Start at 1 to skip destroying the head
    for (int i = 1; i < segments.Count; i++)
    {
      Destroy(segments[i].gameObject);
    }

    // Clear the list but add back this as the head
    segments.Clear();
    segments.Add(transform);

    // -1 since the head is already in the list
    for (int i = 0; i < initialSize - 1; i++)
    {
      Grow();
    }
    startTime = (int)Time.time;
    counter = speedStep;
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.gameObject.CompareTag("Food"))
    {
      Grow();
    }
    else if (other.gameObject.CompareTag("Obstacle"))
    {
      GameObject.Find("Landing").SendMessage("EndGame", segments.Count);
      isPlaying = false;
    }
  }

}
