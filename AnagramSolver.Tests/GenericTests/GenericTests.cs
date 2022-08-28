using GenericTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.Tests.GenericTests
{
    public class GenericTests
    {
        [SetUp]
        public void SetUp()
        {

        }

        [TestCase(1, Gender.Male)]
        [TestCase(2, Gender.Female)]
        [TestCase(3, Gender.Other)]
        [TestCase(4, 4)]
        public void MapValueToEnum_ValueIsPartOfGenderEnum_ReturnsValue(int value, Gender output)
        {
            var result = GenericTasks.MapValueToEnum<Gender, int>(value);

            Assert.That(result, Is.EqualTo(output));
        }

        [TestCase("0", Weekday.Monday)]
        [TestCase("1", Weekday.Tuesday)]
        [TestCase("2", Weekday.Wednesday)]
        [TestCase("3", Weekday.Thursday)]
        [TestCase("4", Weekday.Friday)]
        [TestCase("5", Weekday.Saturday)]
        [TestCase("6", Weekday.Sunday)]
        public void MapValueToEnum_ValueIsStringAndPartOfWeekdayEnum_ReturnsValue(string value, Weekday output)
        {
            var result = GenericTasks.MapValueToEnum<Weekday, string>(value);

            Assert.That(result, Is.EqualTo(output));
        }

        [TestCase(0, Weekday.Monday)]
        [TestCase(1, Weekday.Tuesday)]
        [TestCase(2, Weekday.Wednesday)]
        [TestCase(3, Weekday.Thursday)]
        [TestCase(4, Weekday.Friday)]
        [TestCase(5, Weekday.Saturday)]
        [TestCase(6, Weekday.Sunday)]
        public void MapValueToEnum_ValueIsIntAndPartOfWeekdayEnum_ReturnsValue(int value, Weekday output)
        {
            var result = GenericTasks.MapValueToEnum<Weekday, int>(value);

            Assert.That(result, Is.EqualTo(output));
        }

        [TestCase("")]
        [TestCase("abc")]
        public void MapValueToEnum_ValueIsNotPartOfWeekdayEnum_ThrowsError(string value)
        {
            Assert.Throws<Exception>(() => GenericTasks.MapValueToEnum<Weekday, string>(value));
        }

    }
}
