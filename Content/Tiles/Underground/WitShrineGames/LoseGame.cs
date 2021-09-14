﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace StarlightRiver.Content.Tiles.Underground.WitShrineGames
{
	class LoseGame : WitShrineGame
	{
		public LoseGame(WitShrineDummy parent) : base(parent) { }

		public override void SetupBoard()
		{
			parent.ResetBoard();
		}

		public override void UpdateBoard()
		{
			for (int k = 0; k <= 6; k++)
			{
				if (parent.Timer == 30 * k)
				{
					for(int x = -k; x <= k; x++)
						for (int y = -k; y <= k; y++)
						{
							int realX = Clamp(parent.player.X + x);
							int realY = Clamp(parent.player.Y + y);

							gameBoard[realX, realY] = WitShrineDummy.runeState.Hostile;
						}

					parent.playerTimer = 30;
				}
			}

			if (parent.Timer == 210)
				parent.GotoGame(0);
		}
	}
}