// Amplify Shader Editor - Visual Shader Editing Tool
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>
#if UNITY_POST_PROCESSING_STACK_V2
using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess( typeof( StackTestPPSRenderer ), PostProcessEvent.AfterStack, "StackTest", true )]
public sealed class StackTestPPSSettings : PostProcessEffectSettings
{
	[Tooltip( "Intensity" )]
	public FloatParameter _Intensity = new FloatParameter { value = 0f };
}

public sealed class StackTestPPSRenderer : PostProcessEffectRenderer<StackTestPPSSettings>
{
	public override void Render( PostProcessRenderContext context )
	{
		var sheet = context.propertySheets.Get( Shader.Find( "StackTest" ) );
		sheet.properties.SetFloat( "_Intensity", settings._Intensity );
		context.command.BlitFullscreenTriangle( context.source, context.destination, sheet, 0 );
	}
}
#endif
