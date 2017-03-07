﻿using UnityEngine;
using System.Collections.Generic;

public class MeshStitcher
{
    // CREDITS TO masterprompt FOR THE SCRIPT
	public GameObject Stitch (GameObject sourceClothing, GameObject targetAvatar)
	{
		var boneCatalog = new TransformCatalog (targetAvatar.transform);
		var skinnedMeshRenderers = sourceClothing.GetComponentsInChildren<SkinnedMeshRenderer> ();
		var targetClothing = AddChild (sourceClothing, targetAvatar.transform);
		foreach (var sourceRenderer in skinnedMeshRenderers) {
			var targetRenderer = AddSkinnedMeshRenderer (sourceRenderer, targetClothing);
			targetRenderer.bones = TranslateTransforms (sourceRenderer.bones, boneCatalog);
		}
		return targetClothing;
	}

	private GameObject AddChild (GameObject source, Transform parent)
	{
		var target = new GameObject (source.name);
		target.transform.parent = parent;
		target.transform.localPosition = source.transform.localPosition;
		target.transform.localRotation = source.transform.localRotation;
		target.transform.localScale = source.transform.localScale;
		return target;
	}

	private SkinnedMeshRenderer AddSkinnedMeshRenderer (SkinnedMeshRenderer source, GameObject parent)
	{
		var target = parent.AddComponent<SkinnedMeshRenderer> ();
		target.sharedMesh = source.sharedMesh;
		target.materials = source.materials;
		return target;
	}

	private Transform[] TranslateTransforms (Transform[] sources, TransformCatalog transformCatalog)
	{
		var targets = new Transform[sources.Length];
		for (var index = 0; index < sources.Length; index++)
			targets [index] = DictionaryExtensions.Find (transformCatalog, sources [index].name);
		return targets;
	}

	private class TransformCatalog : Dictionary<string, Transform>
	{

		public TransformCatalog (Transform transform)
		{
			Catalog (transform);
		}



		private void Catalog (Transform transform)
		{
			Add (transform.name, transform);
			foreach (Transform child in transform)
				Catalog (child);
		}

	}

	private class DictionaryExtensions
	{
		public static TValue Find<TKey, TValue> (Dictionary<TKey, TValue> source, TKey key)
		{
			TValue value;
			source.TryGetValue (key, out value);
			return value;
		}
	}
}

