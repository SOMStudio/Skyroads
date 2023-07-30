using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/SOM/Camera/Vignette")]
public class VignetteSOM : MonoBehaviour
{
	#region Variables
	public Shader curShader;

	[Header("Main")]
	[Range(0.0f, 50.0f)] public float intensity = 0.5f;

	private Material curMaterial;
	
	private static readonly int VignetteIntensity = Shader.PropertyToID("vignetteIntensity");
	#endregion

	#region Properties
	Material material
	{
		get
		{
			if (curMaterial == null)
			{
				curMaterial = new Material(curShader);
				curMaterial.hideFlags = HideFlags.HideAndDontSave;
			}

			return curMaterial;
		}
	}
	#endregion

	private void Start()
	{
		if (!SystemInfo.supportsImageEffects)
		{
			enabled = false;
			return;
		}

		if (!curShader && !curShader.isSupported)
		{
			enabled = false;
		}
	}

	private void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
	{
		if (curShader != null)
		{
			material.SetFloat(VignetteIntensity, intensity);

			Graphics.Blit(sourceTexture, destTexture, curMaterial, 0);
		}
		else
		{
			Graphics.Blit(sourceTexture, destTexture);
		}
	}

	private void OnDisable()
	{
		if (curMaterial)
		{
			DestroyImmediate(curMaterial);
		}
	}
}
