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
using GPSControlSMS.Utils;

namespace GPSControlSMS.ViewModels;

/// <summary>
/// View model for the sender page
/// </summary>
/// <param name="displayService"></param>
/// <param name="senderService"></param>
/// <param name="unitService"></param>
public partial class SenderViewModel(
    IDisplayService displayService,
    ISenderService senderService,
    IUnitService unitService,
    ILocalizationResourceManager localization) : BaseViewModel
{

    #region Observable Properties

    // Represents the current unit model
    [ObservableProperty]
    public UnitModel unit = new(string.Empty, string.Empty, string.Empty);

    // Indicates if the password is visible
    [ObservableProperty]
    public bool isPasswordVisible;

    // List of unit names
    [ObservableProperty]
    public List<string> names = [];

    // List of available devices
    public IEnumerable<string> devices = Constants.Devices;

    // List of commands for the selected device
    [ObservableProperty]
    public IEnumerable<CommandModel> commandList = [];

    // New unit name to be added
    [ObservableProperty]
    public string newName = string.Empty;

    #endregion Observable Properties

    #region Properties

    // Selected unit name
    private string selectedUnit = string.Empty;
    public string SelectedUnit
    {
        get => selectedUnit;
        set
        {
            if (SetProperty(ref selectedUnit, value))
            {
                OnUnitModelChanged();
            }
        }
    }

    // Selected device name
    private string selectedDevice = string.Empty;
    public string SelectedDevice
    {
        get => selectedDevice;
        set
        {
            if (SetProperty(ref selectedDevice, value))
            {
                LoadCommands();
            }
        }
    }
    #endregion Properties

    /// <summary>
    /// Initialize the view model with default values
    /// </summary>
    /// <returns></returns>
    public async Task InitializeAsync()
    {
        // Load units and default values
        var units = unitService.GetUnits();
        var defaultUnit = units.First();
        Names = units.Select(d => d.Name).ToList();
        Unit = defaultUnit;
        SelectedDevice = defaultUnit.Device;
        SelectedUnit = defaultUnit.Name;
    }

    /// <summary>
    /// Toggle the visibility of the password
    /// </summary>
    public void TogglePasswordVisibility()
    {
        IsPasswordVisible = !IsPasswordVisible;
    }

    /// <summary>
    /// Add a new unit
    /// </summary>
    /// <returns></returns>
    public async Task AddUnit()
    {
        if (string.IsNullOrEmpty(NewName))
        {
            var errorMessage = localization["EmptyUnit"];
            await displayService.ShowAlert("Error", errorMessage, "OK");
            return;
        }
        if (Names.Contains(NewName))
        {
            var errorMessage = localization["ExistingUnit"];
            await displayService.ShowAlert("Error", errorMessage, "OK");
            return;
        }

        if (Names.Count == 1 && Names[0].Equals(Constants.DefaultUnit))
        {
            Names = [NewName];
        }
        else
        {
            Names.Add(NewName);
        }
        SelectedUnit = NewName;
        NewName = string.Empty;
        var message = localization["UnitAdded"];
        var messageLabel = localization["MessageLabel"];
        await displayService.ShowAlert(messageLabel, message, "OK");
    }

    /// <summary>
    /// Save the current unit
    /// </summary>
    /// <returns></returns>
    public async Task Save()
    {
        if (string.IsNullOrEmpty(Unit.PhoneNumber))
        {
            var errorMessage = localization["PhoneNumberMissing"];
            await displayService.ShowAlert("Error", errorMessage, "OK");
            return;
        }
        if (SelectedUnit.Equals(Constants.DefaultUnit))
        {
            var errorMessage = localization["EmptyUnit"];
            await displayService.ShowAlert("Error", errorMessage, "OK");
            return;
        }
        unitService.AddEditUnit(Unit);
        var message = localization["UnitSaved"];
        var messageLabel = localization["MessageLabel"];
        await displayService.ShowAlert(messageLabel, message, "OK");
    }

    /// <summary>
    /// Send a SMS with the selected command
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task SelectCommand(CommandModel command)
    {
        if (string.IsNullOrEmpty(Unit.PhoneNumber))
        {
            var errorMessage = localization["PhoneNumberMissing"];
            await displayService.ShowAlert("Error", errorMessage, "OK");
            return;
        }

        var confirmationMessage = localization["ConfirmFactory"];
        var yes = localization["YesLabel"];
        var no = localization["NoLabel"];
        var confirmation = localization["ConfirmationLabel"];
        if (await displayService.ShowConfirmationAlert(confirmation, confirmationMessage, yes, no))
        {
            // Replace placeholder of the password
            var message = command.Definition.Replace("******", Unit.Password);

            // Send SMS
            await senderService.SendSMSAsync(Unit.PhoneNumber, message);
        }
    }

    /// <summary>
    /// Handle changes to the selected unit
    /// </summary>
    public void OnUnitModelChanged()
    {
        Unit = unitService.GetUnit(SelectedUnit);
        SelectedDevice = Unit.Device;
    }

    /// <summary>
    /// Load commands for the selected device
    /// </summary>
    public void LoadCommands()
    {
        Unit.Device = SelectedDevice;
        CommandList = SelectedDevice.GetCommands();
        // Replace the Name of each Command with the localized value
        foreach (var command in CommandList)
        {
            command.Label = localization[command.Name];
        }
    }

}
