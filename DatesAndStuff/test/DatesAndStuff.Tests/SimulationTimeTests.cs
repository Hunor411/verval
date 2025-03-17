using FluentAssertions;

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
            // Arrange
            SimulationTime defaultTime = new SimulationTime();
            DateTime now = DateTime.Now;

            // Act
            DateTime simulationDateTime = defaultTime.ToAbsoluteDateTime();

            // Assert
            simulationDateTime.Should().NotBeCloseTo(now, TimeSpan.FromSeconds(5),
                "The default SimulationTime should not be equal to the current real-world time.");
        }
        
        [Test]
        public void ToString_ShouldReturnCorrectFormat()
        {
            // Arrange
            var simulationTime = new SimulationTime(2025, 1, 1, 1, 0, 0);
            string expectedFormat = simulationTime.ToAbsoluteDateTime().ToString("yyyy-MM-ddTHH:mm:ss");

            // Act
            string actualString = simulationTime.ToString();

            // Assert
            actualString.Should().Be(expectedFormat, 
                $"SimulationTime.ToString() should return the correct format, but was '{actualString}'.");
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
            // Arrange
            var time1 = new SimulationTime(2024, 2, 26, 10, 0, 0);
            var time2 = new SimulationTime(2024, 2, 26, 10, 0, 0);
            
            // Assert
            time1.Should().Be(time2, "The two SimulationTime instances should be equal.");
        }

        [Test]
        public void GivenTwoDifferentSimulationTimes_WhenCompared_ThenTheyShouldNotBeEqual()
        {
            // Arrange
            var time1 = new SimulationTime(2024, 2, 26, 10, 0, 0);
            var time2 = new SimulationTime(2024, 2, 26, 10, 0, 1);
            
            // Assert
            time1.Should().NotBe(time2, "The two SimulationTime instances should be different.");
        }
    }

    [TestFixture]
    public class TimeSpanArithmeticTests
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
            expectedDateTime.Should().Be(result.ToAbsoluteDateTime(), "Adding a TimeSpan should correctly shift the SimulationTime.");
        }

        [Test]
        //Method_Should_Then
        public void GivenSimulationTime_WhenSubtracted_ThenTimeIsShiftedBack()
        {
            // Arrange
            DateTime baseDate = new DateTime(2024, 2, 26, 10, 0, 0);
            SimulationTime sut = new SimulationTime(baseDate);
    
            TimeSpan ts = TimeSpan.FromMilliseconds(5000);

            // Act
            SimulationTime result = sut - ts;

            // Assert
            DateTime expectedDateTime = baseDate - ts;
            expectedDateTime.Should().Be(result.ToAbsoluteDateTime(), 
                "Subtracting a TimeSpan should correctly shift the SimulationTime backwards.");
        }
    }

    [TestFixture]
    public class TimeManipulationTests
    {
        [Test]
        // simulation difference timespane and datetimetimespan is the same
        public void GivenSimulationTime_WhenMillisecondsAreAdded_ThenTimeIncreases()
        {
            // Arrange
            var baseTime = new SimulationTime(2025, 1, 1, 0, 0, 0);
            
            // Act
            var updatedTime = baseTime.AddMilliseconds(1000);
            
            // Assert
            baseTime.Should().NotBe(updatedTime, "Adding milliseconds should change the SimulationTime.");
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
            expectedTime.Should().Be(actualTime, "Adding 10 milliseconds should correctly update the SimulationTime.");
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
            nextMiliseconds.TotalMilliseconds.Should().Be(time.TotalMilliseconds + 1,
                "NextMillisec should increase the total milliseconds by exactly 1.");
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
            actualTime.Should().Be(expectedTime);
        }
        
        [Test]
        // the same as before just with seconds
        public void GivenSimulationTime_WhenSecondsAdded_ThenTimeUpdatesCorrectly()
        {
            // Arrange
            var baseTime = new SimulationTime(2025, 1, 1, 1, 0, 0); // Initial time
            var expectedTime = baseTime.AddSeconds(30); // Expected time after adding 30 seconds

            // Act
            var actualTime = baseTime + TimeSpan.FromSeconds(30); // Add 30 seconds to baseTime

            // Assert
            actualTime.Should().Be(expectedTime, 
                "Adding seconds to SimulationTime should correctly update the time.");
        }
        
        [Test]
        // same as before just with timespan
        public void SimulationTime_AddTimeSpan()
        {
            // Arrange
            var baseTime = new SimulationTime(2025, 1, 1, 1, 0, 0);
            var timeSpan = TimeSpan.FromMinutes(15);
            var expectedTime = baseTime.AddTimeSpan(timeSpan);

            // Act
            var actualTime = baseTime + timeSpan;

            // Assert
            actualTime.Should().Be(expectedTime, 
                "Adding a TimeSpan to SimulationTime should correctly update the time.");
        }
    }
}
