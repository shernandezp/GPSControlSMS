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

using Moq;
using GPSControlSMS.Data;
using GPSControlSMS.Interfaces;
using GPSControlSMS.Utils;
using GPSControlSMS.ViewModels;

namespace GPSControlSMS.Tests;

[TestFixture]
public class SenderViewModelTests
{
    private Mock<IDisplayService> _mockDisplayService;
    private Mock<ISenderService> _mockSenderService;
    private Mock<IUnitService> _mockUnitService;
    private Mock<ILocalizationResourceManager> _mockLocalization;
    private SenderViewModel _viewModel;

    [SetUp]
    public void SetUp()
    {
        _mockDisplayService = new Mock<IDisplayService>();
        _mockSenderService = new Mock<ISenderService>();
        _mockUnitService = new Mock<IUnitService>();
        _mockLocalization = new Mock<ILocalizationResourceManager>();
        _viewModel = new SenderViewModel(
            _mockDisplayService.Object,
            _mockSenderService.Object,
            _mockUnitService.Object,
            _mockLocalization.Object);
    }

    [Test]
    public async Task InitializeAsync_ShouldLoadUnitsAndDefaultValues()
    {
        // Arrange
        var units = new List<UnitModel>
        {
            new("Unit1", "1234567890", "Device1")
        };
        _mockUnitService.Setup(s => s.GetUnits()).Returns(units);
        _mockUnitService.Setup(s => s.GetUnit("Unit1")).Returns(units.First());

        // Act
        await _viewModel.InitializeAsync();

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(units.First(), Is.EqualTo(_viewModel.Unit));
            Assert.That(units.First().Device, Is.EqualTo(_viewModel.SelectedDevice));
            Assert.That(units.First().Name, Is.EqualTo(_viewModel.SelectedUnit));
            Assert.That(_viewModel.Names, Is.EqualTo(units.Select(u => u.Name).ToList()).AsCollection);
        }
    }

    [Test]
    public void TogglePasswordVisibility_ShouldToggleIsPasswordVisible()
    {
        // Arrange
        _viewModel.IsPasswordVisible = false;

        // Act
        _viewModel.TogglePasswordVisibility();

        // Assert
        Assert.That(_viewModel.IsPasswordVisible);

        // Act
        _viewModel.TogglePasswordVisibility();

        // Assert
        Assert.That(_viewModel.IsPasswordVisible, Is.False);
    }

    [Test]
    public async Task AddUnit_ShouldShowError_WhenNewNameIsEmpty()
    {
        // Arrange
        _viewModel.NewName = string.Empty;
        _mockLocalization.Setup(l => l["EmptyUnit"]).Returns("Unit name cannot be empty");

        // Act
        await _viewModel.AddUnit();

        // Assert
        _mockDisplayService.Verify(d => d.ShowAlert("Error", "Unit name cannot be empty", "OK"), Times.Once);
    }

    [Test]
    public async Task AddUnit_ShouldShowError_WhenNewNameAlreadyExists()
    {
        // Arrange
        _viewModel.NewName = "ExistingUnit";
        _viewModel.Names = ["ExistingUnit"];
        _mockLocalization.Setup(l => l["ExistingUnit"]).Returns("Unit name already exists");

        // Act
        await _viewModel.AddUnit();

        // Assert
        _mockDisplayService.Verify(d => d.ShowAlert("Error", "Unit name already exists", "OK"), Times.Once);
    }

    [Test]
    public async Task AddUnit_ShouldAddNewUnit_WhenValid()
    {
        // Arrange
        var unit = new UnitModel("NewUnit", "1234567890", "Device1");
        _mockUnitService.Setup(s => s.GetUnit("NewUnit")).Returns(unit);
        _viewModel.NewName = "NewUnit";
        _viewModel.Names = ["DefaultUnit"];
        _mockLocalization.Setup(l => l["UnitAdded"]).Returns("Unit added successfully");
        _mockLocalization.Setup(l => l["MessageLabel"]).Returns("Message");

        // Act
        await _viewModel.AddUnit();

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(_viewModel.SelectedUnit, Is.EqualTo("NewUnit"));
            Assert.That(_viewModel.NewName, Is.Empty);
        }
        _mockDisplayService.Verify(d => d.ShowAlert("Message", "Unit added successfully", "OK"), Times.Once);
    }

    [Test]
    public async Task Save_ShouldShowError_WhenPhoneNumberIsEmpty()
    {
        // Arrange
        _viewModel.Unit.PhoneNumber = string.Empty;
        _mockLocalization.Setup(l => l["PhoneNumberMissing"]).Returns("Phone number is missing");

        // Act
        await _viewModel.Save();

        // Assert
        _mockDisplayService.Verify(d => d.ShowAlert("Error", "Phone number is missing", "OK"), Times.Once);
    }

    [Test]
    public async Task Save_ShouldShowError_WhenSelectedUnitIsDefault()
    {
        // Arrange
        var unit = new UnitModel(Constants.DefaultUnit, "1234567890", "Device1");
        _mockUnitService.Setup(s => s.GetUnit(Constants.DefaultUnit)).Returns(unit);
        _viewModel.Unit.PhoneNumber = "1234567890";
        _viewModel.SelectedUnit = Constants.DefaultUnit;
        _mockLocalization.Setup(l => l["EmptyUnit"]).Returns("Unit name cannot be default");

        // Act
        await _viewModel.Save();

        // Assert
        _mockDisplayService.Verify(d => d.ShowAlert("Error", "Unit name cannot be default", "OK"), Times.Once);
    }

    [Test]
    public async Task Save_ShouldSaveUnit_WhenValid()
    {
        // Arrange
        var unit = new UnitModel("ValidUnit", "1234567890", "Device1");
        _mockUnitService.Setup(s => s.GetUnit("ValidUnit")).Returns(unit);
        _viewModel.Unit.PhoneNumber = "1234567890";
        _viewModel.SelectedUnit = "ValidUnit";
        _mockLocalization.Setup(l => l["UnitSaved"]).Returns("Unit saved successfully");
        _mockLocalization.Setup(l => l["MessageLabel"]).Returns("Message");

        // Act
        await _viewModel.Save();

        // Assert
        _mockUnitService.Verify(s => s.AddEditUnit(_viewModel.Unit), Times.Once);
        _mockDisplayService.Verify(d => d.ShowAlert("Message", "Unit saved successfully", "OK"), Times.Once);
    }

    [Test]
    public async Task SelectCommand_ShouldShowError_WhenPhoneNumberIsEmpty()
    {
        // Arrange
        var command = new CommandModel("Command1", "Definition1");
        _viewModel.Unit.PhoneNumber = string.Empty;
        _mockLocalization.Setup(l => l["PhoneNumberMissing"]).Returns("Phone number is missing");

        // Act
        await _viewModel.SelectCommand(command);

        // Assert
        _mockDisplayService.Verify(d => d.ShowAlert("Error", "Phone number is missing", "OK"), Times.Once);
    }

    [Test]
    public async Task SelectCommand_ShouldSendSMS_WhenConfirmed()
    {
        // Arrange
        var command = new CommandModel("Command1", "Definition1");
        _viewModel.Unit.PhoneNumber = "1234567890";
        _viewModel.Unit.Password = "password";
        _mockLocalization.Setup(l => l["ConfirmFactory"]).Returns("Are you sure?");
        _mockLocalization.Setup(l => l["YesLabel"]).Returns("Yes");
        _mockLocalization.Setup(l => l["NoLabel"]).Returns("No");
        _mockLocalization.Setup(l => l["ConfirmationLabel"]).Returns("Confirmation");
        _mockDisplayService.Setup(d => d.ShowConfirmationAlert("Confirmation", "Are you sure?", "Yes", "No")).ReturnsAsync(true);

        // Act
        await _viewModel.SelectCommand(command);

        // Assert
        _mockSenderService.Verify(s => s.SendSMSAsync("1234567890", "Definition1"), Times.Once);
    }

    [Test]
    public async Task SelectCommand_ShouldNotSendSMS_WhenCommandNameIsFactoryAndNotConfirmed()
    {
        // Arrange
        _viewModel.Unit.PhoneNumber = "1234567890";
        _mockLocalization.Setup(l => l["ConfirmFactory"]).Returns("Are you sure you want to reset to factory settings?");
        _mockLocalization.Setup(l => l["YesLabel"]).Returns("Yes");
        _mockLocalization.Setup(l => l["NoLabel"]).Returns("No");
        _mockLocalization.Setup(l => l["ConfirmationLabel"]).Returns("Confirmation");

        _mockDisplayService.Setup(ds => ds.ShowConfirmationAlert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                          .ReturnsAsync(false);

        var command = new CommandModel("Factory", "Factory reset command");

        // Act
        await _viewModel.SelectCommand(command);

        // Assert
        _mockSenderService.Verify(ss => ss.SendSMSAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        _mockDisplayService.Verify(ds => ds.ShowConfirmationAlert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Test]
    public void OnUnitModelChanged_ShouldUpdateUnitAndSelectedDevice()
    {
        // Arrange
        var unit = new UnitModel("Unit1", "1234567890", "Device1");
        _mockUnitService.Setup(s => s.GetUnit("Unit1")).Returns(unit);
        _viewModel.SelectedUnit = "Unit1";

        // Act
        _viewModel.OnUnitModelChanged();

        // Assert
        Assert.That(unit, Is.EqualTo(_viewModel.Unit));
        Assert.That(unit.Device, Is.EqualTo(_viewModel.SelectedDevice));
    }

    [Test]
    public void LoadCommands_ShouldUpdateCommandList()
    {
        // Arrange
        var device = Constants.Devices.ElementAt(0);
        var commands = device.GetCommands();
        _viewModel.SelectedDevice = device;
        _mockLocalization.Setup(l => l["Command1"]).Returns("Localized Command1");

        // Act
        _viewModel.LoadCommands();

        // Assert
        Assert.That(commands, Has.Count.EqualTo(_viewModel.CommandList.Count()));
    }
}