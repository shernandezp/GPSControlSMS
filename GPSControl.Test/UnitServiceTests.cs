// Copyright (c) 2025 Sergio Hernandez. All rights reserved.
//
//  Licensed under the Apache License, Version 2.0 (the "License").
//  You may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
//

using GPSControlSMS.Data;
using GPSControlSMS.Interfaces;
using GPSControlSMS.Services;
using GPSControlSMS.Utils;
using Moq;

namespace GPSControl.Test;

[TestFixture]
public class UnitServiceTests
{
    private Mock<IStorageService> _mockStorageService;
    private UnitService _unitService;

    [SetUp]
    public void SetUp()
    {
        _mockStorageService = new Mock<IStorageService>();
        _unitService = new UnitService(_mockStorageService.Object);
    }

    [Test]
    public void AddEditUnit_AddsNewUnit_WhenUnitDoesNotExist()
    {
        // Arrange
        var unit = new UnitModel("Unit1", "1234567890", "Device1");
        _mockStorageService.Setup(s => s.GetList<UnitModel>(Constants.UnitStorageKey)).Returns([]);

        // Act
        _unitService.AddEditUnit(unit);

        // Assert
        _mockStorageService.Verify(s => s.SetList(Constants.UnitStorageKey, It.Is<IEnumerable<UnitModel>>(u => u.Count() == 1 && u.First().Name == "Unit1")), Times.Once);
    }

    [Test]
    public void AddEditUnit_EditsExistingUnit_WhenUnitExists()
    {
        // Arrange
        var existingUnit = new UnitModel("Unit1", "1234567890", "Device1");
        var updatedUnit = new UnitModel("Unit1", "0987654321", "Device2");
        _mockStorageService.Setup(s => s.GetList<UnitModel>(Constants.UnitStorageKey)).Returns([existingUnit]);

        // Act
        _unitService.AddEditUnit(updatedUnit);

        // Assert
        _mockStorageService.Verify(s => s.SetList(Constants.UnitStorageKey, It.Is<IEnumerable<UnitModel>>(u => u.Count() == 1 && u.First().PhoneNumber == "0987654321" && u.First().Device == "Device2")), Times.Once);
    }

    [Test]
    public void GetUnits_ReturnsUnits_WhenUnitsExist()
    {
        // Arrange
        var units = new List<UnitModel> { new UnitModel("Unit1", "1234567890", "Device1") };
        _mockStorageService.Setup(s => s.GetList<UnitModel>(Constants.UnitStorageKey)).Returns(units);

        // Act
        var result = _unitService.GetUnits();

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Name, Is.EqualTo("Unit1"));
        }
    }

    [Test]
    public void GetUnits_ReturnsDefaultUnit_WhenNoUnitsExist()
    {
        // Arrange
        _mockStorageService.Setup(s => s.GetList<UnitModel>(Constants.UnitStorageKey)).Returns([]);

        // Act
        var result = _unitService.GetUnits();

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Name, Is.EqualTo(Constants.DefaultUnit));
        }
    }

    [Test]
    public void GetUnit_ReturnsUnit_WhenUnitExists()
    {
        // Arrange
        var unit = new UnitModel("Unit1", "1234567890", "Device1");
        _mockStorageService.Setup(s => s.GetList<UnitModel>(Constants.UnitStorageKey)).Returns([unit]);

        // Act
        var result = _unitService.GetUnit("Unit1");

        // Assert
        Assert.That(result.Name, Is.EqualTo("Unit1"));
    }

    [Test]
    public void GetUnit_ReturnsDefaultUnit_WhenUnitDoesNotExist()
    {
        // Arrange
        _mockStorageService.Setup(s => s.GetList<UnitModel>(Constants.UnitStorageKey)).Returns([]);

        // Act
        var result = _unitService.GetUnit("NonExistentUnit");

        // Assert
        Assert.That(result.Name, Is.EqualTo(Constants.DefaultUnit));
    }
}
