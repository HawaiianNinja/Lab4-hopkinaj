using System;
using NUnit.Framework;
using Expedia;
using Rhino.Mocks;

namespace ExpediaTest
{
	[TestFixture()]
	public class CarTest
	{	
		private Car targetCar;
		private MockRepository mocks;
		
		[SetUp()]
		public void SetUp()
		{
			targetCar = new Car(5);
			mocks = new MockRepository();
		}
		
		[Test()]
		public void TestThatCarInitializes()
		{
			Assert.IsNotNull(targetCar);
		}	
		
		[Test()]
		public void TestThatCarHasCorrectBasePriceForFiveDays()
		{
			Assert.AreEqual(50, targetCar.getBasePrice()	);
		}
		
		[Test()]
		public void TestThatCarHasCorrectBasePriceForTenDays()
		{
            var target = new Car(10);
			Assert.AreEqual(80, target.getBasePrice());	
		}
		
		[Test()]
		public void TestThatCarHasCorrectBasePriceForSevenDays()
		{
			var target = new Car(7);
			Assert.AreEqual(10*7*.8, target.getBasePrice());
		}
		
		[Test()]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestThatCarThrowsOnBadLength()
		{
			new Car(-5);
		}

        [Test()]
        public void TestThatCarDoesGetCarLocationFromTheDatabase()
        {
            IDatabase mockDatabase = mocks.Stub<IDatabase>();
            int car1 = 10;
            String car1Location = "Hawaii";
            
            int car2 = 50;
            String car2Location = "Indiana";

            using (mocks.Record())
            {
                mockDatabase.getCarLocation(car1);
                LastCall.Return(car1Location);

                mockDatabase.getCarLocation(car2);
                LastCall.Return(car2Location);
            }

            var target = new Car(10);
            target.Database = mockDatabase;

            String result;
            result = target.getCarLocation(10);
            Assert.AreEqual(result, car1Location);

            result = target.getCarLocation(50);
            Assert.AreEqual(result, car2Location);
        }

        [Test()]
        public void TestThatCarDoesGetMillageFromDatabase()
        {
            IDatabase mockDatabase = mocks.Stub<IDatabase>();
            mockDatabase.Miles = 2000;
            var target = new Car(10);
            target.Database = mockDatabase;
            int milesTravled = target.Mileage;
            Assert.AreEqual(2000, milesTravled);
        }

        [Test()]
        public void TestObjectMother()
        {
            var target = ObjectMother.BMW();
            Assert.AreEqual(target.Name, "BMW M5 Sports Car");
        }

        [Test()]
        public void TestObjectMotherWithDatabase()
        {
            IDatabase mockDatabase = mocks.Stub<IDatabase>();
            mockDatabase.Miles = 2000;
            var target = ObjectMother.BMW();
            target.Database = mockDatabase;
            int milesTravled = target.Mileage;
            Assert.AreEqual(2000, milesTravled);
        }
	}
}
