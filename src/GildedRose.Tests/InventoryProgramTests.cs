using System.Collections.Generic;
using NUnit.Framework;
using GildedRose.Console;

namespace GildedRose.Tests
{
    // Hi and welcome to team Gilded Rose. As you know, 
    // we are a small inn with a prime location in a prominent city ran by a friendly innkeeper named Allison. 
    // We also buy and sell only the finest goods. 
    // Unfortunately, our goods are constantly degrading in quality as they approach their sell by date. 
    // We have a system in place that updates our inventory for us. 
    // It was developed by a no-nonsense type named Leeroy, who has moved on to new adventures. 
    // Your task is to add the new feature to our system so that we can begin selling a new category of items. 
    // First an introduction to our system:

    //    - All items have a SellIn value which denotes the number of days we have to sell the item
    //    - All items have a Quality value which denotes how valuable the item is
    //    - At the end of each day our system lowers both values for every item

    // Pretty simple, right? Well this is where it gets interesting:

    //    - Once the sell by date has passed, Quality degrades twice as fast
    //    - The Quality of an item is never negative
    //    - "Aged Brie" actually increases in Quality the older it gets
    //    - The Quality of an item is never more than 50
    //    - "Sulfuras", being a legendary item, never has to be sold or decreases in Quality
    //    - "Backstage passes", like aged brie, increases in Quality as it's SellIn value approaches; 
    //        Quality increases by 2 when there are 10 days or less 
    //        and by 3 when there are 5 days or less 
    //        but Quality drops to 0 after the concert

    // We have recently signed a supplier of conjured items. This requires an update to our system:

    //    - "Conjured" items degrade in Quality twice as fast as normal items

    // Feel free to make any changes to the UpdateQuality method 
    // and add any new code as long as everything still works correctly. 
    // However, do not alter the Item class or Items property 
    // as those belong to the goblin in the corner who will insta-rage 
    // and one-shot you as he doesn't believe in shared code ownership 
    // you can make the UpdateQuality method and Items property static if you like, we'll cover for you.
    // Your work needs to be completed by Friday, February 18, 2011 08:00:00 AM PST.

    // Just for clarification, 
    // an item can never have its Quality increase above 50, 
    // however "Sulfuras" is a legendary item and as such its Quality is 80 and it never alters.
    [TestFixture]
    public class InventoryProgramTests
    {
        private readonly Item Item1 = new Item { Name = "Item1", SellIn = 1 };
        private readonly Item Item2 = new Item { Name = "Item2", SellIn = 1 };
        private readonly Item Sulfuras = new Item { Name = "Sulfuras, Hand of Ragnaros", SellIn = 1 };

        [Test]
        public void TestWeCanFindItemByName()
        {
            var inventoryProgram = new Program(new List<Item> { Item1, Item2 });
            var item = inventoryProgram.FindItemByName("Item2");
            Assert.IsNotNull(item);
            Assert.AreEqual("Item2", item.Name);
        }

        [Test]
        public void TestWeCanFindItemByNameReturnsNullWhenThereIsNoMatch()
        {
            var inventoryProgram = new Program(new List<Item> { Item1, Item2 });
            var item = inventoryProgram.FindItemByName("NonExistentItem");
            Assert.IsNull(item);            
        }

        [Test]
        public void TestWeCanFindItemByNameReturnsNullWhenThereAreNoItems()
        {
            var inventoryProgram = new Program();
            var item = inventoryProgram.FindItemByName("NonExistentItem");
            Assert.IsNull(item);
        }

        private Item UpdateQualityReturnItem(IList<Item> items, string itemName)
        {
            var inventoryProgram = new Program(items);
            inventoryProgram.UpdateQuality();
            return inventoryProgram.FindItemByName(itemName);
        }

        [Test]
        public void TestThatSellInDaysIsReducedWhenItemNameIsNotSulfuras()
        {
            var item = UpdateQualityReturnItem(new List<Item> { Item1 }, Item1.Name);
            Assert.AreEqual(0, item.SellIn, "SellIn days was not reduced.");
        }

        [Test]
        public void TestThatSellInDaysIsNotReducedForSulfuras()
        {
            var item = UpdateQualityReturnItem(new List<Item> { Sulfuras }, Sulfuras.Name);
            Assert.AreEqual(1, item.SellIn, "SellIn days was reduced for 'Sulfuras, Hand of Ragnaros'.");
        }
    }
}