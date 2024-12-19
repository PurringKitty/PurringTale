using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using PurringTale.Content.Items.Consumables;

namespace PurringTale.Common.Players
{
    public class WHY : ModPlayer
    {
         public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
            {
                return new[]
                {
            new Item(ModContent.ItemType<BoxOfStuff>(), 1),

                };
         }
    }
}
