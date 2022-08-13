﻿using Poker.Backend;
using Sandbox;
using Sandbox.UI;
using System;

namespace Poker.UI;

[UseTemplate]
internal class Controls : Panel
{
	//
	// TODO: this shit probably shouldn't be in here
	//
	private bool submitPressedLastFrame;
	private bool foldPressedLastFrame;
	private float rawBet;
	private int roundedBet;

	public Label ActionLabel { get; set; }
	public Money ValueLabel { get; set; }
	public Panel PlayControlsPanel { get; set; }

	private float incrementRate => 50f;
	private int snapRate => 10;

	public override void Tick()
	{
		base.Tick();

		if ( Local.Pawn is not Player player )
			return;

		PlayControlsPanel.SetClass( "visible", player.IsMyTurn );

		if ( !player.IsMyTurn )
		{
			rawBet = 0;
			return;
		}

		ProcessInputs( out var submitPressed, out var foldPressed, out var betDelta );
		ProcessSubmitInput( submitPressed );
		ProcessFoldInput( foldPressed );
		ProcessBetInput( betDelta );

		submitPressedLastFrame = submitPressed;
	}

	private void ProcessSubmitInput( bool submitPressed )
	{
		if ( !submitPressedLastFrame && submitPressed )
			PokerControllerEntity.SubmitMove( Move.Bet, roundedBet );
	}

	private void ProcessFoldInput( bool foldPressed )
	{
		if ( !foldPressedLastFrame && foldPressed )
			PokerControllerEntity.SubmitMove( Move.Fold, 0 );
	}

	private void ProcessBetInput( float betDelta )
	{
		if ( MathF.Abs( betDelta ) > 0.5f )
			rawBet += betDelta * Time.Delta * incrementRate;

		rawBet = rawBet.Clamp( PokerControllerEntity.Instance.MinimumBet, 5000 );
		roundedBet = snapRate * (rawBet.CeilToInt() / snapRate);

		var action = PokerUtils.GetMoveName( roundedBet );
		ActionLabel.Text = $"{action}";
		ValueLabel.Text = $"{roundedBet}";
	}

	private void ProcessInputs( out bool submitPressed, out bool foldPressed, out float betDelta )
	{
		submitPressed = InputLayer.Evaluate( "submit" ) > 0.5f;
		foldPressed = InputLayer.Evaluate( "fold" ) > 0.5f;
		betDelta = InputLayer.Evaluate( "adjust_amount" );
	}
}
