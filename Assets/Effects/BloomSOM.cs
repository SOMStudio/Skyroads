using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/SOM/Camera/Bloom")]
public class BloomSOM : MonoBehaviour
{
	#region Variables
	public enum Resolution
	{
		Low = 0,
		High = 1,
	}

	public enum BlurType
	{
		Standard = 0,
		Sgx = 1,
	}

	public Shader curShader;

	[Header("Main")]
	[Range(0.0f, 1.5f)] public float threshold = 0.25f;
	[Range(0.0f, 2.5f)] public float intensity = 0.75f;
	public Resolution resolution = Resolution.Low;

	[Header("Other")]
	[Range(0, 4)] public int blurIterations = 1;
	[Range(0.25f, 5.5f)] public float blurSize = 1.0f;
	public BlurType blurType = BlurType.Standard;

	private Material curMaterial;
	
	private static readonly int Parameter = Shader.PropertyToID("_Parameter");
	private static readonly int Bloom = Shader.PropertyToID("_Bloom");
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
	
	void Start()
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
			int divider = resolution == Resolution.Low ? 4 : 2;
			float widthMod = resolution == Resolution.Low ? 0.5f : 1.0f;

			material.SetVector(Parameter, new Vector4(blurSize * widthMod, 0.0f, threshold, intensity));
			sourceTexture.filterMode = FilterMode.Bilinear;

			var rtW = sourceTexture.width / divider;
			var rtH = sourceTexture.height / divider;
			
			RenderTexture rt = RenderTexture.GetTemporary(rtW, rtH, 0, sourceTexture.format);
			rt.filterMode = FilterMode.Bilinear;
			Graphics.Blit(sourceTexture, rt, material, 1);

			var passOffs = blurType == BlurType.Standard ? 0 : 2;

			for (int i = 0; i < blurIterations; i++)
			{
				material.SetVector(Parameter,
					new Vector4(blurSize * widthMod + (i * 1.0f), 0.0f, threshold, intensity));

				// vertical blur
				RenderTexture rt2 = RenderTexture.GetTemporary(rtW, rtH, 0, sourceTexture.format);
				rt2.filterMode = FilterMode.Bilinear;
				Graphics.Blit(rt, rt2, material, 2 + passOffs);
				RenderTexture.ReleaseTemporary(rt);
				rt = rt2;

				// horizontal blur
				rt2 = RenderTexture.GetTemporary(rtW, rtH, 0, sourceTexture.format);
				rt2.filterMode = FilterMode.Bilinear;
				Graphics.Blit(rt, rt2, material, 3 + passOffs);
				RenderTexture.ReleaseTemporary(rt);
				rt = rt2;
			}

			material.SetTexture(Bloom, rt);

			Graphics.Blit(sourceTexture, destTexture, material, 0);

			RenderTexture.ReleaseTemporary(rt);
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
