﻿namespace Poker;
partial class PokerGame
{
	class PlayerQueue
	{
		private Queue<Player> InternalQueue { get; set; }

		public void CreateQueue( List<Player> players )
		{
			InternalQueue = new(
				players.Where( x =>
				{
					return !x.HasFolded;
				} )
			);

			// Sort InternalQueue based on seat index
			InternalQueue = new(
				InternalQueue.OrderBy( x => x.Seat.SeatNumber )
			);
		}

		public Player Pop()
		{
			return InternalQueue.Dequeue();
		}

		public Player Peek()
		{
			return InternalQueue.Peek();
		}

		public int Count => InternalQueue.Count;
	}

	private PlayerQueue PlayerTurnQueue { get; set; }
}
