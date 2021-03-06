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
        private readonly string SulfurasItemName = "Sulfuras, Hand of Ragnaros";
        private readonly string BrieItemName = "Aged Brie";
        private readonly string BackStagePassItemName = "Backstage passes to a TAFKAL80ETC concert";
        private readonly string ConjuredManaCakeItemName = "Conjured Mana Cake";

        private Item CreateItem1()
        {
            return new Item { Name = "Item1", SellIn = 1, Quality = 1 };
        }
        private Item CreateItem2()
        {
            return new Item { Name = "Item2", SellIn = 1 };
        }
        private Item CreateSulfuras()
        {
            return new Item { Name = SulfurasItemName, SellIn = 1, Quality = 1 };
        }
        private Item CreateBrie()
        {
            return new Item { Name = BrieItemName, SellIn = 1, Quality = 1 };
        }
        private Item CreateBackStagePass()
        {
            return new Item { Name = BackStagePassItemName, Quality = 1, SellIn = 15 };
        }
        private Item CreateConjuredItem()
        {
            return new Item { Name = ConjuredManaCakeItemName, SellIn = 3, Quality = 6 };
        }

        [Test]
        public void TestWeCanFindItemByName()
        {
            var inventoryProgram = new Program(new List<Item> { CreateItem1(), CreateItem2() });
            var item = inventoryProgram.FindItemByName("Item2");
            Assert.IsNotNull(item);
            Assert.AreEqual("Item2", item.Name);
        }

        [Test]
        public void TestWeCanFindItemByNameReturnsNullWhenThereIsNoMatch()
        {
            var inventoryProgram = new Program(new List<Item> { CreateItem1(), CreateItem2() });
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

        // - At the end of each day our system lowers both (Quality, SellIn) values for every item
        [Test]
        public void TestThatSellInDaysIsLoweredWhenItemNameIsNotSulfuras()
        {
            var item = UpdateQualityReturnItem(new List<Item> { CreateItem1() }, "Item1");
            Assert.AreEqual(0, item.SellIn, "SellIn days was not lowered.");
        }

        // - "Sulfuras", being a legendary item, never has to be sold or decreases in Quality
        [Test]
        public void TestThatSellInDaysIsNotLoweredForSulfuras()
        {
            var item = UpdateQualityReturnItem(new List<Item> { CreateSulfuras() }, SulfurasItemName);
            Assert.AreEqual(1, item.SellIn, string.Format("SellIn days was lowered for '{0}'.", SulfurasItemName));
        }

        // - At the end of each day our system lowers both (Quality, SellIn) values for every item
        [Test]
        public void TestThatQualityIsLoweredForNormalCase()
        {
            var item = UpdateQualityReturnItem(new List<Item> { CreateItem1() }, "Item1");
            Assert.AreEqual(0, item.Quality, "Quality was not lowered for normal case.");
        }

        // - "Sulfuras", being a legendary item, never has to be sold or decreases in Quality
        [Test]
        public void TestThatQualityIsNotLoweredForSulfuras()
        {
            var item = UpdateQualityReturnItem(new List<Item> { CreateSulfuras() }, SulfurasItemName);
            Assert.AreEqual(1, item.Quality, string.Format("Quality days was lowered for '{0}'.", SulfurasItemName));
        }

        // - The Quality of an item is never negative
        [Test]
        public void TestThatQualityIsNeverNegative()
        {
            var item = UpdateQualityReturnItem(new List<Item> { CreateItem2() }, "Item2");
            Assert.AreEqual(0, item.Quality, "Quality is negative.");
        }

        // - Once the sell by date has passed, Quality degrades twice as fast
        [Test]
        public void TestThatQualityIsLoweredAtTwiceTheRateWhenSellByDateIsPassed()
        {
            var item = CreateItem1();
            item.SellIn = 0;
            item.Quality = 2;
            item = UpdateQualityReturnItem(new List<Item> { item }, "Item1");
            Assert.AreEqual(0, item.Quality, "Quality did not reduce at twice rate for an item past its sell by date.");
        }

        // - At the end of each day our system lowers both (Quality, SellIn) values for every item
        // - "Conjured" items degrade in Quality twice as fast as normal items
        [Test]
        public void TestThatQualityIsLoweredBy2ForConjuredItem()
        {
            var item = UpdateQualityReturnItem(new List<Item> { CreateConjuredItem() }, ConjuredManaCakeItemName);
            Assert.AreEqual(4, item.Quality, "Quality was not lowered by 2 for conjured item.");
        }

        // - "Aged Brie" actually increases in Quality the older it gets
        [Test]
        public void TestThatAgedBrieIncreasesInQuality()
        {
            var item = UpdateQualityReturnItem(new List<Item> { CreateBrie() }, BrieItemName);
            Assert.AreEqual(2, item.Quality, "Quality was not increased for Brie.");
        }

        // - Once the sell by date has passed, Quality degrades twice as fast
        // - "Aged Brie" actually increases in Quality the older it gets
        // - Combining both "Aged Brie" past the sell by date increases in value twice as fast.
        [Test]
        public void TestThatAgedBrieIncreasesInQualityAtTwiceTheRateWhenSellByDateIsPassed()
        {
            var item = CreateBrie();
            item.SellIn = 0;
            item = UpdateQualityReturnItem(new List<Item> { item }, BrieItemName);
            Assert.AreEqual(3, item.Quality, "Quality did not increase at twice rate for an 'Aged Brie' past its sell by date.");
        }

        // - The Quality of an item is never more than 50
        [Test]
        public void TestThatQualityOfAnItemIsNeverMoreThan50()
        {
            var item = CreateBrie();
            item.Quality = 50;
            item = UpdateQualityReturnItem(new List<Item> { item }, BrieItemName);
            Assert.AreEqual(50, item.Quality, "Quality of an item increased beyond 50.");
        }

        // "Backstage passes", like aged brie, increases in Quality as it's SellIn value approaches; 
        [Test]
        public void TestThatQualityOfABackStagePassItemIncreasesBy1WhenSellInDaysIsMoreThan10()
        {
            var item = CreateBackStagePass();
            item = UpdateQualityReturnItem(new List<Item> { item }, BackStagePassItemName);
            Assert.AreEqual(2, item.Quality, "Quality of a backstage pass did not increase by 1 when sell in days is more than 10.");
        }

        // "Backstage passes", like aged brie, increases in Quality as it's SellIn value approaches; 
        // Quality increases by 2 when there are *10 days* or less
        [Test]
        public void TestThatQualityOfABackStagePassItemIncreasesBy2WhenSellInDays10()
        {
            var item = CreateBackStagePass();
            item.SellIn = 10;
            item = UpdateQualityReturnItem(new List<Item> { item }, BackStagePassItemName);
            Assert.AreEqual(3, item.Quality, "Quality of a backstage pass did not increase by 2 when sell in days is 10.");
        }

        // "Backstage passes", like aged brie, increases in Quality as it's SellIn value approaches; 
        // Quality increases by 2 when there are 10 days or *less*
        [Test]
        public void TestThatQualityOfABackStagePassItemIncreasesBy2WhenSellInDaysIsLessThan10()
        {
            var item = CreateBackStagePass();
            item.SellIn = 9;
            item = UpdateQualityReturnItem(new List<Item> { item }, BackStagePassItemName);
            Assert.AreEqual(3, item.Quality, "Quality of a backstage pass did not increase by 2 when sell in days is less than 10.");
        }

        // "Backstage passes", like aged brie, increases in Quality as it's SellIn value approaches; 
        // and by 3 when there are *5 days* or less 
        [Test]
        public void TestThatQualityOfABackStagePassItemIncreasesBy3WhenSellInDaysIs5()
        {
            var item = CreateBackStagePass();
            item.SellIn = 5;
            item = UpdateQualityReturnItem(new List<Item> { item }, BackStagePassItemName);
            Assert.AreEqual(4, item.Quality, "Quality of a backstage pass did not increase by 3 when sell in days is 5.");
        }

        // "Backstage passes", like aged brie, increases in Quality as it's SellIn value approaches; 
        // and by 3 when there are 5 days or *less*
        [Test]
        public void TestThatQualityOfABackStagePassItemIncreasesBy3WhenSellInDaysIsLessThan5()
        {
            var item = CreateBackStagePass();
            item.SellIn = 4;
            item = UpdateQualityReturnItem(new List<Item> { item }, BackStagePassItemName);
            Assert.AreEqual(4, item.Quality, "Quality of a backstage pass did not increase by 3 when sell in days is less than 5.");
        }

        // "Backstage passes", like aged brie, increases in Quality as it's SellIn value approaches; 
        // but Quality drops to 0 after the concert
        [Test]
        public void TestThatQualityOfABackStagePassItemDropsTo0AfterTheConcert()
        {
            var item = CreateBackStagePass();
            item.SellIn = 0;
            item = UpdateQualityReturnItem(new List<Item> { item }, BackStagePassItemName);
            Assert.AreEqual(0, item.Quality, "Quality of a backstage pass did not drop to 0 after the concert.");
        }

        // "Backstage passes", like aged brie, increases in Quality as it's SellIn value approaches; 
        // Corner case where quality is 47 all the 3 if conditions with asterisks are required even though the if condition is same
        //if (Items[i].Quality < 50) ************************************
        //{
        //    Items[i].Quality = Items[i].Quality + 1;

        //    if (Items[i].Name == "Backstage passes to a TAFKAL80ETC concert")
        //    {
        //        if (Items[i].SellIn < 11)
        //        {
        //            if (Items[i].Quality < 50)************************************
        //            {
        //                Items[i].Quality = Items[i].Quality + 1;
        //            }
        //        }

        //        if (Items[i].SellIn < 6)
        //        {
        //            if (Items[i].Quality < 50)************************************
        //            {
        //                Items[i].Quality = Items[i].Quality + 1;
        //            }
        //        }
        //    }
        //}
        [Test]
        public void TestThatQualityOfABackStagePassItemIncreasesBy3WhenSellInDaysIsLessThan5CornerCasesWhenQualityIs47()
        {
            var item = CreateBackStagePass();
            item.SellIn = 4;
            item.Quality = 47;
            item = UpdateQualityReturnItem(new List<Item> { item }, BackStagePassItemName);
            Assert.AreEqual(50, item.Quality, "Quality of a backstage pass did not increase by 3 when sell in days is less than 5.");
        }
    }
}