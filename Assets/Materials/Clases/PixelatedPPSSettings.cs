// Amplify Shader Editor - Visual Shader Editing Tool
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>
#if UNITY_POST_PROCESSING_STACK_V2
using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess( typeof( PixelatedPPSRenderer ), PostProcessEvent.AfterStack, "Pixelated", true )]
public sealed class PixelatedPPSSettings : PostProcessEffectSettings
{
	[Tooltip( "OriginalRes" )]
	public Vector4Parameter _OriginalRes = new Vector4Parameter { value = new Vector4(1920f,1080f,0f,0f) };
	[Tooltip( "Intensity" )]
	public FloatParameter _Intensity = new FloatParameter { value = 0.99f };
}

public sealed class PixelatedPPSRenderer : PostProcessEffectRenderer<PixelatedPPSSettings>
{
	public override void Render( PostProcessRenderContext context )
	{
		var sheet = context.propertySheets.Get( Shader.Find( "Pixelated" ) );
		sheet.properties.SetVector( "_OriginalRes", settings._OriginalRes );
		sheet.properties.SetFloat( "_Intensity", settings._Intensity );
		context.command.BlitFullscreenTriangle( context.source, context.destination, sheet, 0 );
	}
}
#endif
