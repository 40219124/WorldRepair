using System;
using System.Collections.Generic;
using UnityEngine;

public struct TileObjects
{
	public SpriteRenderer MainTile;
	public SpriteRenderer Overlay;
}

public class WorldManager : MonoBehaviour
{
	public static WorldManager Instance;

	public int Width = 10;
	public int Height = 10;
	public SpriteRenderer MainTilePrefab;
	public SpriteRenderer OverlayPrefab;

	public GroundStyle DefaultStyle;
	public GroundStyle FertileStyle;
	public GroundStyle GrassStyle;
	public float managedLerpSpeed;

	public bool HasRained = false;

	private TileObjects[,] Map;

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		var tileRotation = Quaternion.Euler(90, 0, 0);

		Map = new TileObjects[Width, Height];
		for (int x = 0; x < Width; x++)
		{
			for (int y = 0; y < Height; y++)
			{
				var mainTile = Instantiate(MainTilePrefab, new Vector3(x - (Width * 0.5f), 0, y - (Height * 0.5f)), tileRotation, transform);
				var overlayTile = Instantiate(OverlayPrefab, new Vector3(x - (Width * 0.5f), 0, y - (Height * 0.5f)), tileRotation, transform);

				mainTile.gameObject.SetActive(true);
				overlayTile.gameObject.SetActive(false);

				mainTile.sprite = DefaultStyle.RandomSprite();

				Map[x, y] = new TileObjects()
				{
					MainTile = mainTile,
					Overlay = overlayTile
				};
			}
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Z))
		{
			StartCoroutine(FadeToStyle(DefaultStyle, 1.0f));
		}
		if (Input.GetKeyDown(KeyCode.X))
		{
			StartCoroutine(FadeToStyle(FertileStyle, 1.0f));
		}
		if (Input.GetKeyDown(KeyCode.C))
		{
			StartCoroutine(FadeToStyle(GrassStyle, 1.0f));
		}
	}

	public IEnumerator<YieldInstruction> FadeToStyle(GroundStyle newStyle, float duration)
	{
		for (int x = 0; x < Width; x++)
		{
			for (int y = 0; y < Height; y++)
			{
				var tile = Map[x, y];

				tile.Overlay.sprite = newStyle.RandomSprite();
				tile.Overlay.gameObject.SetActive(true);
				tile.Overlay.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
			}
		}

		foreach (float time in new TimedLoop(duration))
		{
			for (int x = 0; x < Width; x++)
			{
				for (int y = 0; y < Height; y++)
				{
					var tile = Map[x, y];

					tile.Overlay.gameObject.SetActive(true);
					tile.Overlay.color = new Color(1.0f, 1.0f, 1.0f, time);
				}
			}

			yield return null;
		}

		for (int x = 0; x < Width; x++)
		{
			for (int y = 0; y < Height; y++)
			{
				var tile = Map[x, y];

				tile.MainTile.sprite = tile.Overlay.sprite;
				tile.Overlay.gameObject.SetActive(false);
			}
		}
	}

	public IEnumerator<YieldInstruction> ManagedFade(GroundStyle newStyle, Func<float> value)
	{
		for (int x = 0; x < Width; x++)
		{
			for (int y = 0; y < Height; y++)
			{
				var tile = Map[x, y];

				tile.Overlay.sprite = newStyle.RandomSprite();
				tile.Overlay.gameObject.SetActive(true);
				tile.Overlay.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
			}
		}

		float currentValue = 0.0f;

		while (true)
		{
			float targetValue = value();

			currentValue = Mathf.Lerp(currentValue, targetValue, managedLerpSpeed * Time.deltaTime);

			for (int x = 0; x < Width; x++)
			{
				for (int y = 0; y < Height; y++)
				{
					var tile = Map[x, y];

					tile.Overlay.gameObject.SetActive(true);
					tile.Overlay.color = new Color(1.0f, 1.0f, 1.0f, currentValue);
				}
			}

			yield return null;
		}
	}
}
