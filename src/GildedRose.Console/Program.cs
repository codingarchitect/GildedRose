using System.Collections.Generic;
using System.Linq;

namespace GildedRose.Console
{
    public class Program
    {
        public Program(IList<Item> items)
        {
            Items = items;
        }

        public Program()
        {

        }

        IList<Item> Items;
        public Item FindItemByName(string name)
        {
            if (Items != null)
            {
                return Items.FirstOrDefault(i => i.Name.Equals(name));
            }
            return null;
        }

        private static IDictionary<string, IInventoryUpdateStrategy> InventoryUpdateStrategyMap = new Dictionary<string, IInventoryUpdateStrategy>
        {
            { "Aged Brie", new ImprovingQualityItemInventoryUpdateStrategy() },
            { "Sulfuras, Hand of Ragnaros", new LegendaryItemInventoryUpdateStrategy() },
            { "Backstage passes to a TAFKAL80ETC concert", new BackStagePassItemInventoryUpdateStrategy() },
            { "Conjured Mana Cake", new ConjuredItemInventoryUpdateStrategy() }        
        };

        static void Main(string[] args)
        {
            System.Console.WriteLine("OMGHAI!");

            var app = new Program()
                          {
                              Items = new List<Item>
                                          {
                                              new Item {Name = "+5 Dexterity Vest", SellIn = 10, Quality = 20},
                                              new Item {Name = "Aged Brie", SellIn = 2, Quality = 0},
                                              new Item {Name = "Elixir of the Mongoose", SellIn = 5, Quality = 7},
                                              new Item {Name = "Sulfuras, Hand of Ragnaros", SellIn = 0, Quality = 80},
                                              new Item
                                                  {
                                                      Name = "Backstage passes to a TAFKAL80ETC concert",
                                                      SellIn = 15,
                                                      Quality = 20
                                                  },
                                              new Item {Name = "Conjured Mana Cake", SellIn = 3, Quality = 6}
                                          }

                          };

            app.UpdateQuality();

            System.Console.ReadKey();

        }

        public void UpdateQuality()
        {
            foreach (var item in Items)
            {
                IInventoryUpdateStrategy inventoryUpdateStrategy = new PerishableQualityItemInventoryUpdateStrategy();
                if (InventoryUpdateStrategyMap.ContainsKey(item.Name))
                    inventoryUpdateStrategy = InventoryUpdateStrategyMap[item.Name];
                inventoryUpdateStrategy.UpdateInventory(item);
            }
        }        
    }

    public class Item
    {
        public string Name { get; set; }

        public int SellIn { get; set; }

        public int Quality { get; set; }
    }

    public interface IInventoryUpdateStrategy
    {
        void UpdateInventory(Item item);
    }

    public abstract class DefaultInventoryUpdateStrategy : IInventoryUpdateStrategy
    {
        public virtual void UpdateInventory(Item item)
        {
            AdjustSellInDays(item);
            AdjustQuality(item);
            if (item.SellIn < 0)
                AdjustQuality(item);
        }

        protected virtual void AdjustSellInDays(Item item)
        {
            item.SellIn--;
        }

        protected virtual void AdjustQuality(Item item) { }
    }

    public class LegendaryItemInventoryUpdateStrategy : DefaultInventoryUpdateStrategy
    {
        public override void UpdateInventory(Item item)
        {
            
        }
    }

    public class PerishableQualityItemInventoryUpdateStrategy : DefaultInventoryUpdateStrategy
    {
        protected override void AdjustQuality(Item item)
        {
            DecrementQuality(item);
        }

        protected static void  DecrementQuality(Item item)
        {
            if (item.Quality > 0)
                item.Quality--;
        }
    }

    public class ConjuredItemInventoryUpdateStrategy : PerishableQualityItemInventoryUpdateStrategy
    {
        protected override void AdjustQuality(Item item)
        {
            DecrementQuality(item);
            DecrementQuality(item);
        }
    }

    public class ImprovingQualityItemInventoryUpdateStrategy : DefaultInventoryUpdateStrategy
    {
        protected override void AdjustQuality(Item item)
        {
            IncrementQuality(item);
        }

        private static void IncrementQuality(Item item)
        {
            if (item.Quality < 50)
                item.Quality++;
        }
    }

    public class BackStagePassItemInventoryUpdateStrategy : ImprovingQualityItemInventoryUpdateStrategy
    {
        public override void UpdateInventory(Item item)
        {
            base.UpdateInventory(item);
            if (item.SellIn < 11)
            {
                AdjustQuality(item);
            }
            if (item.SellIn < 6)
            {
                AdjustQuality(item);
            }
            if (item.SellIn < 0)
            {
                item.Quality = 0;
            }
        }
    }
}
