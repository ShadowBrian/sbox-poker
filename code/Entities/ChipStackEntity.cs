﻿namespace Poker;

[Title( "Chip Stack" ), Category( "Poker" )]
public partial class ChipStackEntity : Entity
{
	[Net, Change( nameof( SetParticleProperties ) )] public float Value { get; set; }
	[Net, Change( nameof( SetParticleProperties ) )] public int Count { get; set; }
	public Particles Particles { get; set; }

	public override void Spawn()
	{
		base.Spawn();

		Transmit = TransmitType.Always;
	}

	public override void ClientSpawn()
	{
		base.ClientSpawn();
		SetParticleProperties();
	}

	private void SetParticleProperties()
	{
		if ( Particles == null )
		{
			Particles = Particles.Create( "particles/chip_stack/chip_stack.vpcf" );
			return;
		}

		var color = Value switch
		{
			500 => Color.Parse( "#991e38" ) ?? Color.Black,
			250 => Color.Parse( "#1f4e99" ) ?? Color.Blue,
			100 => Color.Parse( "#e31a3b" ) ?? Color.Red,
			50 => Color.Parse( "#eb8344" ) ?? Color.Orange,
			_ => Color.Red
		};

		if ( Count <= 0 )
			Particles.Destroy();

		Particles.SetPosition( 0, Position );
		Particles.SetPositionComponent( 1, 0, Count );
		Particles.SetPosition( 2, new Vector3( color.r, color.g, color.b ) );
	}

	public static ChipStackEntity CreateStack( int count, float value, Vector3 position )
	{
		return new ChipStackEntity()
		{
			Count = count,
			Value = value,
			Position = position
		};
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();

		if ( Game.IsClient && Particles != null )
			Particles.Destroy( true );
	}
}
