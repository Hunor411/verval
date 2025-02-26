namespace DatesAndStuff.Tests;

[TestFixture]
public class SimulationTimeTests
{
    [OneTimeSetUp]
    public void OneTimeSetupStuff()
    {
        // 
    }

    [SetUp]
    public void Setup()
    {
        // minden teszt felteheti, hogz elotte lefutott ez
    }

    [TearDown]
    public void TearDown()
    {
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
    }

    [TestFixture]
    public class ConstructionTests
    {
        [Test]
        // Default time is not current time.
        public void SimulationTime_DefaultTime_IsNotCurrentTime()
        {
            throw new NotImplementedException();
        }
        
        [Test]
        public void ToString_ShouldReturnCorrectFormat()
        {
            throw new NotImplementedException();
        }
    }
    
    [TestFixture]
    public class OperatorTests
    {
        [Test]
        // equal
        // not equal
        // <
        // >
        // <= different
        // >= different 
        // <= same
        // >= same
        // max
        // min
        public void GivenTwoEqualSimulationTimes_WhenCompared_ThenTheyShouldBeEqual()
        {
            var time1 = new SimulationTime(2024, 2, 26, 10, 0, 0);
            var time2 = new SimulationTime(2024, 2, 26, 10, 0, 0);

            Assert.AreEqual(time1, time2, "The two SimulationTime instances should be equal.");
            Assert.IsTrue(time1 == time2, "'==' operator should return true for equal values.");
            Assert.IsFalse(time1 != time2, "'!=' operator should return false for equal values.");
        }

        [Test]
        public void GivenTwoDifferentSimulationTimes_WhenCompared_ThenTheyShouldNotBeEqual()
        {
            var time1 = new SimulationTime(2024, 2, 26, 10, 0, 0);
            var time2 = new SimulationTime(2024, 2, 26, 10, 0, 1);

            Assert.AreNotEqual(time1, time2, "The two SimulationTime instances should be different.");
            Assert.IsFalse(time1 == time2, "'==' operator should return false for different values.");
            Assert.IsTrue(time1 != time2, "'!=' operator should return true for different values.");
        }
    }

    [TestFixture]
    private class TimeSpanArithmeticTests
    {

        [Test]
        // TimeSpanArithmetic
        // add
        // substract
        // Given_When_Then
        public void SimulationTime_Addition_ShiftsTimeCorrectly()
        {
            // UserSignedIn_OrderSent_OrderIsRegistered
            // DBB, specflow, cucumber, gherkin

            // Arrange
            DateTime baseDate = new DateTime(2010, 8, 23, 9, 4, 49);
            SimulationTime sut = new SimulationTime(baseDate);

            var ts = TimeSpan.FromMilliseconds(4544313);

            // Act
            var result = sut + ts;

            // Assert
            var expectedDateTime = baseDate + ts;
            Assert.AreEqual(expectedDateTime, result.ToAbsoluteDateTime());
        }

        [Test]
        //Method_Should_Then
        public void GivenSimulationTime_WhenSubtracted_ThenTimeIsShiftedBack()
        {
            // code kozelibb
            // RegisterOrder_SignedInUserSendsOrder_OrderIsRegistered
            throw new NotImplementedException();
        }
    }

    [TestFixture]
    public class TimeManipulationTests
    {
        [Test]
        // simulation difference timespane and datetimetimespan is the same
        public void GivenSimulationTime_WhenMillisecondsAreAdded_ThenTimeIncreases()
        {
            var baseTime = new SimulationTime(2025, 1, 1, 0, 0, 0);
            var updatedTime = baseTime.AddMilliseconds(1000);
            
            Assert.AreNotEqual(baseTime, updatedTime, "Adding milliseconds should change the SimulationTime.");
        }
        
        [Test]
        // millisecond representation works
        public void SimulationTime_MillisecondRepresentation_WorksCorrectly()
        {
            // Arrange
            var baseTime = SimulationTime.MinValue;
            var expectedTime = baseTime.AddMilliseconds(10);

            // Act
            var actualTime = SimulationTime.MinValue + TimeSpan.FromMilliseconds(10);

            // Assert
            Assert.AreEqual(expectedTime, actualTime, "Adding 10 milliseconds should correctly update the SimulationTime.");
        }
        
        [Test]
        // next millisec calculation works
        public void GivenSimulationTime_WhenNextMillisecCalled_ThenReturnsIncrementedTime()
        {
            // Arrange
            var time = new SimulationTime(2025, 1, 1, 1, 0, 0);

            // Act
            var nextMiliseconds = time.NextMillisec;

            // Assert
            Assert.That(nextMiliseconds.TotalMilliseconds, Is.EqualTo(time.TotalMilliseconds + 1));
        }
        
        [Test]
        // creat a SimulationTime from a DateTime, add the same milliseconds to both and check if they are still equal
        public void SimulationTime_AddMilliseconds_UpdatesTimeCorrectly()
        {
            // Arrenge
            var baseTime = new SimulationTime(2025, 1, 1, 1, 0, 0);
            var expectedTime = baseTime.AddMilliseconds(500);
            
            // Act
            var actualTime = baseTime + TimeSpan.FromMilliseconds(500);

            // Assert
            Assert.That(actualTime, Is.EqualTo(expectedTime));
        }
        
        [Test]
        // the same as before just with seconds
        public void GivenSimulationTime_WhenSecondsAdded_ThenTimeUpdatesCorrectly()
        {
            throw new NotImplementedException();
        }
        
        [Test]
        // same as before just with timespan
        public void SimulationTime_AddTimeSpan()
        {
            throw new NotImplementedException();
        }
        
        [Test]
        // check string representation given by ToString
        public void GivenSimulationTime_WhenToStringCalled_ThenReturnsCorrectFormat()
        {
            throw new NotImplementedException();
        }
    }
}
    