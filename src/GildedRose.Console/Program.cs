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
            foreach(var item in Items)
            {
                if (item.Name == "Sulfuras, Hand of Ragnaros")
                {
                    continue;
                }

                if (item.Name != "Aged Brie" && item.Name != "Backstage passes to a TAFKAL80ETC concert")
                {
                    DecrementQuality(item);                                            
                }
                else
                {
                    IncrementQuality(item);

                    if (item.Name == "Backstage passes to a TAFKAL80ETC concert")
                    {
                        if (item.SellIn < 11)
                        {
                            IncrementQuality(item);                            
                        }

                        if (item.SellIn < 6)
                        {
                            IncrementQuality(item);                            
                        }
                    }
                    
                }

                item.SellIn = item.SellIn - 1;
                
                if (item.SellIn < 0)
                {
                    if (item.Name != "Aged Brie" && item.Name != "Backstage passes to a TAFKAL80ETC concert")
                    {
                        DecrementQuality(item);                        
                    }
                    else
                    {
                        IncrementQuality(item);
                        if (item.Name == "Backstage passes to a TAFKAL80ETC concert")
                        {
                            if (item.SellIn < 0)
                            {
                                item.Quality = 0;
                            }                           
                        }
                    }
                }
            }
        }

        private static void IncrementQuality(Item item)
        {
            if (item.Quality < 50)
                item.Quality++;
        }

        private static void DecrementQuality(Item item)
        {
            if (item.Quality > 0)
                item.Quality--;
        }
    }

    public class Item
    {
        public string Name { get; set; }

        public int SellIn { get; set; }

        public int Quality { get; set; }
    }

}
